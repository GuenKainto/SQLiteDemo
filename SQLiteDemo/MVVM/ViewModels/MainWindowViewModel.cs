using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;
using SQLiteDemo.MVVM.Views;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        #region properties
        private string _username;
        public string UserName
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }
        private BindableBase _CurrentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _CurrentViewModel;
            set { SetProperty(ref _CurrentViewModel, value); }
        }

        private FacultyManagerViewModel _facultyManagerViewModel = new FacultyManagerViewModel();
        private ClassManagerViewModel _classManagerViewModel = new ClassManagerViewModel();
        private TeacherManagerViewModel _teacherManagerViewModel = new TeacherManagerViewModel();
        private StudentManagerViewModel _studentManagerViewModel = new StudentManagerViewModel();
        #endregion

        #region command
        public VfxCommand LoadedCommand { get; set; }
        private void OnLoaded(object obj)
        {
            if(obj is MainWindow wd)
            {
                UserName = wd.Tag.ToString();
            }
        }

        public VfxCommand NavigateCommand { get; set; }
        private void OnNavigate(object obj)
        {
            switch (obj)
            {
                case "Faculty":
                    CurrentViewModel = _facultyManagerViewModel;
                    break;
                case "Class":
                    CurrentViewModel = _classManagerViewModel;
                    break;
                case "Teacher":
                    CurrentViewModel = _teacherManagerViewModel;
                    break;
                default:
                    CurrentViewModel = _studentManagerViewModel;
                    break;
            }
        }
        public VfxCommand LogOutCommand { get; set; }
        private void OnLogOut (object obj)
        {
            if(obj is MainWindow wd)
            {
                LoginView loginView = new LoginView();
                loginView.ShowDialog();
                wd.Close();
            }
        }
        #endregion

        public MainWindowViewModel()
        {
            Init_Command();
            Init_Model();
        }

        private void Init_Model()
        {
            CurrentViewModel = _studentManagerViewModel;
        }

        private void Init_Command()
        {
            LoadedCommand = new VfxCommand(OnLoaded, () => true);
            NavigateCommand = new VfxCommand(OnNavigate, () => true);
            LogOutCommand = new VfxCommand (OnLogOut, () => true);
        }
    }
}
