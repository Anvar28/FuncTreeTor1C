using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTreeFor1C.classes
{
    /// <summary>
    /// Список функций
    /// </summary>
    public class FunctionList
    {
        public List<FunctionInfo> List { get; set; } = new List<FunctionInfo>();
        public FileModule FileModule { get; }

        public FunctionList(FileModule fileModule = null)
        {
            FileModule = FileModule;
        }

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public FunctionInfo Add(FunctionInfo elem)
        {
            List.Add(elem);
            return elem;
        }

        /// <summary>
        /// Добавление в список дргого списка
        /// </summary>
        /// <param name="addList"></param>
        public void Add(FunctionList addList)
        {
            List.AddRange(addList.List);
        }

        /// <summary>
        /// Количество
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return List.Count();
        }

        /// <summary>
        /// реализация интерфейса IEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<FunctionInfo> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Реализация обращения по индексу через скобки
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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
