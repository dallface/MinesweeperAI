using CustomGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.UIItems;

namespace MinesweeperAI
{
    public class Tile : INode<Tile>
    {
        public string Name { get { return Item.Name; } }
        public List<Tile> Neighbors { get; set; }
        public Tile Value
        {
            get { return this; }
            set {  }
        }
        public int? MineCount { get; private set; }
        public bool isConcealed { get; private set; }
        public Point Location { get; private set; }
        public double? BombProbability { get; private set; }
        public bool Flagged
        {
            get
            {
                if (BombProbability == 100)
                    return true;
                else
                    return false;
            }
        }

        private IUIItem Item;
        private BackgroundWorker Worker;

        public Tile(IUIItem item, BackgroundWorker worker)
        {
            Neighbors = new List<Tile>();
            MineCount = null;
            isConcealed = true;
            BombProbability = null;
            Location = item.Location;
            Item = item;

            Worker = worker;
            Worker.WorkerSupportsCancellation = true;
            Worker.DoWork += Worker_DoWork1;
        }

        private void Worker_DoWork1(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                // Check to update tile info if still concealed
                if (isConcealed)
                {
                    // If it's revealed, update the info and it's dead
                    if (!Item.Name.Contains("Concealed"))
                    {
                        isConcealed = false;
                        MineCount = parseNameForMines();
                        return;
                    }

                    // If this is an active node then check for bomb probability
                    if (isActive())
                    {
                        // If there isn't a bomb probability try to update it
                        if (BombProbability == null)
                        {
                            UpdateProbability();
                        }

                        // If there isn't a chance of a bomb invoke a left click
                        else if (BombProbability == 0)
                        {
                            Click();
                        }

                        // If there is for sure a bomb here then this tile is dead
                        else if (BombProbability == 100)
                        {
                            return;
                        }
                    }
                }
                // If it's not concealed it's dead
                else
                {
                    return;
                }
            }               
        }
        private void UpdateProbability()
        {
            int concealedNeighbors = 0;
            int flaggedNeighbors = 0;

            foreach (Tile n in Neighbors)
            {
                // Determine the number of concealed and flagged direct neighbors
                if (n.isConcealed)
                {
                    if (n.Flagged)
                        flaggedNeighbors++;
                    else
                        concealedNeighbors++;
                }
                else
                {
                    int concealedNeighborNeighbors = 0;
                    int flaggedNeighborNeighbors = 0;

                    // Determine how many concealed and flagged neighbors this neighbor has
                    // (note that this node is included but known to be concealed and not flagged)
                    foreach (Tile nn in n.Neighbors)
                    {
                        if (nn.isConcealed)
                        {
                            if (nn.Flagged)
                                flaggedNeighborNeighbors++;
                            else if(nn.BombProbability != 0)
                                concealedNeighborNeighbors++;
                        }
                    }

                    if (n.MineCount - flaggedNeighborNeighbors == concealedNeighborNeighbors)
                    {
                        BombProbability = 100;
                        break;
                    }
                    else if (n.MineCount == flaggedNeighborNeighbors)
                    {
                        BombProbability = 0;
                        break;
                    }
                }
            }
        }
        private bool isActive()
        {
            if (isConcealed)
            {
                foreach (Tile n in Neighbors)
                {
                    // Active means that at least one neighbor is revealed
                    if (!n.isConcealed) return true;
                }
            }
            else
            {
                isConcealed = false;
            }
            return false;
        }

        private int? parseNameForMines()
        {
            if (!isConcealed)
            {
                string cleanName = RemoveSpecialChars(Name);
                string mines = String.Empty;
                mines = cleanName.Remove(0, cleanName.IndexOf("Cleared") + 7);
                mines = mines.Replace("MinesSurrounding", "");
                if (mines == "No") return 0;
                else return Int32.Parse(mines);
            }
            return null;
        }
        public static string RemoveSpecialChars(string word)
        {
            string[] ToRemove = { " ", ",", ".", "(", ")" };
            string newWord = word;
            foreach (string tr in ToRemove)
            {
                newWord = newWord.Replace(tr, String.Empty).Trim();
            }
            if (newWord == String.Empty) newWord = "CustomClass";
            return newWord;
        }        

        public void Click()
        {
            InvokePattern pattern = Item.AutomationElement.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            pattern.Invoke();
        }
    }
}
