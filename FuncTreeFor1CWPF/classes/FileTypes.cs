using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF.classes
{
    /// <summary>
    /// Базоый класс файлов
    /// </summary>
    public abstract class FileType
    {
        public string Name;
        public string FullName;
    }

    /// <summary>
    /// Файл модуля
    /// </summary>
    public class FileModule: FileType
    {
        public FunctionList FunctionList { get; set; }

        public FileModule()
        {
            FunctionList = new FunctionList(this);
        }
    }

    /// <summary>
    /// Файл с данными формы
    /// </summary>
    public class FileForm: FileType
    {
    }

    /// <summary>
    /// Файл с настройками объекта
    /// </summary>
    public class FileMdo : FileType
    {
    }

    /// <summary>
    /// Файл изображения
    /// </summary>
    public class FilePicture : FileType
    {
    }

    /// <summary>
    /// Файл табличный документ
    /// </summary>
    public class FileMXLX : FileType
    {
    }

    /// <summary>
    /// Файл html
    /// </summary>
    public class FileHTML : FileType
    {
    }

    /// <summary>
    /// Разные файлы которые не вошли в другие типы
    /// </summary>
    public class FileOther : FileType
    {
    }
}
