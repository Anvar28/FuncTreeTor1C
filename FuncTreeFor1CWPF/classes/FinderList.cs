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

        public void AddFromFileTypes(FileType fileType, int countCutFullPath)
        {

            var mParents = fileType.FullName.Substring(countCutFullPath).Split(separetor);

            if (fileType is FileModule)
            {
                var fileModule = (FileModule)fileType;
                foreach (var item in fileModule.FunctionList)
                {
                    var newFinderItem = Add(item.Name, item);
                    newFinderItem.Parents.AddRange(mParents);
                    newFinderItem.Export = item.Export;
                }
            }
            else
            {
                var newFinderItem = Add(fileType.Name, fileType);
                var c = mParents.Length - 1;
                for (int i = 0; i < c; i++)
                {
                    newFinderItem.Parents.Add(mParents[i]);
                }
            }
        }

        /// <summary>
        /// Добавление элемента в список
        /// </summary>
        public FinderItem Add(string Name, object obj)
        {
            var newItem = new FinderItem(Name, obj);
            this.Add(newItem);
            return newItem;
        }
    }
}
