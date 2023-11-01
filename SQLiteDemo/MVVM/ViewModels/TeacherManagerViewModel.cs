using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class TeacherManagerViewModel : BindableBase
    {
        #region properties
        private TeacherDB teacherDBConnecter;
        public ObservableCollection<Teacher> ListTeacher { get; set; }
        
        private string _search_tb;

        public string Search_tb
        {
            get => _search_tb; 
            set 
            {
                if(_search_tb != value)
                {
                    _search_tb = value;
                    OnPropertyChanged(nameof(Search_tb));
                    SearchCommand.RaiseCanExecuteChanged(); //BUG
                } 
            }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                if( _selectedTeacher != value)
                {
                    _selectedTeacher = value;
                    OnPropertyChanged(nameof(SelectedTeacher));
                    DeleteCommand.RaiseCanExecuteChanged();
                    ShowCommand.RaiseCanExecuteChanged();
                    UpdateCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoad(Object obj)
        {
            if (obj is Views.TeacherManagerView)
            {
                LoadData();
            }
        }

        public VfxCommand SearchCommand { get; set; }
        private void OnSearch(object obj)
        {
            if (obj is Views.TeacherManagerView wd)
            {
                Search_tb = wd._search_tb.Text;
                ListTeacher.Clear();
                ListTeacher = teacherDBConnecter.SearchTeacher(Search_tb);
                OnPropertyChanged("ListTeacher");
            }
        }

        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            if (obj is Views.TeacherManagerView)
            {
                Search_tb = "";
                OnPropertyChanged("ListTeacher");
                AddUpdateTeacherView addWd = new AddUpdateTeacherView();
                addWd.Tag = "Add";
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
            if (obj is Views.TeacherManagerView)
            {
                MessageBoxResult rs = MessageBox.Show("Are you sure you want to delete " + SelectedTeacher.TID + " " + SelectedTeacher.TName, "Message", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    if (teacherDBConnecter.DeleteTeacher(SelectedTeacher))
                    {
                        MessageBox.Show("Delete Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Can't Delete Teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private bool CanDelete()
        {
            return SelectedTeacher != null;
        }

        public VfxCommand ShowCommand { get; set; }
        private void OnShow(object obj)
        {
            if (obj is Views.TeacherManagerView)
            {
                AddUpdateTeacherView addWd = new AddUpdateTeacherView();
                addWd.Tag = "Show|" + SelectedTeacher.TID;
                addWd.ShowDialog();
            }
        }
        private bool CanShow()
        {
            return SelectedTeacher != null;
        }

        public VfxCommand UpdateCommand { get; set; }
        private void OnUpdate(object obj)
        {
            if (obj is Views.TeacherManagerView)
            {
                AddUpdateTeacherView addWd = new AddUpdateTeacherView();
                addWd.Tag = "Update|" + SelectedTeacher.TID;
                addWd.ShowDialog();
                if (addWd.Tag.ToString() == "Save") // After Add/update success, set tag = "Save" to reload data
                {
                    LoadData();
                }
            }
        }
        private bool CanUpdate()
        {
            return SelectedTeacher != null;
        }
        #endregion

        public TeacherManagerViewModel() 
        {
            Init_Model();
            Init_Command();
        }

        private void Init_Model()
        {
            teacherDBConnecter = new TeacherDB();
            ListTeacher = new ObservableCollection<Teacher>();
        }

        private void Init_Command()
        {
            LoadedCommand = new VfxCommand(OnLoad, () => true);
            SearchCommand = new VfxCommand(OnSearch, () => true);
            AddCommand = new VfxCommand(OnAdd, ()=> true);
            DeleteCommand = new VfxCommand(OnDelete,CanDelete);
            ShowCommand = new VfxCommand (OnShow, CanShow);
            UpdateCommand = new VfxCommand(OnUpdate, CanUpdate);
        }

        private void LoadData()
        {
            ListTeacher.Clear();
            ListTeacher = teacherDBConnecter.GetAllTeacher();
            OnPropertyChanged("ListTeacher");
        }
    }
}
