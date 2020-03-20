using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public abstract class ViewBase
    {
    }

    public class ViewOtherFile : ViewBase
    {
        public string[] Text { get; set; }

        public ViewOtherFile(string fullPath)
        {
            Text = File.ReadAllLines(fullPath);
        }
    }
}
