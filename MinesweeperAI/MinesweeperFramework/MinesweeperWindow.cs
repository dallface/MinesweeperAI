using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.Sessions;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using static MinesweeperFramework.MinesweeperWindow.Row;

namespace MinesweeperFramework
{
    public partial class MinesweeperWindow : IDisposable
    {
        private static Application _application;
        private static string _applicationDirectory = @"C:\Program Files\Microsoft Games\Minesweeper";
        private static string _applicationPath = Path.Combine(_applicationDirectory, "Minesweeper.exe");

        public static MinesweeperWindow Create()
        {
            Window window = getMainWindow();
            return new MinesweeperWindow(window);
        }

        private MinesweeperWindow(Window window)
        {
            GetTitleMenuElements(window.AutomationElement);
            GetApplicationMenuBar(getFirstDescendant(window.AutomationElement, "Application"));
            GetStatusInformationElements(getFirstDescendant(window.AutomationElement, "Status information"));
            

            List<AutomationElement> children = getChildren(window.AutomationElement).FindAll(x => x.Current.Name.Contains("Row")); ;
            TileGrid = new Grid(children);
        }

        private static Window getMainWindow()
        {
            Window MainWindow;
            ProcessStartInfo startinfo = new ProcessStartInfo(_applicationPath);
            _application = Application.AttachOrLaunch(startinfo);
            MainWindow = _application.GetWindows().Find(x => x.Name == "Minesweeper");
            if (MainWindow == null)
            {
                Window errorDialog = _application.GetWindow("The game is running in software rendering mode");
                var dialog = errorDialog.Items[0];
                InvokeClick(dialog.AutomationElement);
                MainWindow = _application.GetWindow("Minesweeper");
            }
            return MainWindow;
        }

        public static List<AutomationElement> getChildren(AutomationElement parent)
        {
            List<AutomationElement> children = new List<AutomationElement>();
            AutomationElement sib = TreeWalker.ControlViewWalker.GetLastChild(parent);
            children.Add(sib);
            do
            {
                sib = TreeWalker.ControlViewWalker.GetPreviousSibling(sib);
                if (sib != null)
                    children.Add(sib);
                else
                    break;
            }
            while (true);

            return children;
        }
        
        public static AutomationElement getFirstDescendant(AutomationElement parent, string name)
        {
            return parent.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name));
        }


        public static void InvokeClick(AutomationElement element)
        {
            InvokePattern pattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            pattern.Invoke();
        }

        public void Dispose()
        {
            _application.Dispose();
        }
    }
}
