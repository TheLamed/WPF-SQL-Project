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
    public class NewOrderService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private ServerController serverController;
        private OrdersController orderController;

        private string _Proc, _RAM, _SSD;
        public string Proc
        {
            get => _Proc;
            set
            {
                _Proc = value;
                ServersChanged();
                NotifyPropertyChanged();
            }
        }
        public string RAM
        {
            get => _RAM;
            set
            {
                _RAM = value;
                ServersChanged();
                NotifyPropertyChanged();
            }
        }
        public string SSD
        {
            get => _SSD;
            set
            {
                _SSD = value;
                ServersChanged();
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Server> Servers { get; set; }

        private Server _SelectedServer;
        public Server SelectedServer
        {
            get => _SelectedServer;
            set
            {
                _SelectedServer = value;
                ServerChanged();
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
                NotifyPropertyChanged();
                NotifyPropertyChanged("Image");
            }
        }

        public byte[] Image => ServerInfo.Image;

        private bool isDateSelected { get; set; } = false;
        private DateTime _FinishDate;
        public DateTime FinishDate
        {
            get => _FinishDate;
            set
            {
                _FinishDate = value;
                isDateSelected = true;
                NotifyPropertyChanged();
            }
        }

        public Command Order { get; set; }

        #endregion

        #region Constructors
        public NewOrderService()
        {
            serverController = new ServerController();
            orderController = new OrdersController();

            Servers = new ObservableCollection<Server>();
            ServersChanged();

            Order = new Command(_Order);
        }
        #endregion

        #region Commands

        private void _Order()
        {
            if (!isDateSelected)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select date!");
                return;
            }
            if (SelectedServer == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select server!");
                return;
            }

            orderController.AddOrder(new Order(0, AppSettings.CurrentUser.ID ?? 0,
                SelectedServer.ID ?? 0, DateTime.Now, FinishDate));

            AppSettings.WindowService.ShowOkMessage("Ordered!");
        }

        #endregion

        #region Methods

        private void ServerChanged()
        {
            ServerInfo = serverController.GetInfo(SelectedServer);
        }

        private void ServersChanged()
        {
            string p = "";
            int r, s;

            if (Proc != null) p = Proc;

            try
            {
                if (RAM == null || RAM == "") throw new Exception();
                r = Convert.ToInt32(RAM);
            }
            catch (Exception)
            {
                r = -1;
            }
            try
            {
                if (SSD == null || SSD == "") throw new Exception();
                s = Convert.ToInt32(SSD);
            }
            catch (Exception)
            {
                s = -1;
            }

            Servers.Clear();
            foreach (var item in serverController.FingServers(p, r, s))
                Servers.Add(item);
        }

        #endregion
    }
}
