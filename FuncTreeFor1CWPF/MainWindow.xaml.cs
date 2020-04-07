using FuncTreeFor1CWPF.classes;
using FuncTreeFor1CWPF.UserControls;
using FuncTreeFor1CWPF.TreeNodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TabItemInfoList tabList;

        public MainWindow()
        {
            myApp = new MyApp();
            InitializeComponent();
            this.DataContext = myApp;
            tbxPathToSrc.DataContext = myApp;
            treeView.ItemsSource = myApp.Tree.Nodes;

            tabList = new TabItemInfoList();
            OpenedFiles.ItemsSource = tabList;
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            // Проверка выделен ли какой либо элемент дерева
            var selectNode = ((System.Windows.Controls.TreeView)e.Source).SelectedValue;
            if (!(selectNode is FuncTreeFor1CWPF.TreeNodes.TreeNode))
            {
                return;
            }

            var obj = ((FuncTreeFor1CWPF.TreeNodes.TreeNode)selectNode).Obj;

            if (obj == null)
            {
                return;
            }

            // Проверка есть ли уже открытая вкладка с данным объектом
            var existTab = tabList.FirstOrDefault(x => x.Obj == obj);
            if (existTab != null)
            {
                OpenedFiles.SelectedItem = existTab;
                return;
            }

            // Создаем новую вкладку и добавляем
            var newTab = TabItemInfoFabric.NewTabItemInfo(obj);
            if (newTab != null)
            {
                tabList.Add(newTab);
                OpenedFiles.SelectedItem = newTab;
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

        #endregion


    }

}
