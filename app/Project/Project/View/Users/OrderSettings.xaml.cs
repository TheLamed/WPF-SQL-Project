using Project.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.View.Users
{
    /// <summary>
    /// Interaction logic for OrderSettings.xaml
    /// </summary>
    public partial class OrderSettings : Page
    {
        public OrderSettings(int OrderID)
        {
            InitializeComponent();
            DataContext = new OrderSettingsService(OrderID);
        }
    }
}
