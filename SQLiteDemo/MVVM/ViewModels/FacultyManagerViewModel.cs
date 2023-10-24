using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

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
                }
            }
        }

        private FacultyDB facDB = new FacultyDB();
        public ObservableCollection<Faculty> ListFaculty = new ObservableCollection<Faculty>();
        #endregion

        #region command
        public VfxCommand AddCommand { get; set; }
        private void OnAdd(object obj)
        {
            MessageBox.Show(Faculty_tb);
            if (obj is Views.FacultyManagerView)
            {
                MessageBox.Show(Faculty_tb);
                if (facDB.CreateFaculty(Faculty_tb))
                {
                    MessageBox.Show("Create Successful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Can't Create Faculty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private bool CanAdd()
        {
            return true;
        }
        public VfxCommand DeleteCommand { get; set; }
        private void OnDelete(object obj)
        {
            
        }
        private bool CanDelete()
        {
            return true;
        }

        #endregion

        public FacultyManagerViewModel()
        {
            Init_Command();
        }

        private void Init_Command()
        {
            AddCommand = new VfxCommand(OnAdd, CanAdd);
            DeleteCommand = new VfxCommand(OnDelete, CanDelete);
        }
    }
}
