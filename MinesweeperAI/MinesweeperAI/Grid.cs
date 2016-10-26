using CustomGraph;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MinesweeperAI
{
    public class Grid
    {
        public Size Size { get { return new Size(Rows[0].Count, Rows.Count); } }
        public List<List<Tile>> Rows = new List<List<Tile>>();
        public int BombsRemaining { get; set; }
        private Graph<Tile> Board = new Graph<Tile>();
        public Grid(List<Tile> gridButtons, int bombCount)
        {
            List<Tile> row = new List<Tile>();
            double? curY = null;
            foreach (Tile tile in gridButtons)
            {
                Board.AddNode(tile);
                if(curY == null) curY = tile.Location.Y;
                if (tile.Location.Y == curY)
                    row.Add(tile);
                else
                {
                    Rows.Add(row);
                    row = new List<Tile>();
                    curY = tile.Location.Y;
                    row.Add(tile);
                }
            }
            Rows.Add(row);
            MapAdj();
        }
        private void MapAdj()
        {
            List<double> dists = getAllDist();
            dists.Sort();
            double low = GetLowest(dists);
            dists.RemoveAll(d => d == low);
            addEdges(low);
            low = GetLowest(dists);
            addEdges(low);
        }
        private void addEdges(double low)
        {
            foreach (Tile curTile in Board.NodeSet)
            {
                foreach(Tile potNeighbor in Board.NodeSet)
                {
                    double dist = DetermineDistance(curTile, potNeighbor);
                    if (dist <= low + 1 && dist >= low-1)
                        Board.AddEdge(curTile, potNeighbor);
                }
            }
        }
        private List<double> getAllDist()
        {
            List<double> dists = new List<double>();
            Tile prevTile = null;
            foreach (Tile b in Board.NodeSet)
            {
                if (prevTile == null)
                    prevTile = b;
                else if (prevTile != b)
                    dists.Add(DetermineDistance(prevTile, b));
            }
            return dists;
        }
        private double GetLowest(IList<double> values)
        {
            double cur = -1;
            foreach (double k in values)
            {
                if (cur == -1) cur = k;
                cur = Math.Min(cur, k);
            }
            return cur;
        }
        private static double DetermineDistance(Tile i1, Tile i2)
        {
            return DetermineDistance(i1.Location, i2.Location);
        }
        private static double DetermineDistance(Point p1, Point p2)
        {
            return Math.Round(Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)),0);
        }
    }
}
