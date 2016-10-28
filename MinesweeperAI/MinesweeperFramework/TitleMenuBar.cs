using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace MinesweeperFramework
{
    public partial class MinesweeperWindow
    {
        private static InvokePattern _minimizeInvoke;
        public void Minimize() { _minimizeInvoke.Invoke(); }

        private static InvokePattern _maximizeInvoke;
        public void Maximize() { _maximizeInvoke.Invoke(); }

        private static InvokePattern _closeInvoke;
        public void Close() { _closeInvoke.Invoke(); }

        public void GetTitleMenuElements(AutomationElement element)
        {
            AutomationElement tmp = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Minimize"));
            _minimizeInvoke = tmp.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;

            tmp = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Maximize"));
            _maximizeInvoke = tmp.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;

            tmp = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Close"));
            _closeInvoke = tmp.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
        }
    }
}
