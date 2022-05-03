using System;
using System.Globalization;
using System.Windows.Data;
using CRMSystem.Views.ManagerViews;

namespace CRMSystem.Converters
{
    public class ManagerFrameContentToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            switch((ManagerFrameContentType)parameter)
            {
                case ManagerFrameContentType.PersonalAccountFrame:
                    if (value.GetType() == typeof(PersonalAccountFrame))
                        return false;

                    break;
                case ManagerFrameContentType.OrdersFrame:
                    if (value.GetType() == typeof(OrdersFrame))
                        return false;

                    break;
                case ManagerFrameContentType.StorageFrame:
                    if (value.GetType() == typeof(StorageFrame))
                        return false;

                    break;
                case ManagerFrameContentType.ClientsFrame:
                    if (value.GetType() == typeof(ClientsFrame))
                        return false;

                    break;
                default:
                    break;
            }

            return true;
        }

        public object ConvertBack(object value, System.Type targetType,
            object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
