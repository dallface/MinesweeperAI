using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGraph
{
    public interface INode<T>
    {
        List<T> Neighbors { get; set; }
        T Value { get; set; }
    }
}
