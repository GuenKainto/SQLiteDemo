using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class StudentInClassViewModel : BindableBase
    {
        #region properties
        private StudentDB studentDBConnection;
        private FacultyDB facultyDBConnection;
        private ClassDB classDBConnection;
        public ObservableCollection<Student> ListStudent { get; set; }
        public ObservableCollection<Faculty> ListFaculty_cb { get; set; }
        public ObservableCollection<Class> ListClass_cb { get; set; }

        private Class _classTag;
        public Class ClassTag
        {
            get => _classTag;
            set
            {
                if (_classTag != value)
                {
                    _classTag = value;
                    OnPropertyChanged(nameof(ClassTag));
                }
            }
        }

        private Faculty _searchFaculty;
        public Faculty SearchFaculty_cb
        {
            get => _searchFaculty;
            set
            {
                if (_searchFaculty != value)
                {
                    _searchFaculty = value;
                    OnPropertyChanged(nameof(SearchFaculty_cb));
                    LoadClassComboBox();
                }
            }
        }

        private Class _searchClass;
        public Class SearchClass_cb
        {
            get => _searchClass;
            set
            {
                if (_searchClass != value)
                {
                    _searchClass = value;
                    OnPropertyChanged(nameof(SearchClass_cb));
                }
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
                    OnPropertyChanged(nameof(SelectedStudent));
                    DeleteCommand.RaiseCanExecuteChanged();
                    ShowCommand.RaiseCanExecuteChanged();
                    UpdateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _search_tb;
        public string Search_tb
        {
            get => _search_tb;
            set
            {
                if (_search_tb != value)
                {
                    _search_tb = value;
                    OnPropertyChanged(nameof(Search_tb));
                }
            }
        }

        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if (obj is Views.StudentInClassView wd)
            {
                if (wd.Tag is Class classTag)
                {
                    ClassTag = classTag;
                }
                LoadData();
                wd.faculty_cb.IsEnabled = false;
                wd.class_cb.IsEnabled=false;
            }
        }
        public VfxCommand SearchCommand { get; set; }
        private void OnSearch(object obj)
        {
            if (obj is Views.StudentInClassView)
            {
                ListStudent.Clear();
                ListStudent = studentDBConnection.SearchStudent(Search_tb, SearchFaculty_cb, SearchClass_cb);
                OnPropertyChanged(nameof(ListStudent));
            }
        }

        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            if (obj is Views.StudentInClassView)
            {
                Search_tb = "";
                OnPropertyChanged("ListStudent");
                AddUpdateStudentView addWd = new AddUpdateStudentView();
                addWd.Tag = "Add|"+ClassTag.SClass;
                
                addWd.ShowDialog();
                if (addWd.Tag.ToString() == "Save")
                {
                    LoadData();
                }
            }

        }
        public VfxCommand DeleteCommand { get; set; }
        private void OnDelete(object obj)
        {
            if (obj is Views.StudentInClassView)
            {
                MessageBoxResult rs = MessageBox.Show("Are you sure you want to delete " + SelectedStudent.SID + " " + SelectedStudent.SName, "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    if (studentDBConnection.DeleteStudent(SelectedStudent))
                    {
                        MessageBox.Show("Delete Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Can't Delete Student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool CanDelete()
        {
            return SelectedStudent != null;
        }
        public VfxCommand ShowCommand { get; set; }
        private void OnShow(object obj)
        {
            if (obj is Views.StudentInClassView)
            {
                AddUpdateStudentView addWd = new AddUpdateStudentView();
                addWd.Tag = "Show|" + SelectedStudent.SID;
                addWd.ShowDialog();
            }
        }
        private bool CanShow()
        {
            return SelectedStudent != null;
        }
        public VfxCommand UpdateCommand { get; set; }
        public void OnUpdate(object obj)
        {
            if (obj is Views.StudentInClassView)
            {
                AddUpdateStudentView addWd = new AddUpdateStudentView();
                addWd.Tag = "Update|" + SelectedStudent.SID;
                addWd.ShowDialog();
                if (addWd.Tag.ToString() == "Save") // After Add/update success, set tag = "Save" to reload data
                {
                    LoadData();
                }
            }
        }
        private bool CanUpdate()
        {
            return SelectedStudent != null;
        }
        #endregion

        public StudentInClassViewModel()
        {
            Init_Model();
            Init_Command();
        }

        private void Init_Model()
        {
            facultyDBConnection = new FacultyDB();
            classDBConnection = new ClassDB();
            studentDBConnection = new StudentDB();
            ListStudent = new ObservableCollection<Student>();
        }

        private void Init_Command()
        {
            LoadedCommand = new VfxCommand(OnLoaded, () => true);
            SearchCommand = new VfxCommand(OnSearch, () => true);
            AddCommand = new VfxCommand(OnAdd, () => true);
            DeleteCommand = new VfxCommand(OnDelete, CanDelete);
            ShowCommand = new VfxCommand(OnShow, CanShow);
            UpdateCommand = new VfxCommand(OnUpdate, CanUpdate);
        }

        private void LoadData()
        {
            ListFaculty_cb = facultyDBConnection.GetAllFac();
            OnPropertyChanged(nameof(ListFaculty_cb));

            if (ListFaculty_cb.Count > 0)
            {
                SearchFaculty_cb = ListFaculty_cb.Where(s => s.Fac == ClassTag.SFaculty.Fac).ToList().SingleOrDefault();
            }

            if (ListClass_cb.Count > 0)
            {
                SearchClass_cb = ListClass_cb.Where(s => s.SClass == ClassTag.SClass).ToList().SingleOrDefault();
            }

            ListStudent.Clear();
            ListStudent = studentDBConnection.SearchStudent("",SearchFaculty_cb,SearchClass_cb);
            OnPropertyChanged(nameof(ListStudent));
        }

        private void LoadClassComboBox()
        {
            ListClass_cb = classDBConnection.SearchClass("", SearchFaculty_cb);
            OnPropertyChanged(nameof(ListClass_cb));
        }
    }
}
