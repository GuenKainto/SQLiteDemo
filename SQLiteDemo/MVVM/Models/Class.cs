﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.MVVM.Models
{
    internal class Class : BaseNotifyPropertyChanged
    {
        private string _sClass;
        public string SClass
        {
            get => _sClass;
            set
            {
                if (_sClass != value)
                {
                    _sClass = value;
                    RaisePropertyChanged(nameof(SClass));
                }
            }
        }

        private Faculty _sFaculty;
        public Faculty SFaculty //Class.Faculty  in db
        {
            get => _sFaculty;
            set
            {
                if(_sFaculty != value)
                {
                    _sFaculty = value;
                    RaisePropertyChanged(nameof(SFaculty));
                }
            }
        }

        public Class(){}

        public Class(string sClass, Faculty sFaculty)
        {
            SClass = sClass;
            SFaculty = sFaculty;
        }
    }
}
