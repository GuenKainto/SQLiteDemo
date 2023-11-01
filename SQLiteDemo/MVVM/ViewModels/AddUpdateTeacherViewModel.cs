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
        public ObservableCollection<Faculty> ListFaculty { get; set; }
        private TeacherDB teacherDBConnecter;
        private FacultyDB facultyDBConnecter;
        
        private string _teacherID;
        public string TeacherID
        {
            get => _teacherID;
            set
            {
                if (_teacherID != value)
                {
                    _teacherID = value;
                    OnPropertyChanged(nameof(TeacherID));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _teacherName;
        public string TeacherName
        {
            get => _teacherName;
            set
            {
                if (_teacherName != value)
                {
                    _teacherName = value;
                    OnPropertyChanged(nameof(TeacherName));
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
        private DateTime _teacherDOB;
        public DateTime TeacherDOB
        {
            get => _teacherDOB;
            set
            {
                if (_teacherDOB != value)
                {
                    _teacherDOB = value;
                    OnPropertyChanged(nameof(TeacherDOB));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _teacherAddress;
        public string TeacherAddress
        {
            get => _teacherAddress;
            set
            {
                if (_teacherAddress != value)
                {
                    _teacherAddress = value;
                    OnPropertyChanged(nameof(TeacherAddress));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _teacherPhone;
        public string TeacherPhone
        {
            get => _teacherPhone;
            set
            {
                if (_teacherPhone != value)
                {
                    _teacherPhone = value;
                    OnPropertyChanged(nameof(TeacherPhone));
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
                    TeacherDOB = DateTime.Now.Date;
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

                            wd.TeacherIdTxb.IsReadOnly = true;
                            wd.TeacherNameTxb.IsReadOnly = true;
                            wd.TeacherFacultyCmb.IsEnabled = true;
                            wd.TeacherDOBDataPicker.IsEnabled = false;
                            wd.TeacherAddressTxb.IsReadOnly = true;
                            wd.TeacherPhoneTxb.IsReadOnly = true;

                            LoadData(sTID);
                        }
                        else //Update
                        {
                            wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                            wd.Show_Button.Visibility = Visibility.Collapsed;

                            wd.TeacherIdTxb.IsReadOnly = true;

                            LoadData(sTID);
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
                if (TeacherID == null || TeacherName == null || SelectedFaculty == null || TeacherDOB == null || TeacherAddress == null || TeacherPhone == null)
                {
                    MessageBox.Show("Please enter all the information","Message",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    if (Mode == "Add")
                    {
                        if (teacherDBConnecter.IsExist(TeacherID))
                        {
                            MessageBox.Show(TeacherID + " is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            TeacherPhone = wd.TeacherPhoneTxb.Text;
                            Teacher item = new Teacher(tID: TeacherID,tName: TeacherName,tFaculty: SelectedFaculty,tDOB: TeacherDOB.Date.ToString("dd-MM-yyyy"),tAddress: TeacherAddress,tPhone: TeacherPhone);

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
                        if (teacherDBConnecter.IsExist(TeacherID))
                        {
                            Teacher item = new Teacher(tID: TeacherID, tName: TeacherName, tFaculty: SelectedFaculty, tDOB: TeacherDOB.Date.ToString("dd-MM-yyyy"), tAddress: TeacherAddress, tPhone: TeacherPhone);

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
                            MessageBox.Show(TeacherID + " isn't available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
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
            SaveCommand = new VfxCommand(OnSave, () => true);
            CloseWindowCommand = new VfxCommand(OnCloseWindow, () => true);
        }

        private void Init_Model()
        {
            teacherDBConnecter = new TeacherDB();
            facultyDBConnecter = new FacultyDB();
            ListFaculty = facultyDBConnecter.GetAllFac();
        }

        private void LoadData(string sTID)
        {
            Teacher temp = teacherDBConnecter.GetTeacher(sTID);

            TeacherID = temp.TID;
            TeacherName = temp.TName;

            if(ListFaculty.Count > 0)
            {
                SelectedFaculty = ListFaculty.Where(s=>s.Fac == temp.TFaculty.Fac).ToList().SingleOrDefault();
            }

            if (DateTime.TryParseExact(temp.TDOB, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                TeacherDOB = result;
            }
            else Console.WriteLine("Can't convert String to DateTime.");
            TeacherAddress = temp.TAddress;
            TeacherPhone = temp.TPhone;
        }
    }
}
