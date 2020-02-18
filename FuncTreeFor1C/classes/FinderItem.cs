using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1C.classes
{
    public class FinderItem
    {
        public string Name { get; set; }
        public object Object { get; set; }
        public bool IsExport { get; set; }
        public List<string> Parents { get; }

        public FinderItem(string name, object obj)
        {
            Name = name;
            Object = obj;
            Parents = new List<string>(8);
        }
    }
}
