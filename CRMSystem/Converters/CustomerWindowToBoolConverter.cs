using CRMSystem.Views.CustomerViews;
using System.Globalization;
using System.Windows.Data;

namespace CRMSystem.Converters
{
    internal class CustomerWindowToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CustomerFrameStatusEnum) parameter)
            {
                case CustomerFrameStatusEnum.PersonalAccountFrame:
                    if (value is PersonalAccountFrame)
                        return false;
                    break;
                case CustomerFrameStatusEnum.CartFrame:
                    if (value is CartFrame)
                        return false;
                    break;
                case CustomerFrameStatusEnum.CatalogFrame:
                    if (value is CatalogFrame)
                        return false;
                    break;
            }
            return true;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
