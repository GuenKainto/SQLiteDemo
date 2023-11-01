using System;

namespace SQLiteDemo.MVVM.Models
{
    internal class Teacher : BaseNotifyPropertyChanged
    {
        private string _tID;
        public string TID
        {
            get => _tID;
            set
            {
                if(_tID != value){
                    _tID = value;
                    RaisePropertyChanged(nameof(TID));
                }
            }
        }

        private string _tName;
        public string TName
        {
            get => _tName;
            set
            {
                if( _tName != value)
                {
                    _tName = value;
                    RaisePropertyChanged(nameof(TName));
                }
            }
        }

        private Faculty _tFaculty;
        public Faculty TFaculty
        {
            get => _tFaculty;
            set
            {
                if ( _tFaculty != value)
                {
                    _tFaculty = value;
                    RaisePropertyChanged(nameof(TFaculty));
                }
            }
        }

        private string _tDOB;
        public string TDOB
        {
            get => _tDOB;
            set
            {
                if(_tDOB != value)
                {
                    _tDOB = value;
                    RaisePropertyChanged(nameof(TDOB));
                }
            }
        }

        private string _tAddress;
        public string TAddress
        {
            get => _tAddress;
            set
            {
                if(_tAddress != value)
                {
                    _tAddress = value;
                    RaisePropertyChanged(nameof(TAddress));
                }
            }
        }

        private string _tPhone;
        public string TPhone
        {
            get => _tPhone;
            set
            {
                if(_tPhone != value)
                {
                    _tPhone = value;
                    RaisePropertyChanged(nameof(TPhone));
                }
            }
        }

        public Teacher() { }

        public Teacher(string tID, string tName, Faculty tFaculty, string tDOB, string tAddress, string tPhone)
        {
            TID = tID;
            TName = tName;
            TFaculty = tFaculty;
            TDOB = tDOB;
            TAddress = tAddress;
            TPhone = tPhone;
        }
    }
}
