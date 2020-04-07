using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.TreeNodes
{

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
