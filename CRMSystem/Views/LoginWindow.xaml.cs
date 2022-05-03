using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CRMSystem.DataBase;
using CRMSystem.Dialogs;
using CRMSystem.Native.Window;
using CRMSystem.Native.Window.Utils;
using CRMSystem.Settings;
using CRMSystem.Utils;
using CRMSystem.Views.CustomerViews;
using CRMSystem.Views.ManagerViews;
using Math = RIS.Mathematics.Math;

namespace CRMSystem.Views
{
    public partial class LoginWindow : Window, INativelyRestorableWindow
    {
        private static readonly object InstanceSyncRoot = new object();
        private static volatile LoginWindow _instance;
        public static LoginWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceSyncRoot)
                    {
                        if (_instance == null)
                            _instance = new LoginWindow();
                    }
                }

                return _instance;
            }
        }



        private HwndSource _hwndSource;
        private WindowState _previousState;



        public bool DuringRestoreToMaximized { get; private set; }



        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;

            WindowStartupLocation = WindowStartupLocation.Manual;
            Width = SettingsManager.AppSettings.LoginWindowWidth;
            Height = SettingsManager.AppSettings.LoginWindowHeight;
            Left = SettingsManager.AppSettings.LoginWindowPositionX - (Width / 2.0);
            Top = SettingsManager.AppSettings.LoginWindowPositionY - (Height / 2.0);
            WindowState = SettingsManager.AppSettings.LoginWindowState;


            _previousState = WindowState;
            DuringRestoreToMaximized = WindowState == WindowState.Maximized;
        }



        public Task ShowLoadingGrid()
        {
            LoadingIndicator.IsActive = true;
            LoadingGrid.Opacity = 1.0;
            LoadingGrid.IsHitTestVisible = true;
            LoadingGrid.Visibility = Visibility.Visible;

            return Task.CompletedTask;
        }

        public Task HideLoadingGrid()
        {
            LoadingIndicator.IsActive = false;

            return Task.Run(async () =>
            {
                for (var i = 1.0; i > 0.0; i -= 0.025)
                {
                    var opacity = i;

                    if (Math.AlmostEquals(opacity, 0.7, 0.01))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            LoadingGrid.IsHitTestVisible = false;
                        });
                    }

                    Dispatcher.Invoke(() =>
                    {
                        LoadingGrid.Opacity = opacity;
                    });

                    await Task.Delay(4)
                        .ConfigureAwait(false);
                }

                Dispatcher.Invoke(() =>
                {
                    LoadingGrid.Visibility = Visibility.Collapsed;
                });
            });
        }



        protected override void OnSourceInitialized(
            EventArgs e)
        {
            base.OnSourceInitialized(e);

            _hwndSource = (HwndSource)PresentationSource.FromVisual(this);

            _hwndSource?.AddHook(WindowUtils.HwndSourceHook);
        }

        protected override void OnStateChanged(
            EventArgs e)
        {
            _previousState = WindowState;

            base.OnStateChanged(e);

            if (_previousState != WindowState.Minimized)
            {
                DuringRestoreToMaximized = WindowState == WindowState.Maximized;
            }
        }

        protected override void OnDeactivated(
            EventArgs e)
        {
            base.OnDeactivated(e);

            LoginTextBox.Clear();
            PasswordTextBox.Clear();

            RememberMeCheckBox.IsChecked = false;
        }


        private void LoginWindow_Closed(object sender,
            EventArgs e)
        {
            SettingsManager.AppSettings.LoginWindowWidth = Width;
            SettingsManager.AppSettings.LoginWindowHeight = Height;
            SettingsManager.AppSettings.LoginWindowPositionX = Left + (Width / 2.0);
            SettingsManager.AppSettings.LoginWindowPositionY = Top + (Height / 2.0);
            SettingsManager.AppSettings.LoginWindowState = WindowState;

            SettingsManager.AppSettings.Save();

            _hwndSource?.RemoveHook(WindowUtils.HwndSourceHook);
        }



        private void LoginButton_Click(object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password)
                || string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                return;
            }

            var users = DB.Context.Users
                .Where(user => string.Equals(user.Login, LoginTextBox.Text)
                                && string.Equals(user.Password, PasswordTextBox.Password));

            if (!users.Any())
            {
                var message = LocalizationUtils
                    .GetLocalized("UserNotFound");

                DialogManager.ShowErrorDialog(message);

                return;
            }

            var user = users.First();

            if (RememberMeCheckBox.IsChecked == true)
            {
                var setUserSuccess = SettingsManager.PersistentSettings.SetUser(
                    user.Login,
                    user.Password);

                if (!setUserSuccess)
                    return;

                if (!SettingsManager.PersistentSettings.SetCurrentUser(
                        user.Login))
                {
                    return;
                }
            }
            else
            {
                SettingsManager.PersistentSettings.RemoveUser(
                    user.Login);

                if (!SettingsManager.PersistentSettings.SetCurrentUserTemporary(
                        user.Login,
                        user.Password))
                {
                    return;
                }
            }

            switch (user.UserTypeId)
            {
                case 1:
                    Application.Current.MainWindow =
                        new ManagerWindow(user);

                    break;
                case 2:
                    Application.Current.MainWindow =
                        new CustomerWindow(user);

                    break;
                default:
                    break;
            }

            Hide();

            Application.Current.MainWindow?.Show();
        }
    }
}
