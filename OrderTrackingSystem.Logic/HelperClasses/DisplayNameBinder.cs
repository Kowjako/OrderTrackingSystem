using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

            ///* Set overrided celltemplate for image */
            //if(e.PropertyName.Equals("Image"))
            //{
            //    /* Tworzymy styl dla komorki */
            //    var style = new Style();
            //    style.TargetType = typeof(DataGridCell);

            //    /* Tworzymy Image do wrzucenia do Content */
            //    FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
            //    factory1.SetValue(Image.HeightProperty, 80.0);
            //    factory1.SetValue(Image.WidthProperty, 120.0);
            //    factory1.SetValue(Image.StretchProperty, Stretch.Fill);
            //    Binding b1 = new Binding("Image");
            //    b1.Mode = BindingMode.TwoWay;
            //    b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //    factory1.SetValue(Image.SourceProperty, b1);
            //    DataTemplate cellTemplate1 = new DataTemplate();
            //    cellTemplate1.VisualTree = factory1;
            //    style.Setters.Add(new Setter(DataGridCell.ContentTemplateProperty, cellTemplate1));

            //    var trigger = new Trigger();
            //    trigger.Property = DataGridCell.IsSelectedProperty;
            //    trigger.Value = true;
            //    trigger.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(Color.FromRgb(222,222,222))));
            //    trigger.Setters.Add(new Setter(DataGridCell.BorderBrushProperty, new SolidColorBrush(Color.FromRgb(222, 222, 222))));
            //    style.Triggers.Add(trigger);

            //    e.Column.CellStyle = style;
            //}
        }

        private static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                DisplayAttribute dn = pd.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                if (dn != null)
                {
                    return Logic.Properties.Resources.ResourceManager.GetString(dn.Name, System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayAttribute dn = attributes[i] as DisplayAttribute;
                        if (dn != null)
                        {
                            return Logic.Properties.Resources.ResourceManager.GetString(dn.Name, System.Globalization.CultureInfo.CurrentCulture);
                        }
                    }
                }
            }
            return null;
        }
    }
}
