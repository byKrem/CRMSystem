using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CRMSystem.View.ManagerViews
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccountFrame.xaml
    /// </summary>
    
    class PersonalAnalityc
    {
        public Managers Manager { get; set; }
        public int CompleatedOrdersCount { get; set; }
        public int InProcessingOrdersCount { get; set; }
        public int NewOrdersCount { get; set; }
        public double Score { get; set; }
    }

    public partial class PersonalAccountFrame : Page
    {
        readonly Managers _currentManager;
        PersonalAnalityc personalAnalityc;
        CRMSystemEntities DB;
        WindowMain windowMain;
        DateTime delta;
        public PersonalAccountFrame(WindowMain wm, Managers manager)
        {
            InitializeComponent();
            _currentManager = manager;
            StartupTimeBlock.DataContext = wm;
            ImageFoto.DataContext = _currentManager;
            windowMain = wm;

            DB = new CRMSystemEntities();

            personalAnalityc = new PersonalAnalityc
            {
                Manager = _currentManager,
                CompleatedOrdersCount = DB.Orders.Where(w => w.OrderStatusId == 6 &&
                    w.Customers.ManagerId == _currentManager.Id).Count(),
                InProcessingOrdersCount = DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                w.OrderStatusId != 6 && w.OrderStatusId != 5).Count(),
                NewOrdersCount = DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                    w.OrderStatusId == 1).Count(),
                Score = 5.2
            };

            LiveCharts.SeriesCollection seriesViews = new LiveCharts.SeriesCollection
            {
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId == 6).Count() },
                    Title = "Завершённые заказы"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId != 5 && w.OrderStatusId != 6).Count()},
                    Title = "Заказы в обработке"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                    w.OrderStatusId == 1).Count()},
                    Title = "Новые заказы"
                }
            };

            PieChart.Series = seriesViews;


            OrdersAnalyticPie.Series = new LiveCharts.SeriesCollection
            {
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId == 6).Count() },
                    Title = "Завершённые заказы"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId != 5 && w.OrderStatusId != 6).Count()},
                    Title = "Заказы в обработке"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                    w.OrderStatusId == 1).Count()},
                    Title = "Новые заказы"
                },
                new LiveCharts.Wpf.PieSeries
                {
                    Values = new LiveCharts.ChartValues<decimal>
                    {
                        DB.Orders.Where(w => w.Customers.ManagerId == _currentManager.Id &&
                        w.OrderStatusId == 5).Count()
                    },
                    Title = "Отменённые заказы"
                }
            };

            ColumnChart.Series = new LiveCharts.SeriesCollection
            {
                new LiveCharts.Wpf.ColumnSeries
                {
                    Title = "Прибыль за квартал",
                    Values = new LiveCharts.ChartValues<decimal>
                    {
                        DB.PaymentHistory
                        .Where(w => w.ManagerId == _currentManager.Id && w.Amount > 0)
                        .Sum(s => s.Amount)
                    }
                },
                new LiveCharts.Wpf.ColumnSeries
                {
                    Title = "Штрафы",
                    Values = new LiveCharts.ChartValues<decimal>
                    {
                        Math.Abs(DB.PaymentHistory
                        .Where(w => w.ManagerId == _currentManager.Id && w.Amount < 0)
                        .Sum(s => s.Amount))
                    }
                },
            };

            SalaryGrid.ItemsSource = DB.PaymentHistory.Where(w =>
                    w.ManagerId == _currentManager.Id).ToList();

            grid.DataContext = personalAnalityc;
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Image)) return;
            
            Image image = sender as Image;
            if (_currentManager.Foto == null)
                image.Source = new BitmapImage(
                    new Uri(@"pack://application:,,,/CRMSystem;component/IMG/unknownImage.png"));
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            windowMain.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(DateTime.Now - delta < TimeSpan.FromMilliseconds(500))
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.FileName = "";
                dlg.DefaultExt = ".png";
                dlg.Filter = "Image files (.png)|*.png;*.jpg;*.jpeg";

                if (dlg.ShowDialog() != true) return;

                byte[] imageBytes = null;
                using (var fs = new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                {
                    imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length));
                }
                _currentManager.Foto = imageBytes;
                DB.Managers.Append(_currentManager);
                DB.SaveChanges();
            }
            delta = DateTime.Now;
        }

        private void ButtonSaveEmailPhone_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EmailBox.Text) && IsValidEmail(EmailBox.Text))
                _currentManager.Email = EmailBox.Text;

            if(!string.IsNullOrEmpty(PhoneBox.Text))
                _currentManager.Phone = long.Parse(PhoneBox.Text);

            DB.Managers.Append(_currentManager);
            DB.SaveChanges();
        }

        private bool IsValidEmail(string Email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(Email.ToLower(), pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void PhoneBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string pattern = "[^0-9.-]+";
            Match isMatch = Regex.Match(e.Text, pattern, RegexOptions.IgnoreCase);
            if (isMatch.Success)
                e.Handled = true;
        }
    }
}
