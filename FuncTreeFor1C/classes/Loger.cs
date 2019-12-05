using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuncTreeFor1C.classes
{
    public interface ILoger
    {
        void Write(string str);
    }

    // Логер в файл
    class LogerFile : ILoger
    {
        private string path;

        public LogerFile(string lPath)
        {
            path = lPath;
        }

        public void Write(string str)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now.ToString() + " " + str);
            }
        }
    }
}
