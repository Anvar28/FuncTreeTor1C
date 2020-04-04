using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuncTreeFor1CWPF.FileTypes;

namespace FuncTreeFor1CWPF.classes
{
    public class FileTypesQueue : ConcurrentQueue<FileType>
    {
    }
}
