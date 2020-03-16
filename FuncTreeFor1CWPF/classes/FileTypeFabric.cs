using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    public class FileTypeFabric
    {
        const string extensionBSL = ".bsl";
        const string extensionMDO = ".mdo";
        const string extensionPNG = ".png";
        const string extensionMXLX = ".mxls";
        const string extensionHTML = ".html";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FileType NewFileType(FileInfo file)
        {
            FileType newFileType;

            if (file.Extension == extensionBSL)
            {
                var text = File.ReadAllLines(file.FullName);
                var newFileModule = new FileModule();
                newFileModule.FunctionList = Parser.Parse(text, newFileModule);
                newFileType = newFileModule;
            }
            else if (file.Extension == extensionMDO)
            {
                newFileType = new FileMdo();
            }
            else if (file.Extension == extensionPNG)
            {
                newFileType = new FilePicture();
            }
            else if (file.Extension == extensionMXLX)
            {
                newFileType = new FileMXLX();
            }
            else if (file.Extension == extensionHTML)
            {
                newFileType = new FileHTML();
            }
            else
            {
                newFileType = new FileOther();
            }

            newFileType.Name = file.Name;
            newFileType.FullName = file.FullName;
            return newFileType;

        }
    }
}
