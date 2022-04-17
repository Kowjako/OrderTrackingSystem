using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OrderTrackingSystem.Presentation.ValueConverter
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* Pattern matching */
            if (value is decimal decimalValue && decimalValue == 0m)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString())) return 0m;
            decimal decimalValue = decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);
            if(decimalValue != 0m)
            {
                return decimalValue;
            }
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value as bool?).Value;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(value as bool?).Value;
    }

}
