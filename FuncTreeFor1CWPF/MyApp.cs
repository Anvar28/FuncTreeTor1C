using FuncTreeFor1CWPF.classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

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

        #endregion

        #region FunctionPublic

        public void FillFinderList()
        {
            if (_pathToSrc.Length == 0)
                return;

            // Ищем все файлы
            var files = FileSearcher.Search(_pathToSrc, UpdateStatusPercentAsync);

            // Парсим файлы
            var parserFile = new ParserFile();
            var funcList = parserFile.ParseFiles(files, UpdateStatusPercentAsync);

            // Заполняем список для поиска, результатами парсинга
            _finderList = new FinderList(funcList, _pathToSrc.Length, UpdateStatusPercentAsync);

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

        #endregion
    }
}
