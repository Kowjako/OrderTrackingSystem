using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace OrderTrackingSystem.Presentation.ValueConverter
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class DecimalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* Pattern matching */
            if (value is decimal decimalValue) return decimalValue switch
            {
                0m => string.Empty,
                _ => decimalValue.ToString("N2")
            };
            else return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                if(decimal.TryParse(str, out decimal output))
                {
                    return Math.Round(output,2);
                }
            }
            return 0m;
        }
    }

    [ValueConversion(typeof(byte), typeof(string))]
    public class ByteToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            /* Pattern matching */
            if (value is byte decimalValue && decimalValue == 0)
                return string.Empty;
            else
                return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrEmpty(str))
            {
                var dec = byte.Parse(str);
                return dec;
            }
            return 0;
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value as bool?).Value;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(value as bool?).Value;
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as bool?) switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed,
            _ => Visibility.Visible
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as int?) switch
        {
            var x when x.HasValue && x.Value > 0 => Visibility.Visible,
            _ => Visibility.Collapsed
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    [ValueConversion(typeof(string), typeof(DateTime))]
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dt && dt == new DateTime(1, 1, 1))
            {
                return DateTime.Now;
            }
            else
                return value as DateTime?;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as DateTime?;
        }
    }

    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            null => Visibility.Collapsed,
            _ => Visibility.Visible
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
