using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1C.classes
{
    public class FinderList
    {
        public List<FinderItem> List { get; set; } = new List<FinderItem>();

        public FinderList()
        {
        }

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        public FinderItem Add(string Name, object obj)
        {
            var newItem = new FinderItem(Name, obj);
            List.Add(newItem);
            return newItem;
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
        public IEnumerator<FinderItem> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        /// <summary>
        /// Реализация обращения по индексу через скобки
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public FinderItem this[int index]
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
