using System;
using System.Linq;
using System.Windows.Controls;

namespace CRMSystem.View.CustomerViews
{
    public partial class CatalogFrame : Page
    {
        private CustomerWindow _customerWindow;
        private DateTime delta;
        public CatalogFrame(CustomerWindow customerWindow)
        {
            InitializeComponent();
            _customerWindow = customerWindow;
            CartIndicatorBlock.Text = $"В корзине: {_customerWindow.Cart.Count} шт.";
            CRMSystemEntities DB = new CRMSystemEntities();
            ListViewProducts.ItemsSource = DB.Products.ToList();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                Products product = (sender as Border).DataContext as Products;
                _customerWindow.Cart.Add(product);
                CartIndicatorBlock.Text = $"В корзине: {_customerWindow.Cart.Count} шт.";
            }

            delta = DateTime.Now;
        }
    }
}
