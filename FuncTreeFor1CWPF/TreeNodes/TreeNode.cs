using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.TreeNodes
{

    public class TreeNode
    {
        #region Properties

        public string Name { get; set; }

        private TreeNodeList _nodes;

        public object Obj { get; set; }

        public TreeNodeList Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new TreeNodeList();
                }                    
                return _nodes;
            }
        }

        #endregion

        #region Constructor

        public TreeNode(string name, object obj)
        {
            Name = name;
            Obj = obj;
        }

        #endregion
    }

}
