using CRMSystem.ViewModels;
using System;
using System.Windows.Controls;

namespace CRMSystem.View.CustomerViews
{
    public partial class CatalogFrame : Page
    {
        private CustomerWindow _customerWindow;
        private CatalogViewModel viewModel;
        private DateTime delta;
        public CatalogFrame(CustomerWindow customerWindow)
        {
            InitializeComponent();
            viewModel = new CatalogViewModel();
            this.DataContext = viewModel;
            _customerWindow = customerWindow;
            CartIndicatorBlock.Text = $"В корзине: {_customerWindow.Cart.Count} шт.";
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                _customerWindow.Cart.Add(viewModel.SelectedProduct);
                CartIndicatorBlock.Text = $"В корзине: {_customerWindow.Cart.Count} шт.";
            }

            delta = DateTime.Now;
        }
    }
}
