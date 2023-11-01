using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class ClassManagerViewModel : BindableBase
    {
        #region properties
        private ClassDB ClassDBConnecter = new ClassDB();
        private FacultyDB FacultyDBConnecter = new FacultyDB();
        public ObservableCollection<Class> ListClass { get; set; }
        public ObservableCollection<Faculty> ListFaculty { get; set; }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                if(_selectedClass != value) 
                {
                    _selectedClass = value;
                    OnPropertyChanged(nameof(SelectedClass));
                }
            }
        }

        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                if(_selectedFaculty != value)
                {
                    _selectedFaculty = value;
                    OnPropertyChanged(nameof(SelectedFaculty));
                }
            }
        }

        private string _searchClass;
        public string SearchClass
        {
            get => _searchClass;
            set
            {
                if(_searchClass != value)
                {
                    _searchClass = value;
                    OnPropertyChanged(nameof(SearchClass));
                }
            }
        }

        private Faculty _searchFaculty;
        public Faculty SearchFaculty
        {
            get => _searchFaculty;
            set
            {
                if(_searchFaculty != value)
                {
                    _searchFaculty = value;
                    OnPropertyChanged(nameof(_searchFaculty));
                }
            }
        }

        private string _className;
        public string ClassName
        {
            get => _className;
            set
            {
                if(_className != value)
                {
                    _className = value;
                    OnPropertyChanged(nameof(ClassName));
                }
            }
        }
        #endregion

        #region command
        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            
            if(obj is Views.ClassManagerView classView)
            {   
                if(classView.sclass_tb.Text == null || SelectedFaculty == null)
                {
                    MessageBox.Show("Please enter full information", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Class temp = new Class(classView.sclass_tb.Text , SelectedFaculty);
                    if (ClassDBConnecter.IsExist(temp))
                    {
                        MessageBox.Show("Class " + ClassName + " is already have", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ClassDBConnecter.CreateClass(temp))
                        {
                            MessageBox.Show("Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                        else MessageBox.Show("Can't create Class", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public VfxCommand DeleteCommand { get; set; }
        private void OnDelete(object obj)
        {
            if (obj is Views.ClassManagerView classView)
            {
                if(SelectedClass == null)
                {
                    MessageBox.Show("Please choice the Class in list","Message",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
                else
                {
                    MessageBoxResult rs = MessageBox.Show("Are you sure you want to delete " + SelectedClass.SClass + " in " + SelectedClass.SFaculty.Fac, "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (rs == MessageBoxResult.Yes)
                    {
                        if (SelectedClass.NoStudent != 0)
                        {
                            MessageBox.Show("There are still students in the class, please delete the list of students in the class before deleting the class", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (ClassDBConnecter.DeleteClass(SelectedClass))
                            {
                                MessageBox.Show("Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadData();
                            }
                            else MessageBox.Show("Can't delete this class", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        public VfxCommand SearchCommand { get; set; }
        private void OnSearch(object obj)
        {
            ListClass.Clear();
            ListClass = ClassDBConnecter.SearchClass(SearchClass, SearchFaculty);
            OnPropertyChanged(nameof(ListClass));
        }
        private bool CanSearch()
        {
            return true;
        }

        public VfxCommand ShowCommand { get; set; }
        private void OnShow(object obj)
        {
            if (obj is Views.ClassManagerView classView)
            {
                if (SelectedClass == null)
                {
                    MessageBox.Show("Please choice the Class in list", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    StudentInClassView studentInClassView = new StudentInClassView();
                    studentInClassView.Title ="Class :" + SelectedClass.SClass;
                    studentInClassView.Tag = SelectedClass;
                    studentInClassView.ShowDialog();
                }
            }
        }
        
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if (obj is Views.ClassManagerView classView)
            {
                LoadData();
            }
        }
        
        #endregion

        public ClassManagerViewModel()
        {
            InitModel();
            InitCommand();
        }

        private void InitCommand()
        {
            AddCommand = new VfxCommand(OnAdd, () => true);
            DeleteCommand = new VfxCommand(OnDelete, () => true);
            ShowCommand = new VfxCommand(OnShow, () => true);
            LoadedCommand = new VfxCommand(OnLoaded, () => true);
            SearchCommand = new VfxCommand(OnSearch, CanSearch);
        }

        private void InitModel()
        {
            ListClass = new ObservableCollection<Class>();
            ListFaculty = new ObservableCollection<Faculty>();
            ListFaculty = FacultyDBConnecter.GetAllFac();
        }

        private void LoadData()
        {
            ClassName = null;
            SelectedFaculty = null;
            ListClass.Clear();
            ListClass = ClassDBConnecter.GetAllClass();
            foreach (Class item in ListClass)
            {
                item.SetNoStudent();
            }
            OnPropertyChanged(nameof(ListClass));

        }
    }
}
