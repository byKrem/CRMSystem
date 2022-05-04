using System.Collections.Generic;
using System.Windows.Controls;

namespace CRMSystem.View.CustomerViews
{
    public partial class CartFrame : Page
    {
        public CartFrame(List<Products> products)
        {
            InitializeComponent();
            Listing.ItemsSource = products;
        }
    }
}
