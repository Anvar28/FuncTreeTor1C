using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class FinderList : List<FinderItem>
    {
        char[] separetor = new char[] { '\\' };

        public List<FinderItem> AddFromFileTypes(FileType fileType, int countCutFullPath)
        {
            var result = new List<FinderItem>();
            var mParents = fileType.FullName.Substring(countCutFullPath).Split(separetor);

            if (fileType is FileModule)
            {
                var fileModule = (FileModule)fileType;
                foreach (var item in fileModule.FunctionList)
                {
                    var newFinderItem = Add(item.Name, item);
                    newFinderItem.Parents.AddRange(mParents);
                    newFinderItem.Export = item.Export;
                    result.Add(newFinderItem);
                }
            }
            else
            {
                var newFinderItem = Add(fileType.Name, fileType);
                var c = mParents.Length - 1;
                for (int i = 0; i < c; i++)
                {
                    newFinderItem.Parents.Add(mParents[i]);
                    result.Add(newFinderItem);
                }
            }

            return result;
        }

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        public FinderItem Add(string Name, object obj)
        {
            var newItem = new FinderItem(Name, obj);
            lock (this) {
                this.Add(newItem);
            }            
            return newItem;
        }
    }
}
