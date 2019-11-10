using Project.Model;
using Project.View.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Admin
{
    public class AdminService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private AdminWindow _parrent;
        public Command Closing { get; set; }

        public Command SettingsBack { get; set; }
        public Command SettingsForward { get; set; }
        public Command SettingsLogout { get; set; }

        public Command ServersInfo { get; set; }
        public Command ServersAdd { get; set; }
        public Command ServersLocations { get; set; }

        public Command UsersInfo { get; set; }
        public Command UsersAdd { get; set; }

        public Command OrdersInfo { get; set; }
        #endregion

        #region Constructors
        public AdminService(AdminWindow parrent)
        {
            _parrent = parrent;
            Closing = AppSettings.WindowService.Closing;
            AppSettings.WindowService.Frame = _parrent.Frame;

            SettingsBack = new Command(_SettingsBack, _parrent.Frame.CanGoBack);
            SettingsForward = new Command(_SettingsForward, _parrent.Frame.CanGoForward);
            SettingsLogout = new Command(_SettingsLogout);

            ServersInfo = new Command(_ServersInfo);
            ServersAdd = new Command(_ServersAdd);
            ServersLocations = new Command(_ServersLocations);

            UsersInfo = new Command(_UsersInfo);
            UsersAdd = new Command(_UsersAdd);

            OrdersInfo = new Command(_OrdersInfo);

            _parrent.Frame.Navigated += _NavigateButtonsUpdate;
        }
        #endregion

        #region Commands

        private void _SettingsBack()
        {
            AppSettings.WindowService.NavigateBack();
        }
        private void _SettingsForward()
        {
            AppSettings.WindowService.NavigateForward();
        }
        private void _SettingsLogout()
        {
            AppSettings.WindowService.Back();
        }

        private void _ServersInfo()
        {
            AppSettings.WindowService.Navigate(new ServersInfo());
        }
        private void _ServersAdd()
        {
            AppSettings.WindowService.Navigate(new AddServer());
        }
        private void _ServersLocations()
        {
            AppSettings.WindowService.Navigate(new Locations());
        }

        private void _UsersInfo()
        {
            AppSettings.WindowService.Navigate(new UsersInfo());
        }
        private void _UsersAdd()
        {
            AppSettings.WindowService.Navigate(new UsersAdd());
        }

        private void _OrdersInfo()
        {
            AppSettings.WindowService.Navigate(new OrdersInfo());
        }
        #endregion

        #region Methods

        private void _NavigateButtonsUpdate(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            SettingsForward.isCanExecute = _parrent.Frame.CanGoForward;
            SettingsBack.isCanExecute = _parrent.Frame.CanGoBack;
        }

        #endregion
    }
}
