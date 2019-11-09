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

        #endregion

        public UserService(UserWindow parrent)
        {
            _parrent = parrent;

            Closing = AppSettings.WindowService.Closing;
            AppSettings.WindowService.Frame = _parrent.Frame;
        }
    }
}
