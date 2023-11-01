using SQLiteDemo.DAO;

namespace SQLiteDemo.MVVM.Models
{
    internal class Faculty : BaseNotifyPropertyChanged
    {
        private FacultyDB facDB = new FacultyDB(); 

        private string _fac;
        public string Fac //Faculty
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
        private int _noStudent = 0;
        public int NoStudent // Number of Student
        {
            get => _noStudent;
            set
            {
                _noStudent = value;
                RaisePropertyChanged(nameof(NoStudent));
            }
        }

        private int _noTeacher = 0;
        public int NoTeacher // Number of Teacher
        {
            get => _noTeacher;
            set
            {
                _noTeacher = value;
                RaisePropertyChanged(nameof(NoTeacher));
            }
        }

        private int _noClass = 0;
        public int NoClass // Number of Class
        {
            get => _noClass;
            set
            {
                if(value != _noClass)
                {
                    _noClass = value;
                    RaisePropertyChanged(nameof(NoClass));
                }
            }
        }

        public Faculty() { }
        public Faculty(string fac)
        {
            Fac = fac;
        }

        public void SetNumber()
        {
            NoClass = facDB.GetNoClass(Fac);
            NoTeacher = facDB.GetNoTeacher(Fac);
            NoStudent = facDB.GetNoStudent(Fac);
        }
    }
}
