using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class FacultyManagerViewModel : BindableBase
    {
        #region properties
        private string _faculty;
        public string Faculty
        {
            get => _faculty;
            set
            {
                if(value != _faculty)
                {
                    _faculty = value;
                    OnPropertyChanged(nameof(Faculty));
                }
            }
        }

        private int _noClass;
        public int NoClass
        {
            get => _noClass;
            set
            {
                if(value != _noClass)
                {
                    _noClass = value;
                    OnPropertyChanged(nameof(NoClass));
                }
            }
        }

        private int _noTeacher;
        public int NoTeacher
        {
            get => _noTeacher;
            set
            {
                if( value != _noTeacher)
                {
                    _noTeacher = value;
                    OnPropertyChanged(nameof(NoTeacher));
                }
            }
        }

        private int _noStudent;
        public int NoStudent
        {
            get => _noStudent;
            set
            {
                if( _noStudent != value)
                {
                    _noStudent = value;
                    OnPropertyChanged(nameof(NoStudent));
                }    
            }
        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                if (value != _selectedFaculty)
                {
                    _selectedFaculty = value;
                    OnPropertyChanged(nameof(SelectedFaculty));
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _faculty_tb;
        public string Faculty_tb
        {
            get => _faculty_tb;
            set
            {
                if (_faculty_tb != value)
                {
                    _faculty_tb = value;
                    OnPropertyChanged(nameof(Faculty_tb));
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private FacultyDB facDB;
        public ObservableCollection<Faculty> ListFaculty { get; set; }
        #endregion

        #region command
        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            if (obj is Views.FacultyManagerView)
            {
                if (facDB.CheckExist(Faculty_tb))
                {
                    MessageBox.Show(Faculty_tb + " is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (facDB.CreateFaculty(Faculty_tb))
                    {
                        Faculty_tb = null;
                        loadData();
                    }
                    else
                    {
                        MessageBox.Show("Can't Create Faculty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
            }
        }
        private bool CanAdd()
        {
            return Faculty_tb != null;
        }
        public VfxCommand DeleteCommand { get; set; }
        private void OnDelete(object obj)
        {
            if (obj is Views.FacultyManagerView)
            {
                MessageBoxResult rs = MessageBox.Show("Are you sure you want to delete "+SelectedFaculty.Fac,"Message",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if(rs == MessageBoxResult.Yes)
                {
                    if (SelectedFaculty.NoTeacher > 0 || SelectedFaculty.NoClass > 0 || SelectedFaculty.NoStudent > 0)
                    {
                        MessageBox.Show("There are still students, classes, teachers in the faculty", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (facDB.DeleteFaculty(SelectedFaculty.Fac))
                        {
                            MessageBox.Show("Delete Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            loadData();
                        }
                        else
                        {
                            MessageBox.Show("Can't Delete Faculty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
        private bool CanDelete()
        {
            return SelectedFaculty != null;
        }

        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if(obj is Views.FacultyManagerView)
            {
                loadData();
            }
        }
        #endregion

        public FacultyManagerViewModel()
        {
            Init_Model();
            Init_Command();
        }

        private void Init_Model()
        {
            facDB = new FacultyDB();
            ListFaculty = new ObservableCollection<Faculty>();
        }

        private void Init_Command()
        {
            LoadedCommand = new VfxCommand(OnLoaded, () => true);
            AddCommand = new VfxCommand(OnAdd, CanAdd);
            DeleteCommand = new VfxCommand(OnDelete, CanDelete);
        }

        private void loadData()
        {
            Faculty_tb = "";
            ListFaculty.Clear();
            ListFaculty = facDB.GetAllFac();
            foreach(Faculty item in ListFaculty) // get Number of Student, Teacher, Class in Faculty in database
            {
                item.SetNumber();
            }
            OnPropertyChanged("ListFaculty");
        }
    }
}
