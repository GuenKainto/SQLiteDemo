using System;

namespace SQLiteDemo.MVVM.Models
{
    internal class Student : BaseNotifyPropertyChanged
    {
        private string _sID;
        public string SID //Student ID
        {
            get => _sID;
            set
            {
                if (_sID != value)
                {
                    _sID = value;
                    RaisePropertyChanged(nameof(SID));
                }
            }
        }

        private string _sName;
        public string SName // Student name
        {
            get => _sName;
            set
            {
                if (_sName != value)
                {
                    _sName = value;
                    RaisePropertyChanged(nameof(SName));
                }
            }
        }

        private Class _sClass;
        public Class SClass //Student Class
        {
            get => _sClass;
            set
            {
                if (value != _sClass)
                {
                    _sClass = value;
                    RaisePropertyChanged(nameof(SClass));
                }
            }
        }

        private string _sPhone;
        public string SPhone //Student Phone
        {
            get => _sPhone;
            set
            {
                if (value != _sPhone)
                {
                    _sPhone = value;
                    RaisePropertyChanged(nameof(SPhone));
                }
            }
        }

        private string _sAddress;
        public string SAddress //Student Address
        {
            get => _sAddress;
            set
            {
                if (value != _sAddress)
                {
                    _sAddress = value;
                    RaisePropertyChanged(nameof(SPhone));
                }
            }
        }

        private string _sDOB;
        public string SDOB //Student Date of Birth
        {
            get => _sDOB;
            set
            {
                if (_sDOB != value)
                {
                    _sDOB = value;
                    RaisePropertyChanged(nameof(SDOB));
                }
            }
        }

        public Student() { }

        public Student(string sID, string sName, Class sClass, string sDOB, string sPhone, string sAddress) // We will get Faculty in Class.SFaculty
        { 
            SID = sID;
            SName = sName;
            SClass = sClass;
            SDOB = sDOB;
            SPhone = sPhone;
            SAddress = sAddress;
        }
    }
}
