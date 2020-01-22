using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuncTreeFor1C.classes
{
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
        public string Name { get; set; }
        public string FileName { get; set; }
        public List<string> Descript { get; set; }
        public List<string> Text { get; set; }
        public bool Export { get; set; }
        public TypeFunction Type { get; set; }

        /// <summary>
        /// Строка с которой начинается функция в файле
        /// </summary>
        public int IndexStart { get; set; }
    }
}
