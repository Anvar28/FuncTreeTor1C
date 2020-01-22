using FuncTreeFor1C.classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuncTreeFor1C
{
    public partial class Form1 : Form
    {
        ListFunction listFunction;
        ILoger loger;

        const string strFindFiles = "Поиск файлов...";
        const string strParse = "Парсинг...";
        const string strFillTree = "Заполнение дерева...";

        public Form1()
        {
            loger = new LogerFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log.txt"));
            InitializeComponent();
        }

        private void BtnSelectPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                tbPath.Text = FBD.SelectedPath;
                FindFilesAndParsing(tbPath.Text);
                FillTree();
            }
        }

        public void Log(string text)
        {
            loger.Write(DateTime.Now.ToString("hh:mm:ss") + " " + text);
        }

        public void LogStopwatch(string text, Stopwatch sw)
        {
            sw.Stop();
            Log(text + (sw.ElapsedMilliseconds / 1000).ToString());
        }

        public void UpdateStatusText(string text)
        {
            if (statusBar.Text != text)
            {
                statusBar.Text = text;
            }
        }

        public void UpdateStatusPercent(byte percent)
        {
            if (percent > 100)
                percent = 100;

            if (percent != progressBar.Value)
            {
                progressBar.Value = percent;
            }
        }

        private Stopwatch StopwatchStart()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            return sw;
        }

        private async void FindFilesAndParsing(string path)
        {
            new Task(() =>
            {
                // Поиск файлов

                this.BeginInvoke((MethodInvoker)(() => UpdateStatusText(strFindFiles)));

                var sw = StopwatchStart();

                var files = FileSearcher.Search(
                    path, 
                    (percent) => {
                        this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
                    }
                );

                LogStopwatch("Сканирование файлов: ", sw);

                // Парсинг

                this.BeginInvoke((MethodInvoker)(() => UpdateStatusText(strParse)));

                sw.Start();

                listFunction = Parser.ParseFiles(
                    files, 
                    (percent) => {
                        this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
                    }, 
                    path.Length
                );

                LogStopwatch("Парсинг файлов: ", sw);

                // Заполняем дерево

                this.BeginInvoke((MethodInvoker)(() => FillTree()));
                
            })
                .Start();
        }

        public void FillTree(string strFilter = "")
        {

            UpdateStatusText(strFillTree);

            var sw = StopwatchStart();

            var treeViewNodes = treeView1.Nodes;

            treeView1.BeginUpdate();
            treeViewNodes.Clear();

            // Фильтруем дерево

            var select = listFunction.List
                .Where(x =>
                    (strFilter.Length > 0 && x.Name.IndexOf(strFilter, StringComparison.OrdinalIgnoreCase) != -1)
                    || (strFilter.Length == 0)
                )
                .GroupBy(x => x.FileName)
                .OrderBy(x => x.Key);


            // Создаем ноды

            var countAll = select.Count();
            var count = 0;

            foreach (var functions in select)
            {
                var newNodeGorup = treeViewNodes.Add(functions.Key);
                foreach (var function in functions)
                {
                    var newNode = newNodeGorup.Nodes.Add(function.Name);
                    newNode.Tag = function;
                }
                count++;
                UpdateStatusPercent((byte)((float)count/countAll*100));
            }
            treeView1.EndUpdate();
            LogStopwatch("Заполнение дерева: ", sw);
        }

        private void TbFind_TextChanged(object sender, EventArgs e)
        {            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbPath.Text = @"d:\projects\C-sharp\FuncTreeFor1C\Тестовая конфа\";
            FindFilesAndParsing(tbPath.Text);
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var nodeTag = e.Node.Tag;
            if (nodeTag is FunctionInfo)
            {
                var function = e.Node.Tag as FunctionInfo;
                tbDescript.Lines = function.Descript?.ToArray();
            }
            else
            {
                ClearFields();
            }
            
        }

        private void ClearFields()
        {
            tbDescript.Text = "";
        }

        private void TreeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void TbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                FillTree(tbFind.Text);
        }
    }
}
