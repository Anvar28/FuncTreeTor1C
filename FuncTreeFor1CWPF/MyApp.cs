using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FuncTreeFor1CWPF
{
    public class MyApp : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Constructor

        public MyApp()
        {
            _loger = new LogerFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log.txt"));
            _treeItem = new Tree();
            _finderList = new FinderList();
        }

        #endregion

        #region Properties

        public ILoger _loger;

        private string _pathToSrc;
        public string PathToSrc
        {
            get { return _pathToSrc; }
            set
            {
                if (_pathToSrc != value)
                {
                    _pathToSrc = value;
                    OnPropertyChanged("PathToSrc");
                }
            }
        }

        private Tree _treeItem;
        public Tree Tree
        {
            get { return _treeItem; }
        }

        public FinderList _finderList;

        private bool _flagFileQueueIsEnd;

        private FileQueue _fileQueue;

        private bool _flagFunctionQueueIsEnd;

        private FileTypesQueue _fileTypeQueue;

        public bool OnlyMethods { get; set; } 
        #endregion

        #region FunctionPublic

        public void ScanFolderAndFill()
        {
            if (_pathToSrc.Length == 0)
                return;

            // Ищем все файлы в потоке
            _fileQueue = new FileQueue();
            new Task(FindFiles).Start();

            // Парсим файлы в несколько потоков
            _fileTypeQueue = new FileTypesQueue();
            new Task(ParseFiles).Start();
            new Task(ParseFiles).Start();
            new Task(ParseFiles).Start();
            new Task(ParseFiles).Start();

            // Заполняем список для поиска, результатами парсинга
            _finderList = new FinderList();
            new Task(FillFinderList).Start();
        }

        public void FillTreeItem(string filter = "")
        {
            // Ищим объекты
            var selectFinderList = Finder.Find(_finderList, filter);

            // Заполняем дерево
            _treeItem.Fill(selectFinderList);
        }

        public void UpdateStatusPercentAsync(byte percent)
        {
            //this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
        }

        #endregion

        #region FunctionPrivate

        private void ParseFiles()
        {
            _flagFunctionQueueIsEnd = false;

            // Если задание по поиску файлов еще работает, то будем парсить файлы
            while (true)
            {
                if (_flagFileQueueIsEnd && _fileQueue.Count() == 0)
                {
                    break;
                }

                if (_fileQueue.Count() > 0)
                {
                    FileInfo fileInfo;
                    _fileQueue.TryDequeue(out fileInfo);
                    if (fileInfo != null)
                    {
                        // Получаем новый тип файла, если это файл модуля, то он
                        // будет уже со списком отпарсенных функций
                        var fileType = FileTypeFabric.NewFileType(fileInfo);
                        _fileTypeQueue.Enqueue(fileType);
                    }
                }
                else
                {
                    Task.Delay(50);
                }
            }

            _flagFunctionQueueIsEnd = true;
        }

        private void FindFiles()
        {
            _flagFileQueueIsEnd = false;
            FileSearcher.Search(_fileQueue, _pathToSrc);
            _flagFileQueueIsEnd = true;
        }

        private void FillFinderList()
        {
            // Если парсинг файлов еще не закончен, то будет продолжать заполнять
            // список поиска
            while (true)
            {
                if (_flagFunctionQueueIsEnd && _fileTypeQueue.Count() == 0)
                {
                    break;
                }
                if (_fileTypeQueue.Count() > 0)
                {
                    FileType fileType;
                    _fileTypeQueue.TryDequeue(out fileType);
                    if (fileType != null)
                    {
                        var addedItems = _finderList.AddFromFileTypes(fileType, _pathToSrc.Length);
                        
                        // сразу добавляем только что добавленные объекты в дерево
                        foreach (var item in addedItems)
                        {
                            _treeItem.Add(item);
                        }
                    }
                }
                else
                {
                    Task.Delay(50);
                }
            }
        }
        #endregion
    }
}
