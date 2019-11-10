using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.ViewModel.Users
{
    public class EditUserService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private UsersController usersController { get; set; }

        private string _Login, _Password, _ConfirmPassword, _Name, _Surname, _Email;
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
        public string ConfirmPassword
        {
            get => _ConfirmPassword;
            set
            {
                _ConfirmPassword = value;
                NotifyPropertyChanged();
            }
        }
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                NotifyPropertyChanged();
            }
        }
        public string Surname
        {
            get => _Surname;
            set
            {
                _Surname = value;
                NotifyPropertyChanged();
            }
        }
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                NotifyPropertyChanged();
            }
        }

        public Command Save { get; set; }
        #endregion

        #region Constructors
        public EditUserService()
        {
            usersController = new UsersController();

            Login = AppSettings.CurrentUser.Login;
            Name = AppSettings.CurrentUser.Name;
            Surname = AppSettings.CurrentUser.Surname;
            Email = AppSettings.CurrentUser.Email;

            Save = new Command(_Save);
        }
        #endregion

        #region Commands

        private void _Save()
        {
            if (Login == null || Login == "")
            {
                AppSettings.WindowService.ShowErrorMessage("Login is empty!");
                return;
            }
            if (Password == null || Password == "")
            {
                Password = AppSettings.CurrentUser.Password;
                ConfirmPassword = AppSettings.CurrentUser.Password;
            }
            if (Login != AppSettings.CurrentUser.Login && usersController.IsLoginUsed(Login))
            {
                AppSettings.WindowService.ShowErrorMessage("This Login alredy used!");
                return;
            }
            if (ConfirmPassword == null || ConfirmPassword == "" || Password != ConfirmPassword)
            {
                AppSettings.WindowService.ShowErrorMessage("Passwords are different!");
                return;
            }
            if (Email != null && Email != "" && !new Regex(@"[A-Za-z0-9_]+@[A-Za-z0-9_]+\.[A-Za-z0-9_]+").IsMatch(Email))
            {
                AppSettings.WindowService.ShowErrorMessage("Email is not valid!");
                return;
            }

            User user = new User(Login, Password, Name, Surname, Email, AppSettings.CurrentUser.ID);
            usersController.UpdateUser(user, AppSettings.IsAdmin);
            AppSettings.WindowService.ShowOkMessage("Changes saved!");
            AppSettings.CurrentUser = user;
        }

        #endregion

        #region Methods

        #endregion
    }
}
