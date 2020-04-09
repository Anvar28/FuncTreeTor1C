using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.XMLTree
{
    public class XMLTreeNode
    {
        public string Name { get; set; }

        private XMLTreeNodeList _nodes;
        public XMLTreeNodeList Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new XMLTreeNodeList();
                }
                return _nodes;
            }
        }
    }
}
