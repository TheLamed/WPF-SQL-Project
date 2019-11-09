using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using Project.View.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Admin
{
    public class UsersService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private UsersController usersController { get; set; }

        public ObservableCollection<User> Users { get; set; }

        private User _SelectedUser;
        public User SelectedUser
        {
            get => _SelectedUser;
            set
            {
                _SelectedUser = value;
                UserChanged();
                NotifyPropertyChanged();
            }
        }

        private UserInfo _Info;
        public UserInfo Info
        {
            get => _Info;
            set
            {
                _Info = value;
                NotifyPropertyChanged();
            }
        }

        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                UsersChanged();
                NotifyPropertyChanged();
            }
        }

        public Command Update { get; set; }
        public Command Remove { get; set; }

        #endregion

        public UsersService()
        {
            usersController = new UsersController();

            Users = new ObservableCollection<User>();
            usersController.UsersChanged += UsersChanged;
            UsersChanged();

            Update = new Command(_Update);
            Remove = new Command(_Remove);

        }

        #region Commands

        public void _Update()
        {
            if (SelectedUser == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select User!");
                return;
            }
            AppSettings.WindowService.Navigate(new UsersAdd(SelectedUser));
        }

        public void _Remove()
        {
            if (SelectedUser == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select User!");
                return;
            }
            usersController.RemoveUser(SelectedUser);
        }

        #endregion

        #region Methods

        private void UserChanged()
        {
            Info = usersController.GetInfo(SelectedUser);
        }

        private void UsersChanged()
        {
            Users.Clear();
            foreach (var item in usersController.GetUsers(Search))
                Users.Add(item);
        }

        #endregion
    }
}
