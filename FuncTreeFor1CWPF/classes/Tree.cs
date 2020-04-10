using FuncTreeFor1CWPF.TreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class Tree
    {
        const string strConfiguration = "Конфигурация";

        public TreeNodeList Nodes { get; }
        public TreeNode Root { get; }

        public Tree()
        {
            Nodes = new TreeNodeList();
            Root = new TreeNode(strConfiguration, null);
            Nodes.Add(Root);
        }

        public void Add(FinderItem finderItem)
        {                 
            
            // Добавляем родителей элемента
            var currentNodeItem = Root;
            foreach (var parentItem in finderItem.Parents)
            {
                TreeNode treeNodeItem;
                treeNodeItem = currentNodeItem.Nodes.FirstOrDefault(x => x.Name == parentItem);
                if (treeNodeItem != null)
                {
                    currentNodeItem = treeNodeItem;
                }
                else
                {
                    // Очередная ветвь дерева не найдена, создаем новую
                    var newTreeItem = new TreeNode(parentItem, null);
                    App.Current.Dispatcher.Invoke((System.Action)delegate
                    {
                        lock(currentNodeItem.Nodes)
                        {
                            currentNodeItem.Nodes.Add(newTreeItem);
                        }
                    });
                    currentNodeItem = newTreeItem;
                }
            }

            // Добавляем элемент
            TreeNode newTreeNodeItem = null;
            if (finderItem.Object is FunctionInfo)
            {
                // Новый элемент, это описание функции или процедуры
                var func = finderItem.Object as FunctionInfo;
                newTreeNodeItem = new TreeNode_Method(
                    finderItem.Name,
                    finderItem.Object,
                    func.Type,
                    func.Export);
            }
            else
            {
                // Новый элемент, это не понятно что.
                newTreeNodeItem = new TreeNode(finderItem.Name, finderItem.Object);
            }
            currentNodeItem.Nodes.Add(newTreeNodeItem);
        }

        public void Fill(IOrderedEnumerable<FinderItem> selectFinderList)
        {
            Clear();
            foreach (var selectFinderItem in selectFinderList)
            {
                Add(selectFinderItem);
            }
        }

        public void Clear()
        {
            Root.Nodes.Clear();
        }
    }
}
