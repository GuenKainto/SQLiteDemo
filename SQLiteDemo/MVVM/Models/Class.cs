using SQLiteDemo.DAO;

namespace SQLiteDemo.MVVM.Models
{
    internal class Class : BaseNotifyPropertyChanged
    {
        private ClassDB classDB = new ClassDB();
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

        private int _noStudent = 0;
        public int NoStudent //Number of Student
        {
            get => _noStudent;
            set
            {
                if (value != _noStudent)
                {
                    _noStudent = value;
                    RaisePropertyChanged(nameof(NoStudent));
                }
            }
        }

        public Class(){}

        public Class(string sClass, Faculty sFaculty)
        {
            SClass = sClass;
            SFaculty = sFaculty;
        }

        public void SetNoStudent()
        {
            NoStudent = classDB.GetNoStudent(SClass);
        }
    }
}
