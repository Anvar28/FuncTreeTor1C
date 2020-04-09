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

namespace FuncTreeFor1CWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ModelFunction.xaml
    /// </summary>
    public partial class ModelFunction : UserControl
    {
        FunctionInfo _functionInfo;

        public ModelFunction(FunctionInfo functionInfo)
        {
            InitializeComponent();

            _functionInfo = functionInfo;

            var fileModule = functionInfo.FunctionList.FileModule;
            if (fileModule == null)
                new Exception("Для functionInfo не определен FileModule");

            // Читаем описание функции, это комментарий перед функцией
            tbxDescript.Document.Blocks.Clear();
            Paragraph paragraph = new Paragraph();
            foreach (var str in functionInfo.Descript)
            {
                paragraph.Inlines.Add(str + "\r\n");
            }
            tbxDescript.Document.Blocks.Add(paragraph);
            
            // 
            tbxBody.Document.Blocks.Clear();
            paragraph = new Paragraph();
            foreach (var str in functionInfo.Body)
            {
                paragraph.Inlines.Add(str + "\r\n");
            }
            tbxBody.Document.Blocks.Add(paragraph);
        }
    }
}
