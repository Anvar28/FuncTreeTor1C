using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuncTreeFor1CWPF.classes
{
    /// <summary>
    /// Осуществяет поиск файлов
    /// </summary>
    public class FileSearcher
    {

        /// <summary>
        /// Поиск файлов в выбранной папке (рекурсивно) и заполнение списка файлов 
        /// </summary>
        /// <param name="path"></param>
        public static void Search(FileQueue fileQueue, string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            foreach (var file in files)
            {
                fileQueue.Enqueue(new FileInfo(file));
            }

            foreach (var dir in directories)
            {
                Search(fileQueue, dir);
            }
        }

    }
}
