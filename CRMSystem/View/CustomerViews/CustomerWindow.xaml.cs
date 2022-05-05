using CRMSystem.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CRMSystem.View.CustomerViews
{
    public partial class CustomerWindow : Window
    {
        public List<Products> Cart { get; private set; }
        private Users _currentCustomer;

        public CustomerWindow(Users customer)
        {
            InitializeComponent();
            Cart = new List<Products>();
            _currentCustomer = customer;
            MainFrame.Navigate(new CatalogFrame(this));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogFrame(this));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CartFrame(this));
        }
    }
}
