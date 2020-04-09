using FuncTreeFor1CWPF.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FuncTreeFor1CWPF.FileTypes;

namespace FuncTreeFor1CWPF.classes
{
    class TabItemInfoFabric
    {
        /// <summary>
        /// Создает новый TabItemInfo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TabItemInfo NewTabItemInfo(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var descript = GetNameAndToolTip(obj);
            var model = GetModel(obj);
            
            // Для некоторых типов объектов, моделей может не быть.
            if (model == null)
            {
                return null;
            }

            var tabInfo = new TabItemInfo(obj)
            {
                Header = descript.Name,
                ToolTip = new System.Windows.Controls.ToolTip
                {
                    Content = descript.ToolTip
                },
                TabContent = model
            };

            return tabInfo;
        }

        /// <summary>
        /// Возвращает модель просмоторщика файла
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static FrameworkElement GetModel(object obj)
        {
            FrameworkElement result = null;

            if (obj is FileModule)
            {
                result = new ModelOtherFile((FileType)obj);
            }
            else if (obj is FunctionInfo)
            {
                result = new ModelFunction((FunctionInfo)obj);
            }
            else if (obj is FileForm)
            {
                result = new ModelXML((FileType)obj);
            }
            else if (obj is FileMdo)
            {
                result = new ModelXML((FileType)obj);
            }
            else if (obj is FilePicture)
            {
                result = new ModelPicture((FilePicture)obj);
            }
            else if (obj is FileMXLX)
            {
                result = new ModelXML((FileType)obj);
            }
            else if (obj is FileHTML)
            {
                result = new ModelXML((FileType)obj);
            }
            else if (obj is FileZIP)
            {
                result = null;
            }
            else if (obj is FileOther)
            {
                result = new ModelOtherFile((FileType)obj);
            }
            else
            {
                new Exception(" Не определен тип.");
            }

            return result;
        }

        /// <summary>
        /// Возвращает наименование закладки и подсказку
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static (string Name, string ToolTip) GetNameAndToolTip(object obj)
        {
            var name = "";
            var toolTip = "";

            FileType fileType = null;
            if (obj is FileType)
            {
                fileType = obj as FileType;
                name = fileType.Name;
            }
            else if (obj is FunctionInfo)
            {
                var funcInfo = obj as FunctionInfo;
                fileType = funcInfo.FunctionList.FileModule;
                name = funcInfo.Name;
            }

            if (fileType == null)
            {
                throw new Exception(" Не определен тип.");
            }

            toolTip = fileType.FullName;

            return (name, toolTip);
        }
    }
}
