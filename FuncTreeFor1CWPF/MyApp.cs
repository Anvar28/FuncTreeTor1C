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

        private bool _flagFileSearchEnd;

        private FileQueue _fileQueue;

        private bool _flagParsingEnd;

        private FileTypesQueue _fileTypeQueue;

        #endregion

        #region FunctionPublic

        public void ScanFolderAndFill()
        {
            if (_pathToSrc.Length == 0)
                return;

            // Флаги используются в тасках
            _flagFileSearchEnd = false;
            _flagParsingEnd = false;

            // Очереди обработки, используются в тасках
            _fileQueue = new FileQueue();
            _fileTypeQueue = new FileTypesQueue();
            _finderList = new FinderList();

            //
            // Ищем все файлы в отдельном потоке
            //
            var taskFileSearch = Task.Factory.StartNew(FindFiles);

            //
            // Запускаем задачу заполнения списка для поиска
            //
            Task.Factory.StartNew(FillFinderList);

            //
            // Парсим файлы в несколько потоков
            //

            // Определяем сколько ядер в система и создадим столько же потоков по парсингу.
            var numberCores = Environment.ProcessorCount;
            var tasks = new Task[numberCores];
            for (int i = 0; i < numberCores; i++)
            {
                tasks[i] = Task.Factory.StartNew(ParseFiles);
            }
            
            // После выполнения всез задач будет сбрасываться флаг
            Task.Factory.ContinueWhenAll(tasks, (taskResult) => { _flagParsingEnd = true; });

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
            // Если задание по поиску файлов еще работает, то будем парсить файлы
            while (true)
            {
                if (_flagFileSearchEnd && _fileQueue.Count() == 0)
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
        }

        private void FindFiles()
        {
            FileSearcher.Search(_fileQueue, _pathToSrc);
            _flagFileSearchEnd = true;
        }

        private void FillFinderList()
        {
            // Если парсинг файлов еще не закончен, то будет продолжать заполнять
            // список поиска
            while (true)
            {
                if (_flagParsingEnd && _fileTypeQueue.Count() == 0)
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
