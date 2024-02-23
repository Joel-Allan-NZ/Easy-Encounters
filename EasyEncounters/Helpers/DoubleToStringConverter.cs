using Microsoft.UI.Xaml.Data;

namespace EasyEncounters.Helpers
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                var number = (double)value;
                string ans = number.ToString();

                if (number < 1 && number > 0)
                {
                    if (number == 0.25)
                        ans = "1/4";
                    else if (number == .125)
                        ans = "1/8";
                    else if (number == 0.5)
                        ans = "1/2";
                }
                return ans;
            }
            throw new ArgumentException("Must be a double");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
    }
}