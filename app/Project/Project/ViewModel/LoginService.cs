using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using Project.View;
using Project.View.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel
{
    public class LoginService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private UserController userController { get; set; }

        private string _Login, _Password;

        public string Login
        {
            get => _Login;
            set
            {
                _Login = value;
                NotifyPropertyChanged();
            }
        }
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;
                NotifyPropertyChanged();
            }
        }

        public Command Logining { get; set; }
        public Command Registration { get; set; }
        public Command Closing { get; set; }
        #endregion

        #region Constructors
        public LoginService()
        {
            Logining = new Command(_Logining);
            Registration = new Command(_Registration);
            Closing = AppSettings.WindowService.Closing;

            userController = new UserController();
        }
        #endregion

        #region Commands

        private void _Logining()
        {
            if (Login == null || Login == "")
            {
                AppSettings.WindowService.ShowErrorMessage("Login is empty");
                return;
            }
            if (Password == null || Password == "")
            {
                AppSettings.WindowService.ShowErrorMessage("Password is empty");
                return;
            }

            Model.Models.User user = userController.Login(Login, Password);
            if(user == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Incorrect login or password!");
                return;
            }

            AppSettings.IsAdmin = userController.IsAdmin(user);
            AppSettings.CurrentUser = user;

            if (AppSettings.IsAdmin)
            {
                AppSettings.WindowService.Push(new AdminWindow());
            }
            else
            {
                AppSettings.WindowService.Push(new View.Users.UserWindow());
            }

            Login = "";
            Password = "";
        }
        private void _Registration()
        {
            AppSettings.WindowService.Push(new Registration());
        }

        #endregion
    }
}
