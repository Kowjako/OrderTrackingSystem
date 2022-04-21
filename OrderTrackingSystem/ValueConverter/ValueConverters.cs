﻿using System;
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
            if (string.IsNullOrEmpty(value.ToString())) return 0;
            byte decimalValue = byte.Parse(value.ToString());
            if (decimalValue != 0)
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

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (value as bool?) switch
        {
            true => Visibility.Visible,
            false => Visibility.Collapsed,
            _ => Visibility.Visible
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (value as Visibility?) switch
        {
            Visibility.Visible => true,
            Visibility.Collapsed => false,
            _ => true
        };

    }
}
