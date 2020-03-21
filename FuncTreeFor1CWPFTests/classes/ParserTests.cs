using Microsoft.VisualStudio.TestTools.UnitTesting;
using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes.Tests
{
    [TestClass()]
    public class ParserTests
    {
        private FunctionList NewParse(string str, string separator = "\r\n")
        {
            return Parser.Parse(
                str.Split(
                    new string[] { separator },
                    StringSplitOptions.RemoveEmptyEntries
                ),
                null
            );
        }

        [TestMethod()]
        public void ParseTest()
        {
            var resut = NewParse("");
            Assert.IsTrue(resut.Count() == 0);
        }

        [TestMethod()]
        public void ParseTest_func()
        {
            var resut = NewParse("функция тест()");
            Assert.IsTrue(resut.Count() == 1);
            Assert.IsTrue(resut[0].IndexStart == 0);
            Assert.IsTrue(resut[0].Export == false);
            Assert.IsTrue(resut[0].IndexStartDescript == 0);
            Assert.IsTrue(resut[0].Name == "тест");
            Assert.IsTrue(resut[0].Type == TypeFunction.function);
        }

        [TestMethod()]
        public void ParseTest_proc()
        {
            var resut = NewParse("процедура тест()");
            Assert.IsTrue(resut.Count() == 1);
            Assert.IsTrue(resut[0].IndexStart == 0);
            Assert.IsTrue(resut[0].Export == false);
            Assert.IsTrue(resut[0].IndexStartDescript == 0);
            Assert.IsTrue(resut[0].Name == "тест");
            Assert.IsTrue(resut[0].Type == TypeFunction.procedure);
        }

        [TestMethod()]
        public void ParseTest_proc_export()
        {
            var resut = NewParse("процедура тест() экспорт");
            Assert.IsTrue(resut.Count() == 1);
            Assert.IsTrue(resut[0].IndexStart == 0);
            Assert.IsTrue(resut[0].Export == true);
            Assert.IsTrue(resut[0].IndexStartDescript == 0);
            Assert.IsTrue(resut[0].Name == "тест");
            Assert.IsTrue(resut[0].Type == TypeFunction.procedure);
        }

        [TestMethod()]
        public void ParseTest_proc_export_comment()
        {
            var resut = NewParse(@"// Описание
                процедура тест() экспорт");
            Assert.IsTrue(resut.Count() == 1);
            Assert.IsTrue(resut[0].IndexStart == 1);
            Assert.IsTrue(resut[0].Export == true);
            Assert.IsTrue(resut[0].IndexStartDescript == 0);
            Assert.IsTrue(resut[0].Name == "тест");
            Assert.IsTrue(resut[0].Type == TypeFunction.procedure);
        }

        [TestMethod()]
        public void ParseTest_2_proc()
        {
            var resut = NewParse(@"// Описание
                процедура тест() экспорт
                
                // процедура
                // функция
                //
                процедура тест2() экспорт");
            Assert.IsTrue(resut.Count() == 2);
            Assert.IsTrue(resut[0].IndexStart == 1);
            Assert.IsTrue(resut[0].Export == true);
            Assert.IsTrue(resut[0].IndexStartDescript == 0);
            Assert.IsTrue(resut[0].Name == "тест");
            Assert.IsTrue(resut[0].Type == TypeFunction.procedure);

            Assert.IsTrue(resut[1].IndexStart == 6);
            Assert.IsTrue(resut[1].Export == true);
            Assert.IsTrue(resut[1].IndexStartDescript == 3);
            Assert.IsTrue(resut[1].Name == "тест2");
            Assert.IsTrue(resut[1].Type == TypeFunction.procedure);
        }
    }
}