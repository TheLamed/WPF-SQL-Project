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
    public class ServersService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties

        private ServerController serverController { get; set; }
        public ObservableCollection<Server> Servers { get; private set; }

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

        private ServerInfo _Info;
        public ServerInfo Info
        {
            get => _Info;
            set
            {
                _Info = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Image");
            }
        }
        public byte[] Image { get => Info.Image; }


        public Command Remove { get; set; }
        public Command Update { get; set; }

        #endregion

        #region Constructors
        public ServersService()
        {
            serverController = new ServerController();

            Servers = new ObservableCollection<Server>();

            serverController.ServersChanged += ServersChanged;
            ServersChanged();

            Remove = new Command(_Remove);
            Update = new Command(_Update);
        }
        #endregion

        #region Commands

        private void _Remove()
        {
            if (SelectedServer == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Server is not selected");
                return;
            }
            serverController.RemoveServer(SelectedServer);
        }

        private void _Update()
        {
            if (SelectedServer == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Server is not selected");
                return;
            }
            AppSettings.WindowService.Navigate(new AddServer(SelectedServer));
        }

        #endregion

        #region Methods

        public void ServerChanged()
        {
            Info = serverController.GetInfo(SelectedServer);
        }

        private void ServersChanged()
        {
            Servers.Clear();
            foreach (var item in serverController.GetServers())
                Servers.Add(item);
        }
        #endregion
    }
}
