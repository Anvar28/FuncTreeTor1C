using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FuncTreeFor1CWPF
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        public MyApp myApp;

        public MainWindow()
        {
            myApp = new MyApp();
            InitializeComponent();
            this.DataContext = myApp;
            tbxPathToSrc.DataContext = myApp;
            treeView.ItemsSource = myApp.Tree.Nodes;
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HideAllObjectPanel();
            myApp.PathToSrc = @"d:\projects\C-sharp\FuncTreeFor1C\Тестовая конфа\";
            myApp.ScanFolderAndFill();
            FilterTreeNode();
        }

        private void BtnSelectPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = myApp.PathToSrc;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myApp.PathToSrc = fbd.SelectedPath;
                myApp.ScanFolderAndFill();
                FilterTreeNode();
            }
        }
        private void TbxFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterTreeNode();
                ExpandTreeRoot();
            }
        }
        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            myApp.FillTreeItem();
        }
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectNode = ((System.Windows.Controls.TreeView)e.Source).SelectedValue;
            if (!(selectNode is FuncTreeFor1CWPF.classes.TreeNode))
            {
                return;
            }

            var obj = ((FuncTreeFor1CWPF.classes.TreeNode)selectNode).Obj;
            FuncTreeFor1CWPF.classes.ViewBase viewObj = null;
            FrameworkElement pnl = null;

            if (obj is FileModule)
            {
                pnl = pnlForOther;
            }
            else if (obj is FunctionInfo)
            {
                pnl = pnlForModule;
                viewObj = new ViewFunction((FunctionInfo)obj);
            }
            else if (obj is FileForm)
            {
                pnl = pnlForOther;
            }
            else if (obj is FileMdo)
            {
                pnl = pnlForOther;
                viewObj = new ViewOtherFile(((FileMdo)obj).FullName);                
            }
            else if (obj is FilePicture)
            {
                pnl = pnlForOther;
            }
            else if (obj is FileMXLX)
            {
                pnl = pnlForOther;
            }
            else if (obj is FileHTML)
            {
                pnl = pnlForOther;
            }
            else if (obj is FileOther)
            {
                pnl = pnlForOther;
            }

            HideAllObjectPanel();

            if (pnl != null)
            {
                pnl.Visibility = Visibility.Visible;
                if (viewObj != null)
                {
                    if (viewObj is ViewOtherFile)
                    {
                        var viewOtherFile = viewObj as ViewOtherFile;
                        rtb.Document.Blocks.Clear();

                        Paragraph paragraph = new Paragraph();

                        foreach (var str in viewOtherFile.Text)
                        {
                            paragraph.Inlines.Add(str + "\r\n");
                        }

                        rtb.Document.Blocks.Add(paragraph);

                    } else if (viewObj is ViewFunction) {

                        var viewOtherFile = viewObj as ViewFunction;

                        tbxComments.Document.Blocks.Clear();
                        Paragraph paragraph = new Paragraph();
                        foreach (var str in viewOtherFile.Descript)
                        {
                            paragraph.Inlines.Add(str + "\r\n");
                        }
                        tbxComments.Document.Blocks.Add(paragraph);

                        tbxDescript.Document.Blocks.Clear();
                        paragraph = new Paragraph();
                        foreach (var str in viewOtherFile.Text)
                        {
                            paragraph.Inlines.Add(str + "\r\n");
                        }
                        tbxDescript.Document.Blocks.Add(paragraph);

                    }
                }
            }
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
        #endregion

        #region Func

        public void FilterTreeNode()
        {
            myApp.FillTreeItem(tbxFilter.Text);
        }

        private void ExpandTreeRoot()
        {
            treeView.Focus();
            var treeItem = treeView.ItemContainerGenerator.ContainerFromItem(myApp.Tree.Root) as TreeViewItem;
            if (treeItem != null)
            {
                treeItem.IsSelected = true;
                treeItem.IsExpanded = true;
            }
        }

        public void HideAllObjectPanel()
        {
            foreach (var item in pnlRight.Children)
            {
                ((UIElement)item).Visibility = Visibility.Hidden;
            }
        }
        #endregion


    }
}
