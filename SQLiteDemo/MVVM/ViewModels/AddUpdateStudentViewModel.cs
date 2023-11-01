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
        public ObservableCollection<Faculty> ListFaculty { get; set; }
        public ObservableCollection<Class> ListClass { get; set; }

        private string _studentId;
        public string StudentId //tb : TextBox
        {
            get => _studentId;
            set
            {
                if (_studentId != value)
                {
                    _studentId = value;
                    OnPropertyChanged(nameof(StudentId));
                }
            }
        }

        private string _studentName;
        public string StudentName
        {
            get => _studentName;
            set
            {
                if (_studentName != value)
                {
                    _studentName = value;
                    OnPropertyChanged(nameof(StudentName));
                }
            }
        }

        private Faculty _selectedDaculty;
        public Faculty SelectedFaculty
        {
            get => _selectedDaculty;
            set
            {
                if (_selectedDaculty != value)
                {
                    _selectedDaculty = value;
                    OnPropertyChanged(nameof(SelectedFaculty));
                    LoadClassComboBox();
                }
            }
        }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                if (_selectedClass != value)
                {
                    _selectedClass = value;
                    OnPropertyChanged(nameof(SelectedClass));
                }
            }
        }

        private DateTime _studentDOB;
        public DateTime StudentDOB //Student Date of Birth
        {
            get => _studentDOB;
            set
            {
                if (_studentDOB != value)
                {
                    _studentDOB = value;
                    OnPropertyChanged(nameof(StudentDOB));
                }
            }
        }

        private string _studentAddress;
        public string StudentAddress
        {
            get => _studentAddress;
            set
            {
                if (_studentAddress != value)
                {
                    _studentAddress = value;
                    OnPropertyChanged(nameof(StudentAddress));
                }
            }
        }
        private string _studentPhone;
        public string StudentPhone
        {
            get => _studentPhone;
            set
            {
                if (_studentPhone != value)
                {
                    _studentPhone = value;
                    OnPropertyChanged(nameof(StudentPhone));
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
                    StudentDOB = DateTime.Now.Date;
                }
                else
                {   //Mode == "Update|SID" or "Show|SID" or "Add|SClass"
                    Mode = parts[0];
                    string temp = parts[1]; //SID or SClass

                    if (Mode == "Show")
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Collapsed;
                        wd.Show_Button.Visibility = Visibility.Visible;

                        wd.StudentIdTxb.IsReadOnly = true;
                        wd.StudentNameTxb.IsReadOnly = true;
                        wd.StudentFacultyCmb.IsEnabled = false;
                        wd.StudentClassCmb.IsEnabled = false;
                        wd.StudentDOBDatePicker.IsEnabled = false;
                        wd.StudentAddressTxb.IsReadOnly = true;
                        wd.StudentPhoneTxb.IsReadOnly = true;

                        LoadData(temp);
                    }
                    else if (Mode == "Update")//Update
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                        wd.Show_Button.Visibility = Visibility.Collapsed;

                        wd.StudentIdTxb.IsReadOnly = true;

                        LoadData(temp);
                    }
                    else
                    {
                        wd.Add_Update_Buttones.Visibility = Visibility.Visible;
                        wd.Show_Button.Visibility = Visibility.Collapsed;
                        StudentDOB = DateTime.Now.Date;

                        Class classStudent = classDBConnection.GetClass(temp);

                        if (ListFaculty.Count > 0)
                        {
                            SelectedFaculty = ListFaculty.Where(s => s.Fac == classStudent.SFaculty.Fac).ToList().SingleOrDefault();
                        }
                        if (ListClass.Count > 0)
                        {
                            SelectedClass = ListClass.Where(s => s.SClass == classStudent.SClass).ToList().SingleOrDefault();
                        }

                        wd.StudentFacultyCmb.IsEnabled = false;
                        wd.StudentClassCmb.IsEnabled = false;
                    }
                }
            }
        }

        public VfxCommand SaveCommand { get; set; }
        private void OnSave(object obj)
        {
            if (obj is Views.AddUpdateStudentView wd)
            {
                if (StudentId == null || StudentName == null || SelectedFaculty == null || SelectedClass == null || StudentDOB == null || StudentAddress == null || StudentPhone == null)
                {
                    MessageBox.Show("Please enter all the information", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (Mode == "Add")
                    {
                        if (studentDBConnection.IsExist(StudentId))
                        {
                            MessageBox.Show($"{StudentId} is already available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            StudentPhone = wd.StudentPhoneTxb.Text;
                            Student item = new Student( sID: StudentId,sName: StudentName, sClass: SelectedClass, sDOB: StudentDOB.Date.ToString("dd-MM-yyyy"), sAddress: StudentAddress, sPhone: StudentPhone);

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
                        if (studentDBConnection.IsExist(StudentId))
                        {
                            Student item = new Student(sID: StudentId, sName: StudentName, sClass: SelectedClass, sDOB: StudentDOB.Date.ToString("dd-MM-yyyy"), sAddress: StudentAddress, sPhone: StudentPhone);

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
                            MessageBox.Show($"{StudentId} isn't available on the database", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
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
            SaveCommand = new VfxCommand(OnSave, () => true);
            CloseWindowCommand = new VfxCommand(OnCloseWindow, () => true);
        }

        private void Init_Model()
        {
            studentDBConnection = new StudentDB();    
            facultyDBConnection = new FacultyDB();
            classDBConnection = new ClassDB();
            ListFaculty = facultyDBConnection.GetAllFac();
            ListClass = classDBConnection.SearchClass("",SelectedFaculty);
        }
        private void LoadData(string SID)
        {
            Student temp = studentDBConnection.GetStudent(SID);

            StudentId = temp.SID;
            StudentName = temp.SName;

            if (ListFaculty.Count > 0)
            {
                SelectedFaculty = ListFaculty.Where(s => s.Fac == temp.SClass.SFaculty.Fac).ToList().SingleOrDefault();
            }
            if (ListClass.Count > 0)
            {
                SelectedClass = ListClass.Where(s => s.SClass == temp.SClass.SClass).ToList().SingleOrDefault();
            }

            if (DateTime.TryParseExact(temp.SDOB, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                StudentDOB = result;
            }
            else Console.WriteLine("Can't convert String to DateTime.");
            StudentAddress = temp.SAddress;
            StudentPhone = temp.SPhone;
        }
        private void LoadClassComboBox()
        {
            ListClass = classDBConnection.SearchClass("", SelectedFaculty);
            OnPropertyChanged(nameof(ListClass));
        }
    }
}
