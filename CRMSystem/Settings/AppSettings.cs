using System;
using System.IO;
using System.Windows;
using RIS.Settings;
using RIS.Settings.Ini;
using Environment = RIS.Environment;

namespace CRMSystem.Settings
{
    public sealed class AppSettings : IniSettings
    {
        public const string SettingsFileName = "AppSettings.config";



        [SettingCategory("Localization")]
        public string Language { get; set; }

        [SettingCategory("LoginWindow")]
        public double LoginWindowWidth { get; set; }
        public double LoginWindowHeight { get; set; }
        public double LoginWindowPositionX { get; set; }
        public double LoginWindowPositionY { get; set; }
        [DefaultSettingValue(WindowState.Normal)]
        public WindowState LoginWindowState { get; set; }

        [SettingCategory("ManagerWindow")]
        public double ManagerWindowWidth { get; set; }
        public double ManagerWindowHeight { get; set; }
        public double ManagerWindowPositionX { get; set; }
        public double ManagerWindowPositionY { get; set; }
        [DefaultSettingValue(WindowState.Normal)]
        public WindowState ManagerWindowState { get; set; }

        [SettingCategory("CustomerWindow")]
        public double CustomerWindowWidth { get; set; }
        public double CustomerWindowHeight { get; set; }
        public double CustomerWindowPositionX { get; set; }
        public double CustomerWindowPositionY { get; set; }
        [DefaultSettingValue(WindowState.Normal)]
        public WindowState CustomerWindowState { get; set; }

        [SettingCategory("Log")]
        public int LogRetentionDaysPeriod { get; set; }



        public AppSettings()
            : base(Path.Combine(Environment.ExecProcessDirectoryName, SettingsFileName))
        {
            Language = "en-US";

            LoginWindowWidth = 300;
            LoginWindowHeight = 450;
            LoginWindowPositionX = SystemParameters.PrimaryScreenWidth / 2.0;
            LoginWindowPositionY = SystemParameters.PrimaryScreenHeight / 2.0;
            LoginWindowState = WindowState.Normal;

            ManagerWindowWidth = 800;
            ManagerWindowHeight = 450;
            ManagerWindowPositionX = SystemParameters.PrimaryScreenWidth / 2.0;
            ManagerWindowPositionY = SystemParameters.PrimaryScreenHeight / 2.0;
            ManagerWindowState = WindowState.Normal;

            CustomerWindowWidth = 800;
            CustomerWindowHeight = 450;
            CustomerWindowPositionX = SystemParameters.PrimaryScreenWidth / 2.0;
            CustomerWindowPositionY = SystemParameters.PrimaryScreenHeight / 2.0;
            CustomerWindowState = WindowState.Normal;

            LogRetentionDaysPeriod = 7;

            Load();
        }
    }
}
