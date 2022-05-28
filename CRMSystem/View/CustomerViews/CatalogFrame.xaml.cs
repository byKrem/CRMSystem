using CRMSystem.ViewModels;
using System;
using System.Linq;
using System.Windows.Controls;

namespace CRMSystem.Views.CustomerViews
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
                
                if (viewModel.SelectedProduct.Count -
                   _customerWindow.Cart.Count(c => c.Id == viewModel.SelectedProduct.Id) == 0) return;
                _customerWindow.Cart.Add(viewModel.SelectedProduct);
                CartIndicatorBlock.Text = $"В корзине: {_customerWindow.Cart.Count} шт.";
            }

            delta = DateTime.Now;
        }
    }
}
