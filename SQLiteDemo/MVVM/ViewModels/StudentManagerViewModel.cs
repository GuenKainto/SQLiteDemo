using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class StudentManagerViewModel : BindableBase
    {
        #region properties
        private StudentDB studentDBConnection;
        private FacultyDB facultyDBConnection;
        private ClassDB classDBConnection;
        public ObservableCollection<Student> ListStudent { get; set; }
        public ObservableCollection<Faculty> ListFaculty_cb { get; set; }
        public ObservableCollection<Class> ListClass_cb { get; set; }

        private Faculty _searchFaculty;
        public Faculty SearchFaculty_cb
        {
            get => _searchFaculty;
            set
            {
                if(_searchFaculty != value)
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
                ListStudent = studentDBConnection.SearchStudent(Search_tb,SearchFaculty_cb,SearchClass_cb);
                OnPropertyChanged(nameof(ListStudent));
            }
        }

        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            if (obj is Views.StudentManagerView)
            {
                Search_tb = "";
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
            ListFaculty_cb = facultyDBConnection.GetAllFac();
            OnPropertyChanged(nameof(ListFaculty_cb));
            ListStudent.Clear();
            ListStudent = studentDBConnection.GetAllStudent();
            OnPropertyChanged(nameof(ListStudent));
        }

        private void LoadClassComboBox()
        {
            ListClass_cb = classDBConnection.SearchClass("", SearchFaculty_cb);
            OnPropertyChanged(nameof(ListClass_cb));
        }
    }
}
