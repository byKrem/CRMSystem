using System.Linq;
using System.Windows.Controls;
using CRMSystem.DataBase;

namespace CRMSystem.Views.CustomerViews
{
    public partial class CatalogFrame : Page
    {
        public CatalogFrame()
        {
            InitializeComponent();

            listing.ItemsSource = DB.Context.Products
                .ToList();
        }
    }
}
