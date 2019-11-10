using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using Project.View.Users;
using Project.ViewModel.Admin;
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
    public class UserProfileService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private UsersController usersController { get; set; }
        private OrdersController ordersController { get; set; }

        public UserInfo Info 
            => usersController.GetInfo(AppSettings.CurrentUser);

        public  ObservableCollection<Order> Orders { get; set; }
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
                NotifyPropertyChanged();
            }
        }

        public Command EditButton { get; set; }
        public Command ViewButton { get; set; }

        #endregion

        #region Constructors
        public UserProfileService()
        {
            usersController = new UsersController();
            ordersController = new OrdersController();

            isOwnSelected = false;

            Orders = new ObservableCollection<Order>();
            OwnOrders = new ObservableCollection<Order>();
            ordersController.OrdersChanged += OrdersChanged;
            OrdersChanged();

            EditButton = new Command(_EditButton);
            ViewButton = new Command(_ViewButton);
        }
        #endregion

        #region Commands

        private void _ViewButton()
        {
            if (isOwnSelected)
            {
                if(SelectedOwnOrder == null)
                {
                    AppSettings.WindowService.ShowErrorMessage("Please select order!");
                    return;
                }


            }
            else
            {
                if (SelectedOrder == null)
                {
                    AppSettings.WindowService.ShowErrorMessage("Please select order!");
                    return;
                }


            }
        }

        private void _EditButton()
        {
            AppSettings.WindowService.Navigate(new EditUser());
        }
        #endregion

        #region Methods

        public void OrdersChanged()
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

        #endregion
    }
}
