using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuncTreeFor1C.classes
{
    /// <summary>
    /// Осуществляет парсинг текста файла
    /// </summary>
    public class Parser
    {
        const string strProc = "процедура";
        const string strFunc = "функция";
        const string strExport = "экспорт";

        const string extensionBSL = ".bsl";

        /// <summary>
        /// Для каждого файла из массива files вызывает парсинг
        /// </summary>
        /// <param name="files"></param>
        /// <param name="cutPathLength"></param>
        /// <returns></returns>
        public static ListFunction ParseFiles(FileInfo[] files, UpdateStatus updateStatus, int cutPathLength = 0)
        {
            var result = new ListFunction();
            var countAll = files.Count();
            var count = 0;
            object locker = new Object();

            Parallel.ForEach(files, (file) =>
            {
                lock (locker)
                {
                    count++;
                    updateStatus((byte)((float)count / countAll * 100));
                }

                if (file.Extension == extensionBSL)
                {
                    var text = File.ReadAllLines(file.FullName);
                    var newListFunction = ParseStrings(text);
                    if (newListFunction.Count() > 0)
                    {
                        foreach (var item in newListFunction)
                        {
                            item.FileName = file.FullName.Substring(cutPathLength);
                            lock (result)
                            {
                                result.Add(item);
                            }
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Парсит переданный текст на наличие функций и процедур
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ListFunction ParseStrings(string[] text)
        {
            var result = new ListFunction();
            var countLines = text.Count();
            var index = 0;
            while (index < countLines)
            {
                var str = text[index].Trim();
                var strLow = str.ToLower();
                var indexStartName = 0;
                TypeFunction typeFunc = TypeFunction.function;

                // Определяем процедура или функция.
                if (strLow.IndexOf(strProc) == 0)
                {
                    indexStartName = strProc.Length;
                    typeFunc = TypeFunction.procedure;
                }
                else if (strLow.IndexOf(strFunc) == 0)
                {
                    indexStartName = strFunc.Length;                 
                }

                if (indexStartName > 0)
                {
                    var firstBracket = strLow.IndexOf('(');
                    if (firstBracket == -1)
                    {
                        firstBracket = strLow.IndexOf(';');
                        if (firstBracket == -1)
                        {
                            firstBracket = strLow.Length;
                        }
                    }
                    var export = strLow.IndexOf(strExport) >= 0;
                    var secondBracket = strLow.IndexOf(')');

                    var newFunction = new FunctionInfo();
                    newFunction.Name = str.Substring(indexStartName, firstBracket - indexStartName).Trim();
                    newFunction.Descript = GetDescriptFunction(text, index);
                    newFunction.Type = typeFunc;
                    newFunction.IndexStart = indexStartName;
                    result.Add(newFunction);
                }
                index++;
            }
            return result;
        }

        /// <summary>
        /// Получим описание функции, это все комментарии которые находятся выше объявления функции
        /// </summary>
        /// <param name="text"></param>
        /// <param name="indexEnd"></param>
        /// <returns></returns>
        private static List<string> GetDescriptFunction(string[] text, int indexEnd)
        {
            var indStart = indexEnd;
            var descriptEmpty = true;
            while (indStart > 0)
            {
                indStart--;
                var strDesc = text[indStart].Trim();
                if ((strDesc.Length > 2 && strDesc.Substring(0, 2) != "//")
                    || strDesc.Length == 0)
                {
                    indStart++;
                    break;
                }
                if (strDesc.Length > 2
                    && strDesc.Substring(0, 2) == "//"
                    && strDesc.Substring(2).Trim() != "")
                {
                    descriptEmpty = false;
                }
            }
            if (!descriptEmpty)
            {
                var result = new List<string>();
                for (int i = indStart; i < indexEnd; i++)
                {
                    var str = text[i].Trim();
                    if (str.Length > 2)
                    {
                        str = str.Substring(2);
                        if (str.Length > 0)
                        {
                            result.Add(str);
                        }
                    }
                }
                return result;
            }
            return null;
        }
    }
}
