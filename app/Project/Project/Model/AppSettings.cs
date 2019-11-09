using Project.Model.Models;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Model
{
    public static class AppSettings
    {
        public static string ConnectionString { get; set; }
        public static WindowService WindowService { get; set; }
        public static User CurrentUser { get; set; }
        public static bool IsAdmin { get; set; }

        static AppSettings()
        {
            WindowService = new WindowService();
            ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Hosting;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
    }
}
