using System.Collections.Generic;
using System.Threading;
using System.Windows.Automation;

namespace MinesweeperFramework
{
    public partial class MinesweeperWindow
    {
        public Grid TileGrid { get; set; }

        public class Grid
        {
            public List<Row> Rows { get; set; }

            public Grid(List<AutomationElement> rowContainers)
            {
                Rows = new List<Row>();
                foreach(AutomationElement r in rowContainers)
                {
                    WaitCallback del = new WaitCallback(AddRow);
                    ThreadPool.QueueUserWorkItem(del, r);
                }
                while(Rows.Count != rowContainers.Count)
                {
                    Thread.Sleep(100);
                }
            }

            public void AddRow(object rowElement)
            {
                Rows.Add(new Row(rowElement as AutomationElement));
            }

            public Row this[int index]
            {
                get
                {
                    return Rows[index];
                }
                set
                {
                    Rows[index] = value;
                }
            }
        }

        public class Row
        {

            public enum GetChildrenMethod
            {
                FindAll,
                Cache,
                TreeWalker
            }
            public List<Tile> Tiles { get; set; }

            public Row(AutomationElement rowContainer)
            {
                Tiles = new List<Tile>();
                List<AutomationElement> children = getChildren(rowContainer);

                foreach (AutomationElement i in children)
                {
                    Tile t = new Tile(i);
                    Tiles.Add(t);
                }
            }

            public Tile this[int index]
            {
                get
                {
                    return Tiles[index];
                }
                set
                {
                    Tiles[index] = value;
                }
            }
        }

        public class Tile
        {
            private AutomationElement element { get; set; }

            public string Name() { return element.Current.Name; }

            public bool isConcealed() { return Name().Contains("Concealed"); }

            public Tile(AutomationElement tile)
            {
                element = tile;
            }
        }
    }
}
