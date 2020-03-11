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
            treeView.ItemsSource = myApp.TreeItem;
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myApp.PathToSrc = @"d:\projects\C-sharp\FuncTreeFor1C\Тестовая конфа\";
            myApp.FillFinderList();
            FillTreeNode();
        }

        private void BtnSelectPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myApp.PathToSrc = fbd.SelectedPath;
                myApp.FillFinderList();
                FillTreeNode();
            }
        }
        private void TbxFilter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tbxFilter.Text.Length > 2)
                {
                    FillTreeNode();
                }
            }
        }
        #endregion

        #region Func

        public void FillTreeNode()
        {
            treeView.ItemsSource = null;
            myApp.FillTreeItem(tbxFilter.Text);
            treeView.ItemsSource = myApp.TreeItem;
        }

        #endregion

    }
}
