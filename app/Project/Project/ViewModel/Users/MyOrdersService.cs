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
    public class MyOrdersService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private OrdersController ordersController { get; set; }
        private UsersController usersController { get; set; }
        private ServerController serverController { get; set; }

        public UserInfo Info
            => usersController.GetInfo(AppSettings.CurrentUser);

        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Order> OwnOrders { get; set; }

        private bool isOwnSelected { get; set; }

        private Order _SelectedOrder;
        public Order SelectedOrder
        {
            get => _SelectedOrder;
            set
            {
                _SelectedOrder = value;
                isOwnSelected = false;
                OrderChanged();
                NotifyPropertyChanged();
            }
        }

        private Order _SelectedOwnOrder;
        public Order SelectedOwnOrder
        {
            get => _SelectedOwnOrder;
            set
            {
                _SelectedOwnOrder = value;
                isOwnSelected = true;
                OrderChanged();
                NotifyPropertyChanged();
            }
        }

        private OrderInfo _OrderInfo;
        public OrderInfo OrderInfo
        {
            get => _OrderInfo;
            set
            {
                _OrderInfo = value;
                OrderUsersChanged();
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<User> OrderUsers { get; set; }

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

        #endregion

        #region Constructors
        public MyOrdersService()
        {
            ordersController = new OrdersController();
            usersController = new UsersController();
            serverController = new ServerController();

            Orders = new ObservableCollection<Order>();
            OwnOrders = new ObservableCollection<Order>();
            ordersController.OrdersChanged += OrdersChanged;
            OrdersChanged();

            OrderUsers = new ObservableCollection<User>();

        }
        #endregion

        #region Commands

        #endregion

        #region Methods
        private void OrdersChanged()
        {
            Orders.Clear();
            OwnOrders.Clear();
            foreach (var item in ordersController.GetOrders(Info.Orders))
            {
                Orders.Add(item);
                if (item.OwnerID == Info.ID)
                    OwnOrders.Add(item);
            }
        }

        private void OrderChanged()
        {
            if (isOwnSelected)
                OrderInfo = ordersController.GetInfo(SelectedOwnOrder);
            else
                OrderInfo = ordersController.GetInfo(SelectedOrder);

            ServerInfo = serverController.GetInfo(
                serverController.GetServer(OrderInfo.ServerID));
        }

        private void OrderUsersChanged()
        {
            OrderUsers.Clear();
            foreach (var item in ordersController.GetUsersByOrder(OrderInfo))
                OrderUsers.Add(item);
        }

        private void SelectedUserChanged()
        {
            UserInfo = usersController.GetInfo(SelectedUser);
        }
        #endregion
    }
}
