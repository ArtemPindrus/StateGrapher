using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StateGrapher.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class NumberGreaterThanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            value is int a && int.TryParse(parameter as string, out int b) && a >= b ? Visibility.Visible : Visibility.Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
