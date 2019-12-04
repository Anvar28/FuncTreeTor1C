using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuncTreeFor1C.classes
{
    /// <summary>
    /// Осуществяет поиск файлов
    /// </summary>
    public class Searcher
    {
        const string filterExt = "*.bsl";

        public static FileInfo[] Search(string path)
        {
            var fileList = new List<FileInfo>();
            FileSearchFunction(path, fileList);
            return fileList.ToArray();
        }

        /// <summary>
        /// Поиск файлов в выбранной папке (рекурсивно) и заполнение списка файлов с расширением bsl
        /// </summary>
        /// <param name="Dir"></param>
        private static void FileSearchFunction(string Dir, List<FileInfo> fileList)
        {
            DirectoryInfo DI = new DirectoryInfo(Dir);
            fileList.AddRange(DI.GetFiles(filterExt, SearchOption.AllDirectories));
        }
    }
}
