using System;
using System.Collections.Generic;
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

        ///// <summary>
        ///// Номер строки окончания функции
        ///// </summary>
        //public int IndexEnd { get; set; }

        /// <summary>
        /// Ссылка на список в котором находится данный элемент
        /// </summary>
        public FunctionList FunctionList { get; }

    }
}
