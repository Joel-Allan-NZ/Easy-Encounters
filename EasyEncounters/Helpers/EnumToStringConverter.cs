using Microsoft.UI.Xaml.Data;

namespace EasyEncounters.Helpers;

public class EnumToStringConverter : IValueConverter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="value">The type of enumeration</param>
    /// <param name="targetType"></param>
    /// <param name="parameter">the value of the enumeration (as a string)</param>
    /// <param name="language"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    { //converting to string: should be trivial
        try
        {
            if (value is Enum enumvalue)
            {
                return ResourceExtensions.GetEnumerationString(enumvalue);
            }
        }
        catch
        {
            return "Unknown Value";
        }
        throw new ArgumentException($"{value} is not a supported type.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}