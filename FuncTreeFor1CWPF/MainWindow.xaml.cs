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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myApp.PathToSrc = @"d:\projects\C-sharp\FuncTreeFor1C\Тестовая конфа\";
        }

        private void BtnSelectPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myApp.PathToSrc = fbd.SelectedPath;
            }
        }
    }
}
