﻿using Project.Model.Models;
using Project.ViewModel.Admin;
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

namespace Project.View.Admin
{
    /// <summary>
    /// Interaction logic for UsersAdd.xaml
    /// </summary>
    public partial class UsersAdd : Page
    {
        public UsersAdd(User user = null)
        {
            InitializeComponent();
            DataContext = new AddUserService(user);
        }
    }
}