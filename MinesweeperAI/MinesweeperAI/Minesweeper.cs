using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;

namespace MinesweeperAI
{
    public class Minesweeper
    {
        private Application _application;
        private static string _applicationDirectory = @"C:\Program Files\Microsoft Games\Minesweeper";
        private string _applicationPath = Path.Combine(_applicationDirectory, "Minesweeper.exe");

        private Stopwatch sw;

        private List<BackgroundWorker> TileWorkers;
        public Window MainWindow { get; private set; }
        public Grid Board { get; private set; }
        public Minesweeper()
        {
            sw = new Stopwatch();
            sw.Start();
            Initialize();
        }
        private void Initialize()
        {
            getMainWindow();

            List<IUIItem> buttonElements = MainWindow.Items.FindAll(x => x.Name.Contains("Concealed"));
            WriteTimeMessage("All Elements Found", sw);

            TileWorkers = new List<BackgroundWorker>();
            List<Tile> tiles = CreateTiles(buttonElements);
            WriteTimeMessage("Tile List Created", sw);

            Board = new Grid(tiles, 9);
            WriteTimeMessage("Board Created", sw);
        }

        private void getMainWindow()
        {
            ProcessStartInfo startinfo = new ProcessStartInfo(_applicationPath);
            _application = Application.AttachOrLaunch(startinfo);
            MainWindow = _application.GetWindows().Find(x => x.Name == "Minesweeper");
            if (MainWindow == null)
            {
                Window errorDialog = _application.GetWindow("The game is running in software rendering mode");
                errorDialog.Items.Find(x => x.Name == "OK").Click();
                MainWindow = _application.GetWindow("Minesweeper");
            }
            WriteTimeMessage("Main Window Found", sw);
        }

        public static void WriteTimeMessage(string message, Stopwatch sw)
        {
            Console.WriteLine(message + " after {0}s", sw.Elapsed.TotalSeconds);
            sw.Restart();
        }

        private List<Tile> CreateTiles(List<IUIItem> elements)
        {
            List<Tile> tiles = new List<Tile>();            
            foreach (IUIItem element in elements)
            {
                BackgroundWorker bw = new BackgroundWorker();
                TileWorkers.Add(bw);
                tiles.Add(new Tile(element, bw));
            }
            return tiles;
        }

        public enum Difficulty
        {
            Beginner,
            Intermediate,
            Advanced
        }
        public void SetGridSize(Difficulty dif)
        {
            MenuBar mb = MainWindow.GetMenuBar(SearchCriteria.ByText("Application"));
            Menu gameMenu = mb.TopLevelMenu.Find(x => x.Name == "Game");
            gameMenu.Click();
            Menu opt = gameMenu.ChildMenus.Find(x => x.Name == "Options");
            opt.Click();
            Window Options = _application.GetWindow("Options");
            var Selection = Options.Items.Find(x => x.Name.Contains(dif.ToString()));
            Selection.Click();
            IUIItem ok = Options.Items.Find(x => x.Name == "OK");
            ok.Click();

            List<IUIItem> buttonElements = MainWindow.Items.FindAll(x => x.Name.Contains("Concealed"));
            TileWorkers = new List<BackgroundWorker>();
            List<Tile> tiles = CreateTiles(buttonElements);

            Board = new Grid(tiles);
        }
        public void Select(int x, int y)
        {
            Select(new Point(x, y));
        }
        public void Select(Point p)
        {
            Tile tile = Board.Rows[(int)p.Y - 1][(int)p.X - 1];
            tile.Click();
        }

        private List<BackgroundWorker> ActiveWorkers;
        public void Sweep()
        {
            ActiveWorkers = new List<BackgroundWorker>();
            foreach(BackgroundWorker bw in TileWorkers)
            {
                bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                bw.RunWorkerAsync();
                ActiveWorkers.Add(bw);
            }
            while(ActiveWorkers.Count > 0)
            {
                Thread.Sleep(100);
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // If on error or cancelled just display a message
            if(e.Error != null || e.Cancelled == true)
            {
                
            }


        }

        public bool CheckDialogPopUp()
        {
            Window dialog = _application.GetWindows().Find(x => x.Name.Contains("Game"));
            if (dialog != null)
            {
                foreach(IUIItem control in dialog.Items)
                {
                    Console.WriteLine(" {0} ", control.Name);
                }
                return true;
            }
            return false;
        }
        public void Close()
        {
            _application.Close();
        }
    }
}
