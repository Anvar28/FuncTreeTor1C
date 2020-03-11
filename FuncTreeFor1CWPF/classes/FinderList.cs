using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class FinderList
    {
        public List<FinderItem> List { get; set; } = new List<FinderItem>();

        public FinderList()
        {

        }

        public FinderList(FileTypesList fileTypesList, int countCutFullPath, UpdateStatus UpdateStatusPercentAsync)
        {
            var separetor = new char[] { '\\' };
            var countAll = fileTypesList.Count();
            var count = 0;

            foreach (var file in fileTypesList)
            {
                var mParents = file.FullName.Substring(countCutFullPath).Split(separetor);

                if (file is FileModule)
                {
                    var fileModule = (FileModule)file;
                    foreach (var item in fileModule.FunctionList)
                    {
                        var newFinderItem = Add(item.Name, item);
                        newFinderItem.Parents.AddRange(mParents);
                        newFinderItem.Export = item.Export;
                    }
                }
                else
                {
                    var newFinderItem = Add(file.Name, file);
                    var c = mParents.Length - 1;
                    for (int i = 0; i < c; i++)
                    {
                        newFinderItem.Parents.Add(mParents[i]);
                    }
                }
                count++;
                var percent = (byte)((float)count / countAll * 100);
                UpdateStatusPercentAsync(percent);
            }
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
