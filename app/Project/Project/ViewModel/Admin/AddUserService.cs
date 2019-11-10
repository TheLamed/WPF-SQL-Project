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

namespace Project.ViewModel.Admin
{
    class AddUserService : INotifyPropertyChanged
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

        private bool _IsAdmin;
        public bool IsAdmin
        {
            get => _IsAdmin;
            set
            {
                _IsAdmin = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUpdate { get; set; }
        public string ButtonContent => isUpdate ? "Update" : "Add";
        private int UserID { get; set; }
        private string startLogin { get; set; }

        public Command AddButton { get; set; }
        #endregion

        #region Constructors
        public AddUserService()
        {
            usersController = new UsersController();

            isUpdate = false;

            IsAdmin = false;

            AddButton = new Command(_AddButton);
        }
        public AddUserService(User user) : this()
        {
            if (user == null || user.ID == null) return;

            isUpdate = true;
            UserID = user.ID ?? 0;

            IsAdmin = usersController.IsAdmin(user);
            Login = user.Login;
            startLogin = user.Login;
            Password = user.Password;
            ConfirmPassword = user.Password;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.Email;
        }
        #endregion

        #region Commands

        public void _AddButton()
        {
            if (Login == null || Login == "")
            {
                AppSettings.WindowService.ShowErrorMessage("Login is empty!");
                return;
            }
            if (Password == null || Password == "")
            {
                AppSettings.WindowService.ShowErrorMessage("Password is empty!");
                return;
            }
            if (Login != startLogin && usersController.IsLoginUsed(Login))
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

            User user = new User(Login, Password, Name, Surname, Email, UserID);

            if (isUpdate)
            {
                usersController.UpdateUser(user, IsAdmin);
                AppSettings.WindowService.ShowOkMessage("User updated!");
            }
            else
            {
                usersController.AddUser(user, IsAdmin);
                AppSettings.WindowService.ShowOkMessage("User added!");
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
