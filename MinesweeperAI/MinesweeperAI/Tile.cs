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
        public List<Tile> Neighbors { get; set; }

        public Tile Value { get { return this; } set { } }

        public string Name { get { return Item.Current.Name; } }

        public Point Location { get; private set; }

        public bool isConcealed { get; private set; }

        public int? MineCount { get; private set; }

        public TileState State { get; private set; }

        public bool? HasMine { get; set; }

        private AutomationElement Item;
        
        public enum TileState
        {
            Active,
            Inactive,
            Dead
        }

        public Tile(AutomationElement item)
        {
            Location = item.Current.BoundingRectangle.TopLeft;
            Neighbors = new List<Tile>();
            MineCount = null;
            isConcealed = true;
            HasMine = null;
            Item = item;
        }

        public void Click()
        {
            InvokePattern pattern = Item.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            pattern.Invoke();
        }

        public bool SingleInfer()
        {
            if(!isConcealed)
            {
                List<Tile> concealed = new List<Tile>();
                int flagged = 0;
                foreach(Tile t in Neighbors)
                {
                    if (t.HasMine == true)
                        flagged++;
                    else if (t.isConcealed)
                        concealed.Add(t);
                }

                // If the current revealed tile has a mine count that's equal to the number of flagged and concealed neighbors
                // then the concealed neighbors are all mines
                if (MineCount - flagged == concealed.Count)
                {
                    foreach (Tile t in concealed) t.HasMine = true;
                    return true;
                }

                // If all of the mines have been found then the remaining concealed neighbors cannot be mines
                else if (MineCount == flagged)
                {
                    foreach (Tile t in concealed) { t.HasMine = false; t.Click(); }
                    return true;
                }
            }
            return false;
        }


        public void Update()
        {
            UpdateConcealed();
            foreach (Tile t in Neighbors) t.UpdateConcealed();
            UpdateState();
        }
        private void UpdateConcealed()
        {
            if (isConcealed)
            {
                if (!Name.Contains("Concealed"))
                {
                    isConcealed = false;
                    MineCount = parseNameForMines();
                }
            }
        }
        private void UpdateState()
        {
            // INACTIVE
            // - Concealed tiles
            // ACTIVE
            // - Revealed tiles that have at least one concealed unknown tile
            // DEAD
            // - Revealed tiles that are surrounded by revealed tiles or known mines
            if (isConcealed)
                State = TileState.Inactive;
            else
            {
                foreach (Tile t in Neighbors)
                {
                    if (t.isConcealed)
                    {
                        State = TileState.Active;
                        return;
                    }
                }
                State = TileState.Dead;
            }
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
        private string RemoveSpecialChars(string word)
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




        //private BackgroundWorker Worker;

        //private void Worker_DoWork1(object sender, DoWorkEventArgs e)
        //{
        //    while (true)
        //    {
        //        // Check to update tile info if still concealed
        //        if (isConcealed)
        //        {
        //            // If it's revealed, update the info and it's dead
        //            if (!Item.Name.Contains("Concealed"))
        //            {
        //                isConcealed = false;
        //                MineCount = parseNameForMines();
        //                return;
        //            }

        //            // If this is an active node then check for bomb probability
        //            if (isActive())
        //            {
        //                // If there isn't a bomb probability try to update it
        //                if (HasMine == null)
        //                {
        //                    UpdateProbability();
        //                }

        //                // If there isn't a chance of a bomb invoke a left click
        //                else if (HasMine == 0)
        //                {
        //                    Click();
        //                }

        //                // If there is for sure a bomb here then this tile is dead
        //                else if (HasMine == 100)
        //                {
        //                    return;
        //                }
        //            }
        //        }
        //        // If it's not concealed it's dead
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        //private void UpdateProbability()
        //{
        //    int concealedNeighbors = 0;
        //    int flaggedNeighbors = 0;

        //    foreach (Tile n in Neighbors)
        //    {
        //        // Determine the number of concealed and flagged direct neighbors
        //        if (n.isConcealed)
        //        {
        //            if (n.HasMine)
        //                flaggedNeighbors++;
        //            else
        //                concealedNeighbors++;
        //        }
        //        else
        //        {
        //            int concealedNeighborNeighbors = 0;
        //            int flaggedNeighborNeighbors = 0;

        //            // Determine how many concealed and flagged neighbors this neighbor has
        //            // (note that this node is included but known to be concealed and not flagged)
        //            foreach (Tile nn in n.Neighbors)
        //            {
        //                if (nn.isConcealed)
        //                {
        //                    if (nn.HasMine)
        //                        flaggedNeighborNeighbors++;
        //                    else if (nn.HasMine != 0)
        //                        concealedNeighborNeighbors++;
        //                }
        //            }

        //            if (n.MineCount - flaggedNeighborNeighbors == concealedNeighborNeighbors)
        //            {
        //                HasMine = 100;
        //                break;
        //            }
        //            else if (n.MineCount == flaggedNeighborNeighbors)
        //            {
        //                HasMine = 0;
        //                break;
        //            }
        //        }
        //    }
        //}

    }
}
