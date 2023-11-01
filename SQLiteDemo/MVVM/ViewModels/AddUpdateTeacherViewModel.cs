using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class AddUpdateTeacherViewModel : BindableBase
    {
        #region properties
        private string Mode;
        public ObservableCollection<Faculty> ListFaculty_cb { get; set; }
        private TeacherDB teacherDBConnecter;
        private FacultyDB facultyDBConnecter;
        private string _tID_tb;
        public string TID_tb
        {
            get => _tID_tb;
            set
            {
                if (_tID_tb != value)
                {
                    _tID_tb = value;
                    OnPropertyChanged(nameof(TID_tb));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _tName_tb;
        public string TName_tb
        {
            get => _tName_tb;
            set
            {
                if (_tName_tb != value)
                {
                    _tName_tb = value;
                    OnPropertyChanged(nameof(TName_tb));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private Faculty _selectedFaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedFaculty;
            set
            {
                if (_selectedFaculty != value)
                {
                    _selectedFaculty = value;
                    OnPropertyChanged(nameof(SelectedFaculty));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private DateTime _tDOB_dp;
        public DateTime TDOB_dp
        {
            get => _tDOB_dp;
            set
            {
                if (_tDOB_dp != value)
                {
                    _tDOB_dp  = value;
                    OnPropertyChanged(nameof(TDOB_dp));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _tAddress_tb;
        public string TAddress_tb
        {
            get => _tAddress_tb;
            set
            {
                if (_tAddress_tb != value)
                {
                    _tAddress_tb = value;
                    OnPropertyChanged(nameof(TAddress_tb));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _tPhone_tb;
        public string TPhone_tb
        {
            get => _tPhone_tb;
            set
            {
                if (_tPhone_tb != value)
                {
                    _tPhone_tb = value;
                    OnPropertyChanged(nameof(TPhone_tb));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if (obj is Views.AddUpdateTeacherView wd)
            {
                Mode = wd.Tag.ToString(); 
                if(Mode == "Add")
                {
                    wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                    wd.Show_Button.Visibility = Visibility.Collapsed;
                    TDOB_dp = DateTime.Now.Date;
                }
                else
                {   //Mode == "Update|TID" or "Show|TID"
                    string[] parts = Mode.Split('|');

                    if (parts.Length == 2)
                    {
                        Mode = parts[0]; // "Update"
                        string sTID = parts[1]; // TID

                        if(Mode == "Show")
                        {
                            wd.Add_Update_Buttones.Visibility = Visibility.Collapsed;
                            wd.Show_Button.Visibility = Visibility.Visible;

                            wd.tID_tb.IsReadOnly = true;
                            wd.tName_tb.IsReadOnly = true;
                            wd.tFaculty_cb.IsEnabled = true;
                            wd.tDOB_dp.IsEnabled = false;
                            wd.tAddress_tb.IsReadOnly = true;
                            wd.tPhone_tb.IsReadOnly = true;

                            loadData(sTID);
                        }
                        else //Update
                        {
                            wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                            wd.Show_Button.Visibility = Visibility.Collapsed;

                            wd.tID_tb.IsReadOnly = true;

                            loadData(sTID);
                        }

                    }
                }
            }
        }

        public VfxCommand SaveCommand { get; set; }
        private void OnSave(object obj)
        {
            if (obj is Views.AddUpdateTeacherView wd)
            {
                if (TID_tb == null || TName_tb == null || SelectedFaculty == null || TDOB_dp == null || TAddress_tb == null || TPhone_tb == null)
                {
                    MessageBox.Show("Please enter all the information","Message",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    if (Mode == "Add")
                    {
                        if (teacherDBConnecter.IsExist(TID_tb))
                        {
                            MessageBox.Show(TID_tb + " is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            TPhone_tb = wd.tPhone_tb.Text;
                            Teacher item = new Teacher(TID_tb, TName_tb, SelectedFaculty, TDOB_dp.Date.ToString("dd-MM-yyyy"), TAddress_tb, TPhone_tb);

                            if (teacherDBConnecter.CreateTeacher(item))
                            {
                                MessageBox.Show("Save Succseccful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                wd.Tag = "Save";
                                wd.Close();
                            }
                            else
                            {
                                MessageBox.Show("Can't Create Teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else //Mode == Update
                    {
                        if (teacherDBConnecter.IsExist(TID_tb))
                        {
                            Teacher item = new Teacher(TID_tb, TName_tb, SelectedFaculty, TDOB_dp.Date.ToString("dd-MM-yyyy"), TAddress_tb, TPhone_tb);

                            if (teacherDBConnecter.UpdateTeacher(item))
                            {
                                MessageBox.Show("Save Succseccful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                wd.Tag = "Save";
                                wd.Close();
                            }
                            else
                            {
                                MessageBox.Show("Can't Save Teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(TID_tb + " isn't available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }
        private bool CanSave()
        {
            return true; //(TID_tb != null && TName_tb != null && SelectedFaculty != null && TDOB_dp != null && TAddress_tb != null && TPhone_tb != null) ;
        }

        public VfxCommand CloseWindowCommand { get; set; }
        private void OnCloseWindow(object obj)
        {
            if (obj is Views.AddUpdateTeacherView wd)
            {
                wd.Close();
            }
        }

        #endregion

        public AddUpdateTeacherViewModel()
        {
            Init_Model();
            Init_Command();
        }

        private void Init_Command()
        {
            LoadedCommand = new VfxCommand(OnLoaded, () => true);
            SaveCommand = new VfxCommand(OnSave, CanSave);
            CloseWindowCommand = new VfxCommand(OnCloseWindow, () => true);
        }

        private void Init_Model()
        {
            teacherDBConnecter = new TeacherDB();
            facultyDBConnecter = new FacultyDB();
            ListFaculty_cb = facultyDBConnecter.GetAllFac();
        }

        private void loadData(string sTID)
        {
            Teacher temp = teacherDBConnecter.GetTeacher(sTID);

            TID_tb = temp.TID;
            TName_tb = temp.TName;

            if(ListFaculty_cb.Count > 0)
            {
                SelectedFaculty = ListFaculty_cb.Where(s=>s.Fac == temp.TFaculty.Fac).ToList().SingleOrDefault();
            }

            if (DateTime.TryParseExact(temp.TDOB, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                TDOB_dp = result;
            }
            else Console.WriteLine("Can't convert String to DateTime.");
            TAddress_tb = temp.TAddress;
            TPhone_tb = temp.TPhone;
        }
    }
}
