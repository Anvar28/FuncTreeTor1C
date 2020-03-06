using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FuncTreeFor1CWPF
{
    public class MyApp : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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

        private 
    }
}
