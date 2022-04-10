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
    class OrderDetails
    {
        public Orders Order { get; set; }
        public decimal Cost { get; set; }
        public Customers Customer { get; set; }
    }
    public partial class OrdersFrame : UserControl
    {
        private DateTime delta;
        private CRMSystemEntities DB;
        private List<OrderDetails> OrderDetails;
        public OrdersFrame()
        {
            InitializeComponent();
            InitializeItemsSources();
        }

        public void InitializeItemsSources()
        {
            DB = new CRMSystemEntities();
            OrderDetails = DB.ProductOrder.GroupBy(i => i.OrderId).Select(x => new View.OrderDetails
            {
                Order = DB.Orders.FirstOrDefault(i => i.Id == x.Key),
                Cost = x.Sum(s => s.Products.Price * s.ProductCount),
                Customer = DB.Orders.FirstOrDefault(c => c.Id == x.Key).Customers
            }).ToList();
            listing.ItemsSource = OrderDetails;
        }

        private void Listing_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DateTime.Now - delta <= TimeSpan.FromMilliseconds(400))
            {
                var t = (sender as Grid).DataContext as OrderDetails;
            }
            delta = DateTime.Now;
        }
    }
}
