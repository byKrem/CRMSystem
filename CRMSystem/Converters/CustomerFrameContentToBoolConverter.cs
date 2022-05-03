using System;
using System.Globalization;
using System.Windows.Data;
using CRMSystem.Views.CustomerViews;

namespace CRMSystem.Converters
{
    public class CustomerFrameContentToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType,
            object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            switch((CustomerFrameContentType)parameter)
            {
                case CustomerFrameContentType.CatalogFrame:
                    if (value.GetType() == typeof(CatalogFrame))
                        return false;

                    break;
                case CustomerFrameContentType.CartFrame:
                    if (value.GetType() == typeof(CartFrame))
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
