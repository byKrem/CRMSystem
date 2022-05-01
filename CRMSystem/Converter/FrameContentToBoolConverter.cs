using CRMSystem.View.ManagerViews;
using System;
using System.Globalization;
using System.Windows.Data;

namespace CRMSystem.Converter
{
    public class FrameContentToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            switch((FrameContentEnum) parameter)
            {
                case FrameContentEnum.PersonalAccountFrame:
                    if (value?.GetType() == typeof(PersonalAccountFrame))
                        return false;
                    break;
                case FrameContentEnum.OrdersFrame:
                    if (value?.GetType() == typeof(OrdersFrame))
                        return false;
                    break;
                case FrameContentEnum.StorageFrame:
                    if (value?.GetType() == typeof(StorageFrame))
                        return false;
                    break;
                case FrameContentEnum.ClientsFrame:
                    if (value?.GetType() == typeof(ClientsFrame))
                        return false;
                    break;
            }
            return true;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
