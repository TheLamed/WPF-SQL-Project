using Project.Model;
using Project.Model.Controllers;
using Project.Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.ViewModel.Admin
{
    public class LocationsService : INotifyPropertyChanged
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
                NotifyPropertyChanged("Info");
                NotifyPropertyChanged("Image");
            }
        }

        public byte[] Image
        {
            get
            {
                if (SelectedLocation == null) return null;
                return SelectedLocation.Image;
            }
        }
        public string Info
        {
            get
            {
                if (SelectedLocation == null) return "";
                return $"ID: {SelectedLocation.ID}\nCountry: {SelectedLocation.Country}\nCity: {SelectedLocation.City}";
            }
        }

        private string _Country, _City, _ImageWay;
        public string Country
        {
            get => _Country;
            set
            {
                _Country = value;
                NotifyPropertyChanged();
            }
        }
        public string City
        {
            get => _City;
            set
            {
                _City = value;
                NotifyPropertyChanged();
            }
        }
        public string ImageWay
        {
            get => _ImageWay;
            set
            {
                _ImageWay = value;
                NotifyPropertyChanged();
            }
        }

        private Visibility _InfoGridVisibility, _AddGridVisibility;
        public Visibility InfoGridVisibility
        {
            get => _InfoGridVisibility;
            set
            {
                _InfoGridVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public Visibility AddGridVisibility
        {
            get => _AddGridVisibility;
            set
            {
                _AddGridVisibility = value;
                NotifyPropertyChanged();
            }
        }

        public Command AddLocation { get; set; }
        public Command UploadImageLocation { get; set; }
        public Command UpdateLocation { get; set; }
        public Command RemoveLocation { get; set; }
        public Command LocationSelected { get; set; }
        #endregion

        #region Constructors
        public LocationsService()
        {
            serverController = new ServerController();

            Locations = new ObservableCollection<Location>();

            serverController.LocationsChanged += LocationsChanged;
            LocationsChanged();

            InfoGridVisibility = Visibility.Visible;
            AddGridVisibility = Visibility.Collapsed;

            AddLocation = new Command(_AddLocation);
            UploadImageLocation = new Command(_UploadImageLocation, AddGridVisibility == Visibility.Visible);
            UpdateLocation = new Command(_UpdateLocation);
            RemoveLocation = new Command(_RemoveLocation, InfoGridVisibility == Visibility.Visible);
            LocationSelected = new Command(_Locationselected);
        }
        #endregion

        #region Commands

        private void _AddLocation()
        {
            if(AddGridVisibility == Visibility.Visible)
            {
                SelectedLocation = null;
                if (Country == null || Country == "")
                {
                    AppSettings.WindowService.ShowErrorMessage("Country is required");
                    return;
                }

                byte[] image = null;

                if(ImageWay != null && ImageWay != "")
                    image = GetImage(ImageWay);

                Location location = new Location(0, Country, City == "" ? null : City, image);

                serverController.AddLocation(location);

                AppSettings.WindowService.ShowOkMessage("Location added!");
            }
            else
            {
                ImageWay = "";
                Country = "";
                City = "";
            }
            ChangeVisibility();
        }
        private void _UploadImageLocation()
        {
            ImageWay = AppSettings.WindowService.OpenFileDialog("ImageFiles(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG");
        }
        private void _UpdateLocation()
        {
            if (SelectedLocation == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select location!");
                return;
            }
            if (AddGridVisibility == Visibility.Visible)
            {
                if (Country == null || Country == "")
                {
                    AppSettings.WindowService.ShowErrorMessage("Country is required");
                    return;
                }

                byte[] image = null;

                if (ImageWay != null && ImageWay != "")
                    image = GetImage(ImageWay);
                else
                    image = SelectedLocation.Image;

                Location location = new Location(SelectedLocation.ID, Country, City == "" ? null : City, image);

                serverController.UpdateLocation(location);

                AppSettings.WindowService.ShowOkMessage("Location updated!");
            }
            else
            {
                ImageWay = "";
                Country = SelectedLocation.Country;
                City = SelectedLocation.City;
            }
            ChangeVisibility();
        }
        private void _RemoveLocation()
        {
            if(SelectedLocation == null)
            {
                AppSettings.WindowService.ShowErrorMessage("Please select location!");
                return;
            }
            serverController.RemoveLocation(SelectedLocation);
        }
        private void _Locationselected()
        {
            if(InfoGridVisibility != Visibility.Visible)
                ChangeVisibility();
        }
        #endregion

        #region Methods

        private void LocationsChanged()
        {
            Locations.Clear();
            foreach (var item in serverController.GetLocations())
                Locations.Add(item);
        }

        private void ChangeVisibility()
        {
            if(InfoGridVisibility == Visibility.Collapsed)
            {
                InfoGridVisibility = Visibility.Visible;
                AddGridVisibility = Visibility.Collapsed;
            }
            else
            {
                InfoGridVisibility = Visibility.Collapsed;
                AddGridVisibility = Visibility.Visible;
            }
            UploadImageLocation.isCanExecute = AddGridVisibility == Visibility.Visible;
            RemoveLocation.isCanExecute = InfoGridVisibility == Visibility.Visible;
        }

        private byte[] GetImage(string way)
        {
            try
            {
                FileStream f = new FileStream(way, FileMode.Open);
                using (MemoryStream m = new MemoryStream())
                {
                    f.CopyTo(m);
                    return m.ToArray();
                }
            }
            catch (Exception e)
            {
                AppSettings.WindowService.ShowErrorMessage(e.Message);
                return null;
            }
        }
        #endregion
    }
}
