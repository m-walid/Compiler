using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Node
    {
        private string data { get; set; }
        private Node parent {get; set; }
        private List<Node> children { get; set; } = new List<Node>();

        public Node(String data, Node parent = null, List<Node> children = null)
        {
            this.data = data;
            this.parent = parent;
            if (children != null) this.children = children;
        }

        public void addChild(Node child)
        {
            this.children.Add(child);
        }
        public void addChildren(List<Node> children)
        {
            this.children.AddRange(children);
        }

    }
}
