using System.Security;
using System.Windows;
using SQLiteDemo.DAO;
using SQLiteDemo.MVVM.Command;
using SQLiteDemo.MVVM.Models;

namespace SQLiteDemo.MVVM.ViewModels
{
    internal class LoginViewModel : BindableBase
    {
        #region properties
        private LoginDB lg {  get; set; }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if(UserName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                    LoginCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private string _passWord;
        public string PassWord
        {
            get => _passWord;
            set
            {
                if(PassWord != value)
                {
                    _passWord = value;
                    OnPropertyChanged(nameof(PassWord));
                    LoginCommand.RaiseCanExecuteChanged();
                    ResetCommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        #region command
        public VfxCommand LoginCommand { get; set; }
        private void OnLogin(object obj)
        {
            if (obj is Views.LoginView loginWd)
            {
                lg = new LoginDB();
                PassWord = loginWd._password.Password;
                if (PassWord == "") 
                    MessageBox.Show("Please enter password !", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    if (lg.CheckLogin(UserName, PassWord))
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Tag = UserName;
                        loginWd.Close();
                        mainWindow.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Username or password incorrect", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private bool CanLogin()
        {
            return UserName != null;
        }

        public VfxCommand ResetCommand { get; set; }
        private void OnReset(object obj)
        {
            if(obj is Views.LoginView loginWd)
            {
                UserName = "";
                loginWd._password.Password = "";
            }
        }

        public VfxCommand PasswordChangedCommand {  get; set; }
        private void OnPasswordChanged(object obj)
        {
            if(obj is Views.LoginView loginWd)
            {
                if (loginWd._password.Password == "") loginWd._placeholder_tblock.Visibility = Visibility.Visible;
                else loginWd._placeholder_tblock.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        public LoginViewModel() 
        {
            Init_Command();
        }

        private void Init_Command()
        {
            PasswordChangedCommand = new VfxCommand(OnPasswordChanged, () => true);
            LoginCommand = new VfxCommand(OnLogin, CanLogin);
            ResetCommand = new VfxCommand(OnReset, ()=>true);
        }
    }
}
