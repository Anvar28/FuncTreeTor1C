using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTreeFor1C.classes
{
    /// <summary>
    /// Список функций
    /// </summary>
    public class ListFunction
    {
        public List<FunctionInfo> List { get; set; } = new List<FunctionInfo>();

        public FunctionInfo Add(FunctionInfo elem)
        {
            List.Add(elem);
            return elem;
        }

        public int Count()
        {
            return List.Count();
        }

        public IEnumerator<FunctionInfo> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public FunctionInfo this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
            }
        }
    }
}
