using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1C.classes
{

    public class TreeItemModel
    {
        #region Properties

        public string Name { get; set; }

        private ObservableCollection<TreeItemModel> children;

        public object Obj { get; set; }

        public ObservableCollection<TreeItemModel> Children
        {
            get
            {
                if (children == null) children = new ObservableCollection<TreeItemModel>();
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

    public class TreeItemModelMethod : TreeItemModel
    {
        #region Properties

        public bool Export { get; set; }

        public TypeFunction Type { get; set; }

        public TreeItemModelMethod(string name, object obj, TypeFunction type, bool export = false)
            : base(name, obj)
        {
            Export = export;
            Type = type;
        }

        #endregion
    }
}
