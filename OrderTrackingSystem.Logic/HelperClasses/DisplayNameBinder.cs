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

            if(CheckCustomAttributes(e.PropertyDescriptor) is string s && !string.IsNullOrEmpty(s) &&  s.Equals("Image"))
            {
                /* Tworzymy styl */
                var style = new Style { TargetType = typeof(DataGridCell) };
                var frameworkElementFactory = new FrameworkElementFactory(typeof(Image));
                frameworkElementFactory.SetValue(Image.HeightProperty, 60.0);
                frameworkElementFactory.SetValue(Image.WidthProperty, 100.0);
                frameworkElementFactory.SetValue(Image.StretchProperty, Stretch.Fill);

                /* Wiązanie danych do DTO */
                var binding = new Binding("Image");
                binding.Mode = BindingMode.OneWay;
                frameworkElementFactory.SetValue(Image.SourceProperty, binding);

                var dataTemplate = new DataTemplate();
                dataTemplate.VisualTree = frameworkElementFactory;

                /* Tworzenie triggerów */
                var trigger = new Trigger();
                trigger.Property = DataGridCell.IsSelectedProperty;
                trigger.Value = true;
                trigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromRgb(222, 222, 222))));
                trigger.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(222, 222, 222))));

                var trigger1 = new Trigger();
                trigger1.Property = DataGridCell.IsFocusedProperty;
                trigger1.Value = true;
                trigger1.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new SolidColorBrush(Colors.Black)));

                var trigger2 = new Trigger();
                trigger2.Property = DataGridCell.IsMouseOverProperty;
                trigger2.Value = true;
                trigger2.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromRgb(240, 240, 240))));

                /* Wypełnianie stylu */
                style.Setters.Add(new Setter(DataGridCell.ContentTemplateProperty, dataTemplate));
                style.Triggers.Add(trigger);
                style.Triggers.Add(trigger1);
                style.Triggers.Add(trigger2);

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
