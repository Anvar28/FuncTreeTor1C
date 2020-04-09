using FuncTreeFor1CWPF.FileTypes;
using FuncTreeFor1CWPF.XMLTree;
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
using System.Xml;

namespace FuncTreeFor1CWPF.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ModelXML.xaml
    /// </summary>
    public partial class ModelXML : UserControl
    {
        FileType _file;
        XMLTreeNodeList nodes;

        public ModelXML(FileType file)
        {
            InitializeComponent();

            nodes = new XMLTreeNodeList();
            TreeView.ItemsSource = nodes;

            _file = file;

            var document = new XmlDocument();
            try
            {
                document.Load(file.FullName);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка анализа XML файла");
                throw;
            }
            XmlNode root = document.DocumentElement;

            FillNode(root, nodes);
        }


        /// <summary>
        /// Рекурсивный обход XML файла и создания дерева 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="nodes"></param>
        private void FillNode(XmlNode root, XMLTreeNodeList nodes)
        {
            foreach (XmlNode item in root.ChildNodes)
            {
                var node = new XMLTreeNode();
                node.Name = item.Value != null ? item.Value : item.Name;
                nodes.Add(node);

                if (item.ChildNodes.Count > 0)
                    FillNode(item, node.Nodes);
            }
        }
    }
}
