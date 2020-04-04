using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncTreeFor1CWPF.FileTypes;

namespace FuncTreeFor1CWPF.classes
{
    /// <summary>
    /// Список функций
    /// </summary>
    public class FunctionList : List<FunctionInfo>
    {
        public FileModule FileModule { get; }
        public FunctionList(FileModule fileModule)
        {
            FileModule = fileModule;
        }
        public FunctionList()
        {
        }
    }
}
