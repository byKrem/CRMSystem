using CRMSystem.View.ManagerViews;
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
using System.Windows.Shapes;

namespace CRMSystem.View
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private CRMSystemEntities DB;
        public LoginWindow()
        {
            InitializeComponent();
            DB = new CRMSystemEntities();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(LoginBox.Text)) return;


            if(DB.Managers.Where(f => string.Equals(f.Login, LoginBox.Text) && 
                                string.Equals(f.Password, PasswordBox.Password)).Count() == 0)
                return;

            Managers manager = DB.Managers.First(f => string.Equals(f.Login, LoginBox.Text) &&
                    string.Equals(f.Password, PasswordBox.Password));
            new WindowMain(manager).Show();
            this.Close();
        }
    }
}
