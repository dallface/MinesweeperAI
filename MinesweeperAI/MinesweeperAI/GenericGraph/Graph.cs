using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomGraph
{
    public class Graph<T>
    {
        public NodeList<T> NodeSet { get; private set; }
        public int Count { get { return NodeSet.Count; } }
        public Graph() { NodeSet = new NodeList<T>(); }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                NodeSet = new NodeList<T>();
            else
                NodeSet = nodeSet;
        }
        public void AddNode(INode<T> node) { NodeSet.Add(node); }
        public void AddEdge(T from, T to)
        {
            INode<T> n1 = GetNode(from);
            INode<T> n2 = GetNode(to);
            if(n1 != null && n2 != null) AddEdge(n1,n2);
        }
        public void AddEdge(INode<T> from, INode<T> to)
        {
            if(!from.Neighbors.Contains(to.Value)) { from.Neighbors.Add(to.Value); }
            if (!to.Neighbors.Contains(from.Value)) { to.Neighbors.Add(from.Value); }
        }
        public INode<T> GetNode(T data) { return NodeSet.FindByValue(data); }
        public bool Contains(INode<T> node) { return NodeSet.Contains(node); }
        public bool Contains(T value) { return NodeSet.FindByValue(value) != null; }
        public bool Remove(INode<T> node)
        {
            // first remove the node from the nodeset
            NodeSet.Remove(node);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (INode<T> n in NodeSet)
            {
                int index = node.Neighbors.IndexOf(node.Value);
                if (index != -1)
                {
                    // remove the reference to the node
                    n.Neighbors.RemoveAt(index);
                }
            }
            return true;
        }
    }
}
