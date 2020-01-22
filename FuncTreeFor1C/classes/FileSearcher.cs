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
    public class FileSearcher
    {
        const string filterExt = "*.*";

        /// <summary>
        /// Поиск файлов в выбранной папке (рекурсивно) и заполнение списка файлов с расширением bsl
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static FileInfo[] Search(string path, UpdateStatus updateStatus)
        {
            var fileList = new List<FileInfo>();
            DirectoryInfo DI = new DirectoryInfo(path);
            fileList.AddRange(DI.GetFiles(filterExt, SearchOption.AllDirectories));

            updateStatus(100);

            return fileList.ToArray();
        }
    }
}
