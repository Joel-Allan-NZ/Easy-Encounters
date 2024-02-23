using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace EasyEncounters.Helpers;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
        object parameter, string language)
    {
        var boolValue = (bool)value;
        boolValue = (parameter != null) ? !boolValue : boolValue;
        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, string language)
    {
        throw new NotImplementedException();
    }
}