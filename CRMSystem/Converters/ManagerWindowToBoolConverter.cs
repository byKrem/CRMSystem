using CRMSystem.Views.ManagerViews;
using System.Globalization;
using System.Windows.Data;

namespace CRMSystem.Converters
{
    public class ManagerWindowToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            switch((ManagerFrameStatusEnum) parameter)
            {
                case ManagerFrameStatusEnum.PersonalAccountFrame:
                    if (value is PersonalAccountFrame)
                        return false;
                    break;
                case ManagerFrameStatusEnum.OrdersFrame:
                    if (value is OrdersFrame)
                        return false;
                    break;
                case ManagerFrameStatusEnum.StorageFrame:
                    if (value is StorageFrame)
                        return false;
                    break;
                case ManagerFrameStatusEnum.ClientsFrame:
                    if (value is ClientsFrame)
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
