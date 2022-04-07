using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Controls;

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
        }

        public static string GetPropertyDisplayName(object descriptor)
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
