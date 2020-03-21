using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class ViewFunction: ViewBase
    {
        public List<string> Descript { get; set; }
        public List<string> Text { get; set; }

        public ViewFunction(FunctionInfo functionInfo)
        {
            var fileModule = functionInfo.FunctionList.FileModule;
            if (fileModule == null)
                return;

            IEnumerable<string> result = File.ReadLines(fileModule.FullName).Skip(functionInfo.IndexStartDescript);

            Descript = new List<string>();

            var descript = result.Take(functionInfo.IndexStart - functionInfo.IndexStartDescript).ToList();
            foreach (var item in descript)
            {
                var tmp = item.Trim();
                if (tmp.Length > 2)
                    Descript.Add(tmp.Substring(2).Trim());
                else
                    Descript.Add("");
            }

            Text = new List<string>();

            result = result.Skip(functionInfo.IndexStart - functionInfo.IndexStartDescript);

            foreach (string str in result)
            {
                Text.Add(str);
                var tmpStr = str.ToLower();
                if ((tmpStr.Length >= 12 && tmpStr.Substring(0, 12) == "конецфункции") 
                 || (tmpStr.Length >= 14 && tmpStr.Substring(0, 14) == "конецпроцедуры"))
                {
                    break;
                }
            }
        }
    }
}
