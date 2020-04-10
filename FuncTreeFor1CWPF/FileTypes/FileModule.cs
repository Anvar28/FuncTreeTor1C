using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.FileTypes
{

    /// <summary>
    /// Файл модуля
    /// </summary>
    public class FileModule: FileType
    {
        public FunctionList FunctionList { get; set; }

        public FileModule()
        {
            FunctionList = new FunctionList(this);
        }
    }
}
