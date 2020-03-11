using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{

    public class TreeItemModel
    {
        #region Properties

        public string Name { get; set; }

        private TreeModel children;

        public object Obj { get; set; }

        public TreeModel Children
        {
            get
            {
                if (children == null)
                {
                    children = new TreeModel();
                }                    
                return children;
            }
        }

        #endregion

        #region Constructor

        public TreeItemModel(string name, object obj)
        {
            Name = name;
            Obj = obj;
        }

        #endregion
    }

    public class TreeItemModel_Method : TreeItemModel
    {
        #region Properties

        public bool Export { get; set; }

        public TypeFunction Type { get; set; }

        public TreeItemModel_Method(string name, object obj, TypeFunction type, bool export = false)
            : base(name, obj)
        {
            Type = type;
            Export = export;
        }

        public TreeItemModel_Method(string name, object obj, bool export = false)
            : base(name, obj)
        {
            Export = export;
        }

        #endregion
    }
}
