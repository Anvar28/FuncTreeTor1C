using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class FileTypesList
    {
        public List<FileType> List { get; set; } = new List<FileType>();

        public FileType Add(FileType elem)
        {
            List.Add(elem);
            return elem;
        }

        public void AddRange(FileType[] elements)
        {
            List.AddRange(elements);
        }

        public int Count()
        {
            return List.Count();
        }

        public IEnumerator<FileType> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public FileType this[int index]
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
