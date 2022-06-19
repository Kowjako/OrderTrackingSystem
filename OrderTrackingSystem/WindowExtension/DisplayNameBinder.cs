using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace OrderTrackingSystem.Presentation.WindowExtension
{
    public class DisplayNameBinder
    {
        public static void SetDisplayNameIfExists(DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            /* Hide non-browsable columns */
            if (((PropertyDescriptor)e.PropertyDescriptor).IsBrowsable == false)
                e.Cancel = true;

            /* Set display name attribute value to column */
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

            if(e.Column is DataGridBoundColumn dgbc && dgbc != null)
            {
                var binding = dgbc.Binding;
                if(binding != null && binding.StringFormat == null)
                {
                    binding.StringFormat = CheckCustomAttributes(e.PropertyDescriptor);
                }
            }

            if (e.Column is DataGridCheckBoxColumn)
            {
                var style = new Style { TargetType = typeof(DataGridCell) };
                style.Setters.Add(new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Left));
                style.Setters.Add(new Setter(DataGridCell.MarginProperty, new Thickness(15, 0, 0, 0)));
                e.Column.CellStyle = style;
            }

            if (CheckCustomAttributes(e.PropertyDescriptor) is string s && !string.IsNullOrEmpty(s) && s.Equals("Image"))
            {
                var style = new ResourceDictionary() { Source = new Uri("/OrderTrackingSystem.Presentation;component/Styles/DefinedStyles.xaml", UriKind.RelativeOrAbsolute) }["imageCell"] as Style;
                e.Column.CellStyle = style;
            }
        }

        private static string GetPropertyDisplayName(object descriptor)
        {
            if (descriptor is PropertyDescriptor pd && pd != null)
            {
                if (pd.Attributes[typeof(DisplayAttribute)] is  DisplayAttribute dn && dn != null)
                {
                    return Logic.Properties.Resources.ResourceManager.GetString(dn.Name, System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    var attributes = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        if (attributes[i] is DisplayAttribute dn && dn != null)
                        {
                            return Logic.Properties.Resources.ResourceManager.GetString(dn.Name, System.Globalization.CultureInfo.CurrentCulture);
                        }
                    }
                }
            }
            return null;
        }

        private static string CheckCustomAttributes(object descriptor)
        {
            if (descriptor is PropertyDescriptor pi && pi != null)
            {
                if (pi.Attributes[typeof(UKAttribute)] is UKFormatAttribute fattribute && fattribute != null)
                {
                    return fattribute.GetStringFormat();
                }

                if(pi.Attributes[typeof(UKAttribute)] is UKAttribute attribute && attribute != null)
                {
                    return attribute.PropertyName;
                }
            }
            return null;
        }
    }
}
