using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FuncTreeFor1CWPF.classes
{

    public class TabItemInfo
    {
        public Object Obj { get; }
        public string Header { get; set; }
        public System.Windows.Controls.ToolTip ToolTip { get; set; }
        public FrameworkElement TabContent { get; set; }

        public TabItemInfo(Object obj)
        {
            Obj = obj;
        }
    }
}
