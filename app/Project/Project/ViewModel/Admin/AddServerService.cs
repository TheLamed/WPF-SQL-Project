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

namespace Project.ViewModel.Admin
{
    class AddServerService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties

        private ServerController serverController { get; set; }

        public ObservableCollection<Location> Locations { get; set; }
        private Location _SelectedLocation;

        public Location SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                NotifyPropertyChanged();
            }
        }


        private string _LocationSearch;
        public string LocationSearch
        {
            get => _LocationSearch;
            set
            {
                _LocationSearch = value;
                LocationSearchChanged.Execute(null);
                NotifyPropertyChanged();
            }
        }

        private string _Processor, _RAM, _SSD;
        public string Processor
        {
            get => _Processor;
            set
            {
                _Processor = value;
                NotifyPropertyChanged();
            }
        }
        public string RAM
        {
            get => _RAM;
            set
            {
                _RAM = value;
                NotifyPropertyChanged();
            }
        }
        public string SSD
        {
            get => _SSD;
            set
            {
                _SSD = value;
                NotifyPropertyChanged();
            }
        }

        private bool isUpdate { get; set; }
        public string ButtonContent => isUpdate ? "Update" : "Add";
        private int ServerID { get; set; }

        public Command LocationSearchChanged { get; set; }
        public Command AddButton { get; set; }

        #endregion

        public AddServerService()
        {
            serverController = new ServerController();

            Locations = new ObservableCollection<Location>();
            _LocationSearchChanged();
            isUpdate = false;

            LocationSearchChanged = new Command(_LocationSearchChanged);
            AddButton = new Command(_AddButton);
        }
        public AddServerService(Server server) : this()
        {
            if (server == null || server.ID == null) return;

            isUpdate = true;
            ServerID = server.ID ?? 0;
            Processor = server.Processor;
            RAM = server.RAM.ToString();
            SSD = server.SSD.ToString();

            SelectedLocation = Locations.First(location => location.ID == server.LocationID ? true : false);
        }

        #region Commands

        private void _LocationSearchChanged()
        {
            Locations.Clear();
            foreach (var item in serverController.GetLocations(LocationSearch))
                Locations.Add(item);
        }
        
        private void _AddButton()
        {
            int? ram = null, ssd = null;
            try
            {
                if (RAM != null && RAM.Trim() != "")
                    ram = Convert.ToInt32(RAM);
                if (SSD != null && SSD.Trim() != "")
                    ssd = Convert.ToInt32(SSD);
            }
            catch (Exception)
            {
                AppSettings.WindowService.ShowErrorMessage("Convertation error!");
                return;
            }

            if (SelectedLocation == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Location is not selected!");
                return;
            }

            string proc = null;
            if (Processor != null && Processor.Trim() != "")
                proc = Processor;

            Server server = new Server(proc, ram, ssd, SelectedLocation.ID);

            if (isUpdate)
            {
                serverController.UpdateServer(ServerID, server);
                AppSettings.WindowService.ShowOkMessage("Server updated!");
            }
            else
            {
                serverController.AddServer(server);
                AppSettings.WindowService.ShowOkMessage("Server added!");
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
