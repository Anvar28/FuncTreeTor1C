﻿using FuncTreeFor1C.classes;
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
        FileTypesList listFiles;
        FinderList finderList;
        ILoger loger;

        const string strFindFiles = "Поиск файлов...";
        const string strParse = "Парсинг...";
        const string strFillTree = "Заполнение дерева...";
        const string strPrepareTreeNode = "Подготовка дерева...";

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

        private void FindFilesAndParsing(string path)
        {
            new Task(() =>
            {
                //
                // Поиск файлов
                //

                this.BeginInvoke((MethodInvoker)(() => UpdateStatusText(strFindFiles)));

                var sw = StopwatchStart();

                var files = FileSearcher.Search(
                    path,
                    (percent) =>
                    {
                        this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
                    }
                );

                LogStopwatch("Сканирование файлов: ", sw);

                //
                // Парсинг
                //

                this.BeginInvoke((MethodInvoker)(() => UpdateStatusText(strParse)));

                sw.Start();

                var parserFile = new ParserFile();

                listFiles = parserFile.ParseFiles(
                    files,
                    (percent) =>
                    {
                        this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
                    }
                );

                LogStopwatch("Парсинг файлов: ", sw);

                //
                // Создаем список поисковика, в него будем помещать все объекты по которым необходимо будет делать поиск
                // 

                var countAll = listFiles.Count();
                var count = 0;

                var separetor = new char[] { '\\' };
                finderList = new FinderList();

                var countCutFullPath = path.Length;

                foreach (var file in listFiles)
                {
                    var mParents = file.FullName.Substring(countCutFullPath).Split(separetor);

                    if (file is FileModule)
                    {
                        var fileModule = (FileModule)file;
                        foreach (var item in fileModule.FunctionList)
                        {
                            var newFinderItem = finderList.Add(item.Name, item);
                            newFinderItem.Parents.AddRange(mParents);
                        }
                    }
                    else
                    {
                        var newFinderItem = finderList.Add(file.Name, file);
                        newFinderItem.Parents.AddRange(mParents);
                    }
                    count++;
                    var percent = (byte)((float)count / countAll * 100);
                    this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
                }
                LogStopwatch("Создание списка поисковика: ", sw);

                //
                // Заполняем дерево
                //

                this.BeginInvoke((MethodInvoker)(() => FillTree()));

            })
                .Start();
        }

        public void FillTree(string strFilter = "")
        {

            UpdateStatusText("Фильтрация");
            UpdateStatusPercent(0);

            var sw = StopwatchStart();

            var treeViewNodes = treeView1.Nodes;

            treeView1.BeginUpdate();
            treeViewNodes.Clear();

            //
            // Фильтруем дерево
            //

            sw.Start();

            var selectFinderList = finderList.List
                .Where(x =>
                    (strFilter.Length > 0 && x.Name.IndexOf(strFilter, StringComparison.OrdinalIgnoreCase) != -1)
                    || (strFilter.Length == 0)
                );

            LogStopwatch("Фильтрация списка: ", sw);

            //
            // Создаем древовидную структуру из тех элементов которые попали в отфильтрованный список
            // 

            UpdateStatusText("Создание дерева");
            UpdateStatusPercent(33);

            sw.Start();

            var root = new TreeItemModel("Конфигурация", null);
            var currentNodeItem = root;
            foreach (var selectFinderItem in selectFinderList)
            {
                
                // Добавляем родителей элемента

                currentNodeItem = root;
                foreach (var parentItem in selectFinderItem.Parents)
                {
                    var treeNodeItem = currentNodeItem.Children.FirstOrDefault(x => x.Name == parentItem);
                    if (treeNodeItem != null)
                    {
                        currentNodeItem = treeNodeItem;
                    }
                    else
                    {
                        // Очередная ветвь дерева не найдена, создаем новую
                        var newTreeNodeItem = new TreeItemModel(parentItem, null);
                        currentNodeItem.Children.Add(newTreeNodeItem);
                        currentNodeItem = newTreeNodeItem;
                    }
                }

                // Добавляем элемент

                if (selectFinderItem.Object is FunctionInfo)
                {
                    var func = selectFinderItem.Object as FunctionInfo;
                    var newTreeNodeItem = new TreeItemModelMethod(
                        selectFinderItem.Name, 
                        selectFinderItem.Object,
                        func.Type,
                        func.Export);
                }
                else
                {
                    var newTreeNodeItem = new TreeItemModel(selectFinderItem.Name, selectFinderItem.Object);
                }
                
            }

            LogStopwatch("Создание дерева: ", sw);

            //
            // Создаем ноды
            // 

            UpdateStatusText("Заполнение дерева");
            UpdateStatusPercent(66);

            sw.Start();

            var rootTreeNode = treeViewNodes.Add(root.Name);
            CreateTreeNodes(rootTreeNode, root);

            treeView1.EndUpdate();
            LogStopwatch("Заполнение дерева: ", sw);

            UpdateStatusPercent(0);
            UpdateStatusText("");
        }

        private void CreateTreeNodes(TreeNode currentTreeNode, TreeItemModel model)
        {
            foreach (var item in model.Children)
            {
                var newTreeNode = currentTreeNode.Nodes.Add(item.Name);
                CreateTreeNodes(newTreeNode, item);
            }
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
