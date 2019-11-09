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

namespace Project.ViewModel.Admin
{
    public class OrdersService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private OrdersController ordersController { get; set; }

        public ObservableCollection<Order> Orders { get; set; }

        private Order _SelectedOrder;
        public Order SelectedOrder
        {
            get => _SelectedOrder;
            set
            {
                _SelectedOrder = value;
                OrderChanged();
                NotifyPropertyChanged();
            }
        }

        private OrderInfo _Info;
        public OrderInfo Info
        {
            get => _Info;
            set
            {
                _Info = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("InfoUsers");
            }
        }
        public List<string> InfoUsers => Info.Users;


        #endregion

        #region Constructors

        public OrdersService()
        {
            ordersController = new OrdersController();

            Orders = new ObservableCollection<Order>();
            ordersController.OrdersChanged += OrdersChanged;
            OrdersChanged();

        }

        #endregion

        #region Commands



        #endregion

        #region Methods
        private void OrdersChanged()
        {
            Orders.Clear();
            foreach (var item in ordersController.GetOrders())
                Orders.Add(item);
        }
        private void OrderChanged()
        {
            Info = ordersController.GetInfo(SelectedOrder);
        }

        #endregion
    }
}
