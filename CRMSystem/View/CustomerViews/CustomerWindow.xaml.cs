using CRMSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CRMSystem.Views.CustomerViews
{
    public partial class CustomerWindow : Window
    {
        public List<Products> Cart { get; private set; }
        public Users CurrentCustomer => _currentCustomer;

        private Users _currentCustomer;

        public CustomerWindow(Users customer)
        {
            InitializeComponent();
            Cart = new List<Products>();
            _currentCustomer = customer;
            if (_currentCustomer?.Foto == null)
                CustomerFoto.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
            MainFrame.Navigate(new PersonalAccountFrame(this, _currentCustomer));
            this.DataContext = CurrentCustomer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogFrame(this));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartFrame(this));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalAccountFrame(this, _currentCustomer));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}
