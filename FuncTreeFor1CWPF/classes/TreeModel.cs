using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class TreeModel : ObservableCollection<TreeItemModel>
    {
        const string strConfiguration = "Конфигурация";

        public TreeModel()
        {

        }

        public TreeModel(IOrderedEnumerable<FinderItem> selectFinderList)
        {            
            var root = new TreeItemModel(strConfiguration, null);
            this.Add(root);
            var currentNodeItem = root;
            foreach (var selectFinderItem in selectFinderList)
            {

                // Добавляем родителей элемента

                currentNodeItem = root;
                foreach (var parentItem in selectFinderItem.Parents)
                {
                    var treeNodeItem = currentNodeItem.Children.FirstOrDefault(x => x.Name == parentItem);
                    if (treeNodeItem != null)
                    {
                        currentNodeItem = treeNodeItem;
                    }
                    else
                    {
                        // Очередная ветвь дерева не найдена, создаем новую
                        var newTreeItem = new TreeItemModel(parentItem, null);
                        currentNodeItem.Children.Add(newTreeItem);
                        currentNodeItem = newTreeItem;
                    }
                }

                // Добавляем элемент
                TreeItemModel newTreeNodeItem = null;
                if (selectFinderItem.Object is FunctionInfo)
                {
                    // Новый элемент, это описание функции или процедуры
                    var func = selectFinderItem.Object as FunctionInfo;
                    newTreeNodeItem = new TreeItemModel_Method(
                        selectFinderItem.Name,
                        selectFinderItem.Object,
                        func.Type,
                        func.Export);
                }
                else
                {
                    // Новый элемент, это не понятно что.
                    newTreeNodeItem = new TreeItemModel(selectFinderItem.Name, selectFinderItem.Object);
                }
                currentNodeItem.Children.Add(newTreeNodeItem);
            }
        }
    }
}
