using System;
using System.Collections.Generic;
using System.IO;
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

namespace CRMSystem.View.ManagerViews
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
            grid.ItemsSource = Analytics;

        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {

            if (!(sender is Image)) return;

            Image image = sender as Image;
            if ((image.DataContext as StorageAnalytic)?.Product?.Image == null)
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));

        }
    }
}
