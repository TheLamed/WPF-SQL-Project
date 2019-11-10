using Project.Model;
using Project.View.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModel.Users
{
    public class UserService : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        #region Properties
        private UserWindow _parrent;
        public Command Closing { get; set; }

        public Command SettingsBack { get; set; }
        public Command SettingsForward { get; set; }
        public Command SettingsProfile { get; set; }
        public Command SettingsLogout { get; set; }
        public Command SettingsEditProfile { get; set; }

        public Command OrdersMy { get; set; }
        public Command OrdersNew { get; set; }
        #endregion

        #region Constructors
        public UserService(UserWindow parrent)
        {
            _parrent = parrent;
            Closing = AppSettings.WindowService.Closing;
            AppSettings.WindowService.Frame = _parrent.Frame;
            _parrent.Frame.Navigated += _NavigateButtonsUpdate;

            SettingsBack = new Command(_SettingsBack, _parrent.Frame.CanGoBack);
            SettingsForward = new Command(_SettingsForward, _parrent.Frame.CanGoForward);
            SettingsLogout = new Command(_SettingsLogout);
            SettingsProfile = new Command(_SettingsProfile);
            SettingsEditProfile = new Command(_SettingsEditProfile);

            OrdersMy = new Command(_OrdersMy);
            OrdersNew = new Command(_OrdersNew);
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
        private void _SettingsProfile()
        {
            AppSettings.WindowService.Navigate(new UserProfile());
        }
        private void _SettingsEditProfile()
        {
            AppSettings.WindowService.Navigate(new EditUser());
        }

        private void _OrdersMy()
        {
            AppSettings.WindowService.Navigate(new MyOrders());
        }
        private void _OrdersNew()
        {
            //AppSettings.WindowService.Navigate();
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
