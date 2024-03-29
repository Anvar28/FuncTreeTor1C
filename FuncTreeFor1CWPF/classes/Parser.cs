﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FuncTreeFor1CWPF.FileTypes;

namespace FuncTreeFor1CWPF.classes
{
    /// <summary>
    /// Осуществляет парсинг текста
    /// </summary>
    public class Parser
    {
        const string strProc = "процедура";
        const string strFunc = "функция";
        const string strExport = "экспорт";
        const string strAreaBegin = "#область";
        const string strAreaEnd = "#конецобласти";

        /// <summary>
        /// Парсит переданный текст на наличие функций и процедур
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static FunctionList Parse(string[] text, FileModule fileModule)
        {
            var result = new FunctionList(fileModule);
            var countLines = text.Count();
            var index = 0;
            var areas = new Stack<string>();

            while (index < countLines)
            {
                var str = text[index].Trim();
                var strLow = str.ToLower();
                var indexStartName = 0;
                TypeFunction typeFunc = TypeFunction.function;

                // Если начало области, то поместим в стек ее название
                if (strLow.StartsWith(strAreaBegin))
                {
                    areas.Push(str.Substring(strAreaBegin.Length + 1));
                }

                // Если конец области то уберем имя последней области из стека
                if (strLow.StartsWith(strAreaEnd))
                {
                    if (areas.Count > 0)
                        areas.Pop();
                }

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

                    var newFunction = new FunctionInfo(result);
                    newFunction.Name = str.Substring(indexStartName, firstBracket - indexStartName).Trim();
                    newFunction.IndexStartDescript = GetStartDescriptFunction(text, index);
                    newFunction.Type = typeFunc;
                    newFunction.IndexStart = index;
                    newFunction.Export = export;
                    if (areas.Count > 0)
                    {
                        newFunction.Area = areas.ToArray().Reverse().ToArray();
                    }                        
                    result.Add(newFunction);
                }
                index++;
            }
            result.Sort((x, y) => string.Compare(x.Name, y.Name));
            return result;
        }

        /// <summary>
        /// Возвращает номер строки начала коментария который описывает функцию, 
        /// это все комментарии которые находятся выше объявления функции
        /// </summary>
        /// <param name="text"></param>
        /// <param name="indexEnd"></param>
        /// <returns></returns>
        private static int GetStartDescriptFunction(string[] text, int indexEnd)
        {
            var indStart = indexEnd;
            while (indStart > 0)
            {
                indStart--;
                var strDesc = text[indStart].Trim();
                if (!ItsComment(strDesc))
                {
                    return ++indStart;
                }
            }
            return indStart;
        }

        private static bool ItsComment(string str)
        {
            var tmpStr = str.Trim();
            if ((tmpStr.Length >= 2) && tmpStr.Substring(0, 2) == "//")
            {
                return true;
            }
            return false;
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
