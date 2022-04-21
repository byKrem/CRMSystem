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

namespace CRMSystem.View
{
    class StorageAnalytic
    {
        public Products Product { get; set; }
        public double Popularity { get; set; }
    }
    public partial class StorageFrame : UserControl
    {
        CRMSystemEntities DB;
        List<StorageAnalytic> Analytics;
        public StorageFrame()
        {
            InitializeComponent();
            DB = new CRMSystemEntities();
            Analytics = DB.Products.Select(s => new StorageAnalytic
            {
                Product = DB.Products.FirstOrDefault(f => f.Id == s.Id),
                Popularity = (double) DB.ProductOrder.Count(c => c.ProductId == s.Id) / DB.Orders.Count()
            }).ToList();
            listing.ItemsSource = Analytics;
        }
    }
}
