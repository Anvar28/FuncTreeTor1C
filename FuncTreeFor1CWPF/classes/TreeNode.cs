using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{

    public class TreeNode
    {
        #region Properties

        public string Name { get; set; }

        private TreeNodes _nodes;

        public object Obj { get; set; }

        public TreeNodes Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new TreeNodes();
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

    public class TreeNode_Method : TreeNode
    {
        #region Properties

        public bool Export { get; set; }

        public TypeFunction Type { get; set; }

        public TreeNode_Method(string name, object obj, TypeFunction type, bool export = false)
            : base(name, obj)
        {
            Type = type;
            Export = export;
        }

        public TreeNode_Method(string name, object obj, bool export = false)
            : base(name, obj)
        {
            Export = export;
        }

        #endregion
    }
}
