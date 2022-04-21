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
    /// <summary>
    /// Логика взаимодействия для ClientsFrame.xaml
    /// </summary>
    public partial class ClientsFrame : UserControl
    {
        CRMSystemEntities DB;
        int CurrentManagerId = 1;
        public ClientsFrame()
        {
            InitializeComponent();
            DB = new CRMSystemEntities();
            grid.ItemsSource = DB.Customers.Where(w => w.ManagerId == CurrentManagerId).ToList();
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
