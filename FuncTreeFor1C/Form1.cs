using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FuncTreeFor1C
{

    /// <summary>
    /// Данные о функции, ее описание и параметры
    /// </summary>
    public class FunctionInfo
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public List<string> Descript { get; set; }
    }

    /// <summary>
    /// Список функций
    /// </summary>
    public class ListFunction
    {
        public List<FunctionInfo> List { get; set; } = new List<FunctionInfo>();

        public FunctionInfo Add(FunctionInfo elem)
        {
            List.Add(elem);
            return elem;
        }

        public int Count()
        {
            return List.Count();
        }

        public IEnumerator<FunctionInfo> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public FunctionInfo this[int index]
        {
            get
            {
                return List[index];
            }
            set
            {
                List[index] = value;
            }
        }
    }

    /// <summary>
    /// Осуществляет парсинг текста файла
    /// </summary>
    public class Parser
    {
        const string strProc = "процедура";
        const string strFunc = "функция";

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
                if (strLow.IndexOf(strProc) == 0 || strLow.IndexOf(strFunc) == 0)
                {
                    var firstBreak = strLow.IndexOf(' ') + 1;
                    var firstBracket = strLow.IndexOf('(');
                    if (firstBracket == -1)
                    {
                        firstBracket = strLow.IndexOf(';');
                        if (firstBracket == -1)
                        {
                            firstBracket = strLow.Length;
                        }
                    }

                    var secondBracket = strLow.IndexOf(')');
                    if (firstBreak > 0)
                    {
                        var newFunction = new FunctionInfo();
                        newFunction.Name = str.Substring(firstBreak, firstBracket - firstBreak).Trim();
                        newFunction.Descript = GetDescriptFunction(text, index);
                        result.Add(newFunction);
                    }
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
                if (strDesc.Length > 2
                    && strDesc.Substring(0, 2) == "//"
                    && strDesc.Substring(2).Trim() != "")
                {
                    descriptEmpty = false;
                }
                else
                {
                    indStart++;
                    descriptEmpty = false;
                    break;
                }
            }
            if (!descriptEmpty)
            {
                var result = new List<string>();
                for (int i = indStart; i < indexEnd; i++)
                {
                    var str = text[i].Trim().Substring(2);
                    if (str.Length > 0)
                    {
                        result.Add(str);
                    }
                }
                return result;
            }
            return null;
        }
    }

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
            fileList.AddRange(DI.GetFiles(filterExt));

            DirectoryInfo[] SubDir = DI.GetDirectories();
            foreach (var item in SubDir)
            {
                FileSearchFunction(item.FullName, fileList);
            }
        }
    }

    public partial class Form1 : Form
    {
        ListFunction listFunction = new ListFunction();

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = FBD.SelectedPath;
                var files = Searcher.Search(tbPath.Text);
                foreach (var file in files)
                {
                    var text = File.ReadAllLines(file.FullName);
                    var newListFunction = Parser.ParseStrings(text);
                    if (newListFunction.Count() > 0)
                    {
                        foreach (var item in newListFunction)
                        {
                            item.FileName = file.Name;
                            listFunction.Add(item);
                        }
                    }
                }
            }
        }
    }
}
