using CRMSystem.Views.CustomerViews;
using CRMSystem.Views.ManagerViews;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CRMSystem.Views
{
    public partial class LoginWindow : Window
    {
        private CRMSystemEntities DB;
        public LoginWindow()
        {
            InitializeComponent();
            DB = new CRMSystemEntities();
#if DEBUG
            PasswordBox.Password = "!2#4%6&8(0";
            LoginBox.Text = "Gremlin";
#endif
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(LoginBox.Text))
            {
                MessageBox.Show("Поля \"Логин\" и \"Пароль\" обязательны для заполнения!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }


            if(!DB.Users.Any(a => string.Equals(a.Login, LoginBox.Text) && 
                                string.Equals(a.Password, PasswordBox.Password)))
            {
                MessageBox.Show("Такого пользователя не существует!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Users user = DB.Users.First(f => string.Equals(f.Login, LoginBox.Text) &&
                    string.Equals(f.Password, PasswordBox.Password));

            switch(user.UserTypeId)
            {
                case 1:
                    new ManagerWindow(user).Show();
                    break;
                case 2:
                    new CustomerWindow(user).Show();
                    break;
            }

            this.Close();
        }
    }
}
