using Microsoft.VisualStudio.TestTools.UnitTesting;
using FuncTreeFor1C;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuncTreeFor1C.classes;

namespace FuncTreeFor1C.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        private ListFunction NewParse(string str, string separator = "\r\n")
        {
            return Parser.ParseStrings(
                str.Split(
                    new string[] { separator },
                    StringSplitOptions.RemoveEmptyEntries
                )
            );
        }

        [TestMethod()]
        public void ParseStringsTest1()
        {
            var resut = NewParse(" функккция    ");
            Assert.IsTrue(resut.Count() == 0);
        }
        [TestMethod()]
        public void ParseStringsTest2()
        {
            var resut = NewParse(" функция тест1   ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест1", resut[0].Name);
        }
        [TestMethod()]
        public void ParseStringsTest3()
        {
            var resut = NewParse(" функция тест2 (   ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест2", resut[0].Name);
        }
        [TestMethod()]
        public void ParseStringsTest4()
        {
            var resut = NewParse(" функция тест3 ()   ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест3", resut[0].Name);
        }
        [TestMethod()]
        public void ParseStringsTest5()
        {
            var resut = NewParse(@"
                // описание1 
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест3", resut[0].Name);
            Assert.IsTrue(resut[0].Descript.Count() == 1);
            Assert.IsTrue(resut[0].Descript[0] == " описание1");
        }
        [TestMethod()]
        public void ParseStringsTest6()
        {
            var resut = NewParse(@"
                
                // описание1 
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест3", resut[0].Name);
            Assert.IsTrue(resut[0].Descript.Count() == 1);
            Assert.IsTrue(resut[0].Descript[0] == " описание1");
        }
        [TestMethod()]
        public void ParseStringsTest7()
        {
            var resut = NewParse(@"
                
                // описание1 
                // описание2 
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест3", resut[0].Name);
            Assert.IsTrue(resut[0].Descript.Count() == 2);
            Assert.IsTrue(resut[0].Descript[0] == " описание1");
            Assert.IsTrue(resut[0].Descript[1] == " описание2");
        }
        [TestMethod()]
        public void ParseStringsTest8()
        {
            var resut = NewParse(@"
                левый текст
                // описание1 
                // описание2 
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 1);
            Assert.AreEqual("тест3", resut[0].Name);
            Assert.IsTrue(resut[0].Descript.Count() == 2);
            Assert.IsTrue(resut[0].Descript[0] == " описание1");
            Assert.IsTrue(resut[0].Descript[1] == " описание2");
        }
        [TestMethod()]
        public void ParseStringsTest9()
        {
            var resut = NewParse(@"
                   
                Функция тест
                // описание1  функция 11
                // описание1 
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 2);
            Assert.AreEqual("тест", resut[0].Name);
            Assert.AreEqual(null, resut[0].Descript);
            Assert.AreEqual("тест3", resut[1].Name);
            Assert.IsTrue(resut[1].Descript.Count() == 2);
            Assert.IsTrue(resut[1].Descript[0] == " описание1  функция 11");
        }
        [TestMethod()]
        public void ParseStringsTest10()
        {
            var resut = NewParse(@"

                // Тут тоже описание
                //
                // Тут еще описаение                

                // Описание тест 1
                процедура тест 1
                // описание1  функция 11

                // описание1
                функция тест3 ()   
            ");
            Assert.IsTrue(resut.Count() == 2);
            Assert.AreEqual("тест 1", resut[0].Name);
            Assert.IsTrue(resut[0].Descript.Count() == 3);
            Assert.IsTrue(resut[0].Descript[0] == " Тут тоже описание");

            Assert.AreEqual("тест3", resut[1].Name);
            Assert.IsTrue(resut[1].Descript.Count() == 2);
            Assert.IsTrue(resut[1].Descript[0] == " описание1  функция 11");
            Assert.IsTrue(resut[1].Descript[1] == " описание1");
        }
    }
}