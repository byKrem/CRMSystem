using System.Linq;
using System.Windows.Controls;

namespace CRMSystem.View.CustomerViews
{
    public partial class CatalogFrame : Page
    {
        public CatalogFrame()
        {
            InitializeComponent();
            CRMSystemEntities DB = new CRMSystemEntities();
            listing.ItemsSource = DB.Products.ToList();
        }
    }
}
