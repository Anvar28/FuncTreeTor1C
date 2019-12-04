using FuncTreeFor1C.classes;
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
                FindFilesAndParsing(tbPath.Text);
                FillTree();
            }
        }

        private void FindFilesAndParsing(string path)
        {
            var lenPath = path.Length;
            var files = Searcher.Search(path);
            foreach (var file in files)
            {
                var text = File.ReadAllLines(file.FullName);
                var newListFunction = Parser.ParseStrings(text);
                if (newListFunction.Count() > 0)
                {
                    foreach (var item in newListFunction)
                    {
                        item.FileName = file.FullName.Substring(lenPath);
                        listFunction.Add(item);
                    }
                }
            }
            FillTree();
        }

        public void FillTree(string strFilter = "")
        {
            treeView1.Nodes.Clear();

            var treeViewNodes = treeView1.Nodes;
            var select = listFunction.List
                .Where(x =>
                    (strFilter.Length > 0 && x.Name.IndexOf(strFilter, StringComparison.OrdinalIgnoreCase) != -1)
                    || (strFilter.Length == 0)
                )
                .GroupBy(x => x.FileName)
                .OrderBy(x => x.Key);

            foreach (var functions in select)
            {
                var newNodeGorup = treeViewNodes.Add(functions.Key);
                foreach (var function in functions)
                {
                    var newNode = newNodeGorup.Nodes.Add(function.Name);
                    newNode.Tag = function;
                }
            }
        }

        private void TbFind_TextChanged(object sender, EventArgs e)
        {
            FillTree(tbFind.Text);
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
    }
}
