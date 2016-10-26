using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CustomGraph
{
    public class NodeList<T> : Collection<INode<T>>
    {
        public NodeList() : base() { }       
        public INode<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (INode<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
        public IList<INode<T>> Nodes { get { return base.Items; } }
        public new void Add(INode<T> node)
        { Items.Add(node); }
        public void AddRange(NodeList<T> nodeList)
        {
            foreach (INode<T> node in nodeList)
            {
                Items.Add(node);
            }
        }
    }
}
