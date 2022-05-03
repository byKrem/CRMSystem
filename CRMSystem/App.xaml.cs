using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CRMSystem.Cryptography;
using CRMSystem.DataBase;
using CRMSystem.Dialogs;
using CRMSystem.Native;
using CRMSystem.Settings;
using CRMSystem.Utils;
using CRMSystem.Views;
using CRMSystem.Views.CustomerViews;
using CRMSystem.Views.ManagerViews;
using RIS.Cryptography;
using RIS.Extensions;
using RIS.Localization;
using RIS.Localization.Providers;
using RIS.Logging;
using RIS.Synchronization.Context;
using RIS.Wrappers;
using Environment = RIS.Environment;

namespace CRMSystem
{
    public partial class App : Application
    {
        private const string UniqueName = "App+CRMSystem+{9CADDB06-18B2-46E0-91FB-4ACDEACBCC45}";



        private static readonly object InstanceSyncRoot = new object();
        private static volatile SingleInstanceApp _instance;
        internal static SingleInstanceApp Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceSyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new SingleInstanceApp(UniqueName);
                        }
                    }
                }

                return _instance;
            }
        }



        private static bool AppCreateHashFiles { get; set; }



        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                SingleThreadedSynchronizationContext.Await(() =>
                {
                    return MainAsync(args);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static Task MainAsync(string[] args)
        {
            ParseArgs(args);

            LogManager.Startup();

            if (AppCreateHashFiles)
            {
                CreateHashFiles();

                return Task.CompletedTask;
            }

            return Instance.Run(() =>
            { 
                SingleInstanceMain();
            });
        }

        private static void SingleInstanceMain()
        {
            var app = new App();

            app.DispatcherUnhandledException += app.OnDispatcherUnhandledException;

            LogManager.LoggingShutdown += app.LogManager_LoggingShutdown;

            LocalizationUtils.Initialize();

            LocalizationUtils.LocalizationsLoaded += app.LocalizationUtils_LocalizationsLoaded;
            LocalizationUtils.LocalizationsNotFound += app.LocalizationUtils_LocalizationsNotFound;
            LocalizationUtils.LocalizationFileNotFound += app.LocalizationUtils_LocalizationFileNotFound;
            LocalizationUtils.LocalizedCultureNotFound += app.LocalizationUtils_LocalizedCultureNotFound;

            app.ShutdownMode = ShutdownMode.OnMainWindowClose;

            app.InitializeComponent();
            app.Run();
        }



        private static void ParseArgs(
            string[] args)
        {
            var wrapper = UnwrapArgs(
                args);

            foreach (var (key, value) in wrapper.Enumerate())
            {
                switch (key)
                {
                    case "createHashFiles":
                        var createHashFilesString = (string)value;

                        try
                        {
                            AppCreateHashFiles = string.IsNullOrWhiteSpace(createHashFilesString)
                                                 || createHashFilesString.ToBoolean();
                        }
                        catch (Exception)
                        {
                            AppCreateHashFiles = false;
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private static void CreateHashFile(
            string hash, string hashName,
            string hashType)
        {
            var currentAppVersion = FileVersionInfo
                .GetVersionInfo(Environment.ExecAppAssemblyFilePath)
                .ProductVersion;
            var hashFileNameWithoutExtension = $"{Environment.ExecAppAssemblyFileNameWithoutExtension.Replace('.', '_')}." +
                                                  $"v{currentAppVersion.Replace('.', '_')}." +
                                                  $"{Environment.RuntimeIdentifier.Replace('.', '_')}." +
                                                  $"{(!Environment.IsStandalone ? "!" : string.Empty)}{nameof(Environment.IsStandalone)}." +
                                                  $"{(!Environment.IsSingleFile ? "!" : string.Empty)}{nameof(Environment.IsSingleFile)}";
            var hashFileDirectoryName =
                Path.Combine(
                    Environment.ExecProcessDirectoryName ?? string.Empty,
                    "hash",
                    hashType);

            if (!Directory.Exists(hashFileDirectoryName))
                Directory.CreateDirectory(hashFileDirectoryName);

            using var file = new StreamWriter(
                Path.Combine(
                    hashFileDirectoryName,
                    $"{hashName}.{hashFileNameWithoutExtension}.{hashType}"),
                false, SecureUtils.SecureUTF8);

            file.WriteLine(hash);
        }




        public static ArgsKeyedWrapper UnwrapArgs(
            string[] args)
        {
            var argsEntries = new List<(string Key, object Value)>(args.Length);

            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                var separatorPosition = arg
                    .IndexOf(':');

                if (separatorPosition == -1)
                {
                    argsEntries.Add((
                        arg
                            .Trim(' ', '-', '\'', '\"')
                            .TrimStart('-'),
                        string.Empty
                        ));

                    continue;
                }

                argsEntries.Add((
                    arg[..separatorPosition]
                        .Trim(' ', '\'', '\"')
                        .TrimStart('-'),
                    arg[(separatorPosition + 1)..]
                        .Trim(' ', '\'', '\"')
                    ));
            }

            return new ArgsKeyedWrapper(argsEntries);
        }

        public static (string LibrariesHash, string ExeHash, string ExePdbHash) CalculateHashes()
        {
            var librariesHash = HashManager.GetLibrariesHash();
            var exeHash = HashManager.GetExeHash();
            var exePdbHash = HashManager.GetExePdbHash();

            LogManager.Default.Info($"Libraries SHA512 - {librariesHash}");
            LogManager.Default.Info($"Exe SHA512 - {exeHash}");
            LogManager.Default.Info($"Exe PDB SHA512 - {exePdbHash}");

            return (librariesHash, exeHash, exePdbHash);
        }

        public static void CreateHashFiles()
        {
            const string hashType = "sha512";

            LogManager.Default.Info($"Hash file creation started - {hashType}");

            var (librariesHash, exeHash, exePdbHash) = CalculateHashes();

            CreateHashFile(librariesHash, "LibrariesHash", hashType);
            CreateHashFile(exeHash, "ExeHash", hashType);
            CreateHashFile(exePdbHash, "ExePdbHash", hashType);

            LogManager.Default.Info($"Hash files creation completed - {hashType}");
        }



#pragma warning disable SS001 // Async methods should return a Task to make them awaitable
        protected override async void OnStartup(
            StartupEventArgs e)
        {
            var splash = new SplashScreen(
                @"Resources\Images\OrionTech.png");

            splash.Show(false);

            MainWindow = LoginWindow.Instance;

            await Task.Delay(
                    TimeSpan.FromSeconds(0.2))
                .ConfigureAwait(true);

            base.OnStartup(e);

            await Task.Delay(
                    TimeSpan.FromSeconds(0.5))
                .ConfigureAwait(true);

            LocalizationUtils.ReloadLocalizations<XamlLocalizationProvider>();

            if (LocalizationUtils.Localizations.Count == 0)
                return;

            LocalizationUtils.SetDefaultCulture(
                "en-US");
            LocalizationUtils.SwitchLocalization(
                SettingsManager.AppSettings.Language);

            LocalizationUtils.LocalizationChanged += LocalizationUtils_LocalizationChanged;

            await Task.Run(() =>
            {
                LogManager.DeleteLogs(SettingsManager.AppSettings
                    .LogRetentionDaysPeriod);

                var currentUserLogin =
                    SettingsManager.PersistentSettings.GetCurrentUserLogin();

                if (string.IsNullOrEmpty(currentUserLogin))
                {
                    return;
                }

                if (!SettingsManager.PersistentSettings.SetCurrentUser(currentUserLogin))
                {
                    SettingsManager.PersistentSettings.RemoveUser(
                        currentUserLogin);

                    return;
                }

                var localUser = SettingsManager.PersistentSettings.CurrentUser;
                var remoteUsers = DB.Context.Users
                    .Where(user => string.Equals(user.Login, localUser.Login)
                                    && string.Equals(user.Password, localUser.Password));

                if (!remoteUsers.Any())
                {
                    SettingsManager.PersistentSettings.RemoveUser(
                        currentUserLogin);

                    return;
                }

                var remoteUser = remoteUsers.First();

                Dispatcher.Invoke(() =>
                {
                    switch (remoteUser.UserTypeId)
                    {
                        case 1:
                            MainWindow =
                                new ManagerWindow(remoteUser);

                            break;
                        case 2:
                            MainWindow =
                                new CustomerWindow(remoteUser);

                            break;
                        default:
                            break;
                    }
                });
            }).ConfigureAwait(true);

            splash.Close(
                TimeSpan.FromSeconds(0.5));

            await Task.Delay(
                    TimeSpan.FromSeconds(1.0))
                .ConfigureAwait(true);

            MainWindow?.Show();
        }
#pragma warning restore SS001 // Async methods should return a Task to make them awaitable

        protected override void OnExit(
            ExitEventArgs e)
        {
            SettingsManager.AppSettings.Save();

            base.OnExit(e);
        }



        private void OnDispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            LogManager.Default.Fatal(
                $"{e.Exception?.GetType().Name ?? "Unknown"} - " +
                $"Message={e.Exception?.Message ?? "Unknown"}," +
                $"HResult={e.Exception?.HResult.ToString() ?? "Unknown"}," +
                $"StackTrace=\n{e.Exception?.StackTrace ?? "Unknown"}");
        }



        private void LogManager_LoggingShutdown(object sender,
            EventArgs e)
        {
            DispatcherUnhandledException -= OnDispatcherUnhandledException;
        }



        private void LocalizationUtils_LocalizationChanged(object sender,
            LocalizationChangedEventArgs e)
        {
            Dispatcher?.Invoke(() =>
            {
                Thread.CurrentThread.CurrentCulture = e.NewLocalization?.Culture
                                                      ?? LocalizationUtils.DefaultCulture;
                Thread.CurrentThread.CurrentUICulture = e.NewLocalization?.Culture
                                                        ?? LocalizationUtils.DefaultCulture;
            });

            SettingsManager.AppSettings.Language = e.NewLocalization.CultureName;

            SettingsManager.AppSettings.Save();
        }

        private void LocalizationUtils_LocalizationsLoaded(object sender,
            EventArgs e)
        {
            if (LocalizationUtils.Localizations.Count > 0)
                return;

            LocalizationUtils.TryGetLocalized(
                "LocalizationsNotFoundMessage", out var message);

            if (string.IsNullOrEmpty(message))
                message = "Localizations not found";

            DialogManager.ShowErrorDialog(message);

            Shutdown(0x1);
        }

        private void LocalizationUtils_LocalizationsNotFound(object sender,
            EventArgs e)
        {
            LocalizationUtils.TryGetLocalized(
                "LocalizationsNotFoundMessage", out var message);

            if (string.IsNullOrEmpty(message))
                message = "Localizations not found";

            DialogManager.ShowErrorDialog(message);

            Shutdown(0x1);
        }

        private void LocalizationUtils_LocalizationFileNotFound(object sender,
            LocalizationFileNotFoundEventArgs e)
        {
            LocalizationUtils.TryGetLocalized(
                "LocalizationFileTitle", out var localizationFileTitle);

            if (string.IsNullOrEmpty(localizationFileTitle))
                localizationFileTitle = "Localization file";

            LocalizationUtils.TryGetLocalized(
                "NotFoundTitle1", out var notFoundTitle);

            if (string.IsNullOrEmpty(notFoundTitle))
                notFoundTitle = "Not found";

            DialogManager.ShowErrorDialog(
                $"{localizationFileTitle}['{e.FilePath}'] {notFoundTitle.ToLower()}");
        }

        private void LocalizationUtils_LocalizedCultureNotFound(object sender,
            LocalizedCultureNotFoundEventArgs e)
        {
            LocalizationUtils.TryGetLocalized(
                "LocalizedCultureTitle", out var localizedCultureTitle);

            if (string.IsNullOrEmpty(localizedCultureTitle))
                localizedCultureTitle = "Localized culture";

            LocalizationUtils.TryGetLocalized(
                "NotFoundTitle2", out var notFoundTitle);

            if (string.IsNullOrEmpty(notFoundTitle))
                notFoundTitle = "Not found";

            DialogManager.ShowErrorDialog(
                $"{localizedCultureTitle}['{e.CultureName}'] {notFoundTitle.ToLower()}");
        }
    }
}
