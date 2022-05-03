using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CRMSystem.DataBase;

namespace CRMSystem.Views.ManagerViews
{
    class StorageAnalytic
    {
        public Products Product { get; set; }
        public double Popularity { get; set; }
    }
    public partial class StorageFrame : UserControl
    {
        List<StorageAnalytic> Analytics;
        public StorageFrame()
        {
            InitializeComponent();
            Analytics = DB.Context.Products.Select(s => new StorageAnalytic
            {
                Product = DB.Context.Products.FirstOrDefault(f => f.Id == s.Id),
                Popularity = (double)DB.Context.ProductOrder.Count(c => c.ProductId == s.Id) / DB.Context.Orders.Count()
            }).ToList();
            grid.ItemsSource = Analytics;

        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Image)) return;

            Image image = sender as Image;
            if ((image.DataContext as StorageAnalytic)?.Product?.Image == null)
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/CRMSystem;component/Resources/Images/Placeholders/ImagePlaceholder.png"));
        }
    }
}
