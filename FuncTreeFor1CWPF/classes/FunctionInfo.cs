using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FuncTreeFor1CWPF.classes
{
    /// <summary>
    /// Тип функция/процедура
    /// </summary>
    public enum TypeFunction
    {
        function,
        procedure
    }

    /// <summary>
    /// Данные о функции, ее описание и параметры
    /// </summary>
    public class FunctionInfo
    {

        public FunctionInfo(FunctionList functionList)
        {
            FunctionList = functionList;
        }

        /// <summary>
        /// Имя функции
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер строки с которой начинается описание функции
        /// </summary>
        public int IndexStartDescript { get; set; }

        /// <summary>
        /// Экспортная функция
        /// </summary>
        public bool Export { get; set; }

        /// <summary>
        /// Тип функции
        /// </summary>
        public TypeFunction Type { get; set; }

        /// <summary>
        /// Номер строки с которой начинается функция в файле
        /// </summary>
        public int IndexStart { get; set; }

        /// <summary>
        /// Ссылка на список в котором находится данный элемент
        /// </summary>
        public FunctionList FunctionList { get; }

        /// <summary>
        /// Получает описание функции из файла
        /// </summary>
        public List<string> Descript
        {
            get
            {
                var fileModule = FunctionList.FileModule;
                IEnumerable<string> file = File.ReadLines(fileModule.FullName).Skip(IndexStartDescript);

                var descript = file.Take(IndexStart - IndexStartDescript).ToArray();
                var result = descript
                    .Select(str =>
                    {
                        if (str.Length > 2)
                            return str.Substring(2).Trim();
                        else
                            return "";
                    })
                    .ToList();
                return result;
            }
        }

        /// <summary>
        /// Получает тело функции из файла
        /// </summary>
        public List<string> Body
        {
            get
            {
                var fileModule = FunctionList.FileModule;
                IEnumerable<string> file = File.ReadLines(fileModule.FullName).Skip(IndexStart);

                var result = new List<string>(20);

                foreach (var str in file)
                {
                    result.Add(str);
                    var tmpStr = str.ToLower();
                    if ((tmpStr.Length >= 12 && tmpStr.Substring(0, 12) == "конецфункции")
                        || (tmpStr.Length >= 14 && tmpStr.Substring(0, 14) == "конецпроцедуры"))
                    {
                        break;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Области в которых находится функция
        /// </summary>
        string[] _area;
        public string[] Area
        {
            get
            {
                if (_area == null)
                {
                    _area = new string[0];
                }
                return _area;
            }
            set { _area = value; }
        }
    }
}
