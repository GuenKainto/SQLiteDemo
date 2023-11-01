using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class StudentManagerViewModel : BindableBase
    {
        #region properties
        private StudentDB studentDBConnection;
        private FacultyDB facultyDBConnection;
        private ClassDB classDBConnection;
        public ObservableCollection<Student> ListStudent { get; set; }
        public ObservableCollection<Faculty> ListFaculty { get; set; }
        public ObservableCollection<Class> ListClass { get; set; }

        private Faculty _searchFaculty;
        public Faculty SearchFaculty
        {
            get => _searchFaculty;
            set
            {
                if(_searchFaculty != value)
                {
                    _searchFaculty = value;
                    OnPropertyChanged(nameof(SearchFaculty));
                    LoadClassComboBox();
                }
            }
        }

        private Class _searchClass;
        public Class SearchClass
        {
            get => _searchClass;
            set
            {
                if (_searchClass != value)
                {
                    _searchClass = value;
                    OnPropertyChanged(nameof(SearchClass));
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

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                if (_search != value)
                {
                    _search = value;
                    OnPropertyChanged(nameof(Search));
                }
            }
        }

        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if(obj is Views.StudentManagerView)
            {
                LoadData();
            }
        }
        public VfxCommand SearchCommand { get; set; }
        private void OnSearch(object obj)
        {
            if (obj  is Views.StudentManagerView)
            {
                ListStudent.Clear();
                ListStudent = studentDBConnection.SearchStudent(searchText: Search,searchFaculty: SearchFaculty, searchClass: SearchClass);
                OnPropertyChanged(nameof(ListStudent));
            }
        }

        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            if (obj is Views.StudentManagerView)
            {
                Search = "";
                OnPropertyChanged("ListStudent");
                AddUpdateStudentView addWd = new AddUpdateStudentView();
                addWd.Tag = "Add";
                addWd.ShowDialog();
                if (addWd.Tag.ToString() == "Save")
                {
                    LoadData();
                }
            }

        }
        public VfxCommand DeleteCommand {  get; set; }
        private void OnDelete(object obj)
        {
            if (obj is Views.StudentManagerView)
            {
                MessageBoxResult rs = MessageBox.Show($"Are you sure you want to delete {SelectedStudent.SID}-{SelectedStudent.SName}", "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
            if (obj is Views.StudentManagerView)
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
        public VfxCommand UpdateCommand {  get; set; }
        public void OnUpdate(object obj)
        {
            if (obj is Views.StudentManagerView)
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

        public StudentManagerViewModel()
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
            ListFaculty = facultyDBConnection.GetAllFac();
            OnPropertyChanged(nameof(ListFaculty));
            ListStudent.Clear();
            ListStudent = studentDBConnection.GetAllStudent();
            OnPropertyChanged(nameof(ListStudent));
        }

        private void LoadClassComboBox()
        {
            ListClass = classDBConnection.SearchClass("", SearchFaculty);
            OnPropertyChanged(nameof(ListClass));
        }
    }
}
