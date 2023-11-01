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
    internal class AddUpdateStudentViewModel : BindableBase
    {
        #region properties
        private string Mode;
        private StudentDB studentDBConnection { get; set; }
        private FacultyDB facultyDBConnection { get; set; }
        private ClassDB classDBConnection { get; set; }
        public ObservableCollection<Faculty> ListFaculty_cb { get; set; }
        public ObservableCollection<Class> ListClass_cb { get; set; }

        private string _sid_tb;
        public string SID_tb
        {
            get => _sid_tb;
            set
            {
                if (_sid_tb != value)
                {
                    _sid_tb = value;
                    OnPropertyChanged(nameof(SID_tb));
                }
            }
        }

        private string _sname_tb;
        public string SName_tb
        {
            get => _sname_tb;
            set
            {
                if (_sname_tb != value)
                {
                    _sname_tb = value;
                    OnPropertyChanged(nameof(SName_tb));
                }
            }
        }

        private Faculty _faculty_tb;
        public Faculty SelectedFaculty
        {
            get => _faculty_tb;
            set
            {
                if (_faculty_tb != value)
                {
                    _faculty_tb = value;
                    OnPropertyChanged(nameof(SelectedFaculty));
                    LoadClassComboBox();
                }
            }
        }

        private Class _class_tb;
        public Class SelectedClass
        {
            get => _class_tb;
            set
            {
                if (_class_tb != value)
                {
                    _class_tb = value;
                    OnPropertyChanged(nameof(SelectedClass));
                }
            }
        }

        private DateTime _sDOB_dp;
        public DateTime SDOB_dp
        {
            get => _sDOB_dp;
            set
            {
                if (_sDOB_dp != value)
                {
                    _sDOB_dp = value;
                    OnPropertyChanged(nameof(SDOB_dp));
                }
            }
        }

        private string _sAddress_tb;
        public string SAddress_tb
        {
            get => _sAddress_tb;
            set
            {
                if (_sAddress_tb != value)
                {
                    _sAddress_tb = value;
                    OnPropertyChanged(nameof(SAddress_tb));
                }
            }
        }
        private string _sPhone_tb;
        public string SPhone_tb
        {
            get => _sPhone_tb;
            set
            {
                if (_sPhone_tb != value)
                {
                    _sPhone_tb = value;
                    OnPropertyChanged(nameof(SPhone_tb));
                }
            }
        }

        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if (obj is Views.AddUpdateStudentView wd)
            {
                Mode = wd.Tag.ToString();
                string[] parts = Mode.Split('|');
                if (parts.Length == 1 && Mode == "Add")
                {
                    wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                    wd.Show_Button.Visibility = Visibility.Collapsed;
                    SDOB_dp = DateTime.Now.Date;
                }
                else
                {   //Mode == "Update|SID" or "Show|SID" or "Add|SClass"
                    Mode = parts[0];
                    string temp = parts[1]; //SID or SClass

                    if (Mode == "Show")
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Collapsed;
                        wd.Show_Button.Visibility = Visibility.Visible;

                        wd.sID_tb.IsReadOnly = true;
                        wd.sName_tb.IsReadOnly = true;
                        wd.sFaculty_cb.IsEnabled = false;
                        wd.sClass_cb.IsEnabled = false;
                        wd.sDOB_dp.IsEnabled = false;
                        wd.sAddress_tb.IsReadOnly = true;
                        wd.sPhone_tb.IsReadOnly = true;

                        LoadData(temp);
                    }
                    else if (Mode == "Update")//Update
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                        wd.Show_Button.Visibility = Visibility.Collapsed;

                        wd.sID_tb.IsReadOnly = true;

                        LoadData(temp);
                    }
                    else
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                        wd.Show_Button.Visibility = Visibility.Collapsed;
                        SDOB_dp = DateTime.Now.Date;

                        Class classStudent = classDBConnection.GetClass(temp);

                        if (ListFaculty_cb.Count > 0)
                        {
                            SelectedFaculty = ListFaculty_cb.Where(s => s.Fac == classStudent.SFaculty.Fac).ToList().SingleOrDefault();
                        }
                        if (ListClass_cb.Count > 0)
                        {
                            SelectedClass = ListClass_cb.Where(s => s.SClass == classStudent.SClass).ToList().SingleOrDefault();
                        }

                        wd.sFaculty_cb.IsEnabled = false;
                        wd.sClass_cb.IsEnabled = false;
                    }
                }
            }
        }

        public VfxCommand SaveCommand { get; set; }
        private void OnSave(object obj)
        {
            if (obj is Views.AddUpdateStudentView wd)
            {
                if (SID_tb == null || SName_tb == null || SelectedFaculty == null || SelectedClass == null || SDOB_dp == null || SAddress_tb == null || SPhone_tb == null)
                {
                    MessageBox.Show("Please enter all the information", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (Mode == "Add")
                    {
                        if (studentDBConnection.CheckExist(SID_tb))
                        {
                            MessageBox.Show(SID_tb + " is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            SPhone_tb = wd.sPhone_tb.Text;
                            Student item = new Student(SID_tb, SName_tb, SelectedClass, SDOB_dp.Date.ToString("dd-MM-yyyy"), SAddress_tb, SPhone_tb);

                            if (studentDBConnection.CreateStudent(item))
                            {
                                MessageBox.Show("Save Succseccful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                wd.Tag = "Save";
                                wd.Close();
                            }
                            else
                            {
                                MessageBox.Show("Can't Create Student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else //Mode == Update
                    {
                        if (studentDBConnection.CheckExist(SID_tb))
                        {
                            Student item = new Student(SID_tb, SName_tb, SelectedClass, SDOB_dp.Date.ToString("dd-MM-yyyy"), SAddress_tb, SPhone_tb);

                            if (studentDBConnection.UpdateStudent(item))
                            {
                                MessageBox.Show("Save Succseccful", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                                wd.Tag = "Save";
                                wd.Close();
                            }
                            else
                            {
                                MessageBox.Show("Can't Save Student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show(SID_tb + " isn't available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (obj is Views.AddUpdateStudentView wd)
            {
                wd.Close();
            }
        }
        #endregion

        public AddUpdateStudentViewModel()
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
            studentDBConnection = new StudentDB();    
            facultyDBConnection = new FacultyDB();
            classDBConnection = new ClassDB();
            ListFaculty_cb = facultyDBConnection.GetAllFac();
            ListClass_cb = classDBConnection.SearchClass("",SelectedFaculty);
        }
        private void LoadData(string SID)
        {
            Student temp = studentDBConnection.GetStudent(SID);

            SID_tb = temp.SID;
            SName_tb = temp.SName;

            if (ListFaculty_cb.Count > 0)
            {
                SelectedFaculty = ListFaculty_cb.Where(s => s.Fac == temp.SClass.SFaculty.Fac).ToList().SingleOrDefault();
            }
            if (ListClass_cb.Count > 0)
            {
                SelectedClass = ListClass_cb.Where(s => s.SClass == temp.SClass.SClass).ToList().SingleOrDefault();
            }

            if (DateTime.TryParseExact(temp.SDOB, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                SDOB_dp = result;
            }
            else Console.WriteLine("Can't convert String to DateTime.");
            SAddress_tb = temp.SAddress;
            SPhone_tb = temp.SPhone;
        }
        private void LoadClassComboBox()
        {
            ListClass_cb = classDBConnection.SearchClass("", SelectedFaculty);
            OnPropertyChanged(nameof(ListClass_cb));
        }
    }
}
