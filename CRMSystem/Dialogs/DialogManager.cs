using System;
using System.Windows;
using CRMSystem.Utils;
using RIS;
using RIS.Logging;

namespace CRMSystem.Dialogs
{
    public static class DialogManager
    {
        public static MessageBoxResult ShowDialog(
            string title, string message,
            MessageBoxButton buttons = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            MessageBoxResult defaultResult = MessageBoxResult.OK)
        {
            LogManager.Default.Info($"{title} - {message}");

            return MessageBox.Show(
                message, title,
                buttons, image,
                defaultResult);
        }



        public static MessageBoxResult ShowConfirmationDialog(
            string additionalMessage = null)
        {
            if (!LocalizationUtils.TryGetLocalized("ConfirmationTitle", out var title))
                title = "Confirmation";
            if (!LocalizationUtils.TryGetLocalized("ConfirmationMessage", out var message))
                message = "Are you sure?";

            if (!string.IsNullOrEmpty(additionalMessage))
                message += " " + additionalMessage;

            return ShowDialog(title, message,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);
        }

        public static MessageBoxResult ShowErrorDialog(
            string message)
        {
            if (!LocalizationUtils.TryGetLocalized("ErrorTitle", out var title))
                title = "Error";

            Events.OnError(new RErrorEventArgs(
                $"{title} - {message}"));

            return ShowDialog(title, message,
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK);
        }

        public static MessageBoxResult ShowSuccessDialog(
            string message)
        {
            if (!LocalizationUtils.TryGetLocalized("SuccessTitle", out var title))
                title = "Success";

            return ShowDialog(title, message,
                MessageBoxButton.OK,
                MessageBoxImage.Information,
                MessageBoxResult.OK);
        }
    }
}
