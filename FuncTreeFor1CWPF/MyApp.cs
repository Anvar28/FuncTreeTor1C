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
            _treeItem = new TreeModel();
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

        private TreeModel _treeItem;
        public TreeModel TreeItem
        {
            get { return _treeItem; }
        }

        public FinderList _finderList;

        private bool _flagFileQueueIsEnd;

        private FileQueue _fileQueue;

        private bool _flagFunctionQueueIsEnd;

        private FileTypesQueue _fileTypeQueue;

        #endregion

        #region FunctionPublic

        public void FillFinderList()
        {
            if (_pathToSrc.Length == 0)
                return;

            _fileQueue = new FileQueue();

            //
            // Ищем все файлы в потоке
            //

            new Task(() => {
                _flagFileQueueIsEnd = false;
                FileSearcher.Search(_fileQueue, _pathToSrc);
                _flagFileQueueIsEnd = true;
            }).Start();

            //
            // Парсим файлы в несколько потоков
            //

            _fileTypeQueue = new FileTypesQueue();

            new Task(() =>
            {
                _flagFunctionQueueIsEnd = false;

                Task.Delay(200);

                // Если задание по поиску файлов еще работает, то будем парсить файлы
                while (!_flagFileQueueIsEnd)
                {
                    // Запускаем перебор файлов и парсинг в несколько потоков
                    var parserFile = new ParserFile();
                    parserFile.ParseFiles(_fileQueue, _fileTypeQueue);
                }

                _flagFunctionQueueIsEnd = true;

            }).Start();

            // Заполняем список для поиска, результатами парсинга

            _finderList = new FinderList();

            new Task(() =>
            {
                Task.Delay(200);
                // Если парсинг файлов еще не закончен, то будет продолжать заполнять
                // список поиска
                while (!_flagFunctionQueueIsEnd)
                {
                    Task.Delay(5);
                    foreach (var fileType in _fileTypeQueue)
                    {
                        _finderList.AddFromFileTypes(fileType, _pathToSrc.Length);
                    }
                }
            }).Start();

        }

        public void FillTreeItem(string filter = "")
        {
            // Ищим объекты
            var selectFinderList = Finder.Find(_finderList, filter);

            // Заполняем дерево
            _treeItem = new TreeModel(selectFinderList);
        }

        public void UpdateStatusPercentAsync(byte percent)
        {
            //this.BeginInvoke((MethodInvoker)(() => UpdateStatusPercent(percent)));
        }

        #endregion

        #region FunctionPrivate

        private void ParseFiles()
        {
        }

        #endregion
    }
}
