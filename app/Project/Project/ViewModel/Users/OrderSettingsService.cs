using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Users
{
    public class OrderSettingsService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private OrdersController orderController;
        private UsersController usersController;
        private ServerController serverController;

        private int OrderID { get; set; }

        private OrderInfo _OrderInfo;
        public OrderInfo OrderInfo
        {
            get => _OrderInfo;
            set
            {
                _OrderInfo = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<User> Users { get; set; }

        private User _SelectedUser;
        public User SelectedUser
        {
            get => _SelectedUser;
            set
            {
                _SelectedUser = value;
                SelectedUserChanged();
                NotifyPropertyChanged();
            }
        }

        private UserInfo _UserInfo;
        public UserInfo UserInfo
        {
            get => _UserInfo;
            set
            {
                _UserInfo = value;
                NotifyPropertyChanged();
            }
        }

        private ServerInfo _ServerInfo;
        public ServerInfo ServerInfo
        {
            get => _ServerInfo;
            set
            {
                _ServerInfo = value;
                NotifyPropertyChanged("ServerImage");
                NotifyPropertyChanged();
            }
        }

        public byte[] ServerImage => ServerInfo.Image;

        private string _FindUser;
        public string FindUser
        {
            get => _FindUser;
            set
            {
                _FindUser = value;
                NotifyPropertyChanged();
            }
        }

        public Command AddUser { get; set; }
        public Command RemoveUser { get; set; }
        #endregion

        #region Constructors
        public OrderSettingsService(int orderID)
        {
            orderController = new OrdersController();
            usersController = new UsersController();
            serverController = new ServerController();

            Users = new ObservableCollection<User>();

            OrderID = orderID;
            OrderChanged();

            ServerInfo = serverController.GetInfo(
                serverController.GetServer(OrderInfo.ServerID));

            AddUser = new Command(_AddUser);
            RemoveUser = new Command(_RemoveUser);
        }
        #endregion

        #region Commands

        private void _AddUser()
        {
            User user = usersController.GetUserByLogin(FindUser);
            if(user == null)
            {
                AppSettings.WindowService.ShowErrorMessage("This user doesn`t exist!");
                return;
            }
            orderController.AddUserToOrder(OrderInfo, user);
            OrderChanged();
        }

        private void _RemoveUser()
        {
            if(SelectedUser == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select user!");
                return;
            }
            if(SelectedUser.ID == AppSettings.CurrentUser.ID)
            {
                AppSettings.WindowService.ShowErrorMessage("You can`t remove yourself!");
                return;
            }
            orderController.RemoveUserFromOrder(OrderInfo, SelectedUser);
            OrderChanged();
        }
        #endregion

        #region Methods

        private void OrderChanged()
        {
            OrderInfo = orderController.GetInfo(orderController.GetOrderById(OrderID));
            OrderUsersChanged();
        }

        private void SelectedUserChanged()
        {
            UserInfo = usersController.GetInfo(SelectedUser);
        }

        private void OrderUsersChanged()
        {
            Users.Clear();
            foreach (var item in orderController.GetUsersByOrder(OrderInfo))
                Users.Add(item);
        }
        #endregion
    }
}
