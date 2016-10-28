using MinesweeperFramework;
using System;
using System.Diagnostics;

namespace MinesweeperAI
{
    class Program
    {
        public static void Main(string[] args)
        {


            MinesweeperWindow msw;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            msw = MinesweeperWindow.Create();
            Minesweeper.WriteTimeMessage("Total load", sw);
            msw.GameMenu.Options();
            

            Minesweeper minesweeper = new Minesweeper();

            Random r = new Random();
            int maxX = (int)minesweeper.Board.Size.Width;
            int maxY = (int)minesweeper.Board.Size.Height;
            minesweeper.Select(r.Next(1, maxX), r.Next(1, maxY));

            bool gameover = false;
            while (!gameover)
            {
                //Minesweeper.WriteTimeMessage("Sweeping...", sw);
                //minesweeper.Sweep();
                //Minesweeper.WriteTimeMessage("Swept", sw);

                //WriteGrid(minesweeper);
                                
                gameover = minesweeper.CheckDialogPopUp();
            }
            Console.ReadLine();
        }

        public static double getAverage(double[] data)
        {
            double total = 0;
            foreach(double d in data)
            {
                Console.WriteLine(d);
                total += d;
            }
            return total / data.Length;
        }

        public static void WriteGrid(Minesweeper minesweeper)
        {
            int maxX = (int)minesweeper.Board.Size.Width;
            int maxY = (int)minesweeper.Board.Size.Height;
            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    //Can replace with whatever specific Box property you want
                    string d = String.Empty;
                    bool? bp = minesweeper.Board.Rows[y][x].HasMine;
                    if (bp == true) d = "F";
                    else if (bp == null) d = "?";
                    else if (bp == false) d = " ";
                    Console.Write("{" + d + "} ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }        
    }
}
