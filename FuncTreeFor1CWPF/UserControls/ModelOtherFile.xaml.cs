using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ModelOtherFile.xaml
    /// </summary>
    public partial class ModelOtherFile : UserControl
    {

        FileType fileModule;

        public ModelOtherFile(FileType obj)
        {
            InitializeComponent();

            fileModule = obj;
            var text = File.ReadAllLines(fileModule.FullName);

            tbxBody.Document.Blocks.Clear();
            var paragraph = new Paragraph();
            foreach (var str in text)
            {
                paragraph.Inlines.Add(str + "\r\n");
            }
            tbxBody.Document.Blocks.Add(paragraph);
        }
    }
}
