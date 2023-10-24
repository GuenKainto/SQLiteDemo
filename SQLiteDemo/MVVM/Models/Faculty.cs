using SQLiteDemo.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDemo.MVVM.Models
{
    internal class Faculty : BaseNotifyPropertyChanged
    {
        private FacultyDB facDB = new FacultyDB(); 

        private string _fac;
        public string Fac
        {
            get => _fac;
            set
            {
                if (_fac != value)
                {
                    _fac = value;
                    RaisePropertyChanged(nameof(Fac));
                }
            }
        }
        private int _noStudent;
        public int NoStudent
        {
            get => _noStudent;
            set
            {
                _noStudent = facDB.GetNoStudent(Fac);
                RaisePropertyChanged(nameof(NoStudent));
            }
        }

        private int _noTeacher;
        public int NoTeacher
        {
            get => _noTeacher;
            set
            {
                _noTeacher = facDB.GetNoTeacher(Fac);
                RaisePropertyChanged(nameof(NoTeacher));
            }
        }

        private int _noClass;
        public int NoClass
        {
            get => _noClass;
            set
            {
                _noClass = facDB.GetNoClass(Fac);
                RaisePropertyChanged(nameof(NoClass));
            }
        }

        public Faculty() { }
        public Faculty(string fac)
        {
            Fac = fac;
        }
    }
}
