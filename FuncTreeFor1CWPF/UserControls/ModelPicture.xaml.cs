using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FuncTreeFor1CWPF.FileTypes;

namespace FuncTreeFor1CWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ModelPicture.xaml
    /// </summary>
    public partial class ModelPicture : UserControl
    {

        FilePicture _file;

        public ModelPicture(FilePicture file)
        {
            InitializeComponent();

            _file = file;

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(_file.FullName, UriKind.Absolute);
            src.EndInit();
            img.Source = src;
        }
    }
}
