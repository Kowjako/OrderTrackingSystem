using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.CustomControls
{
    public class ExtensibleViewPanel_v2 : ContentControl
    {
        /* DP to set panel header name */
        public string CaptionV2
        {
            get { return (string)GetValue(CaptionV2Property); }
            set { SetValue(CaptionV2Property, value); }
        }

        public static readonly DependencyProperty CaptionV2Property =
            DependencyProperty.Register("CaptionV2",
                typeof(string),
                typeof(ExtensibleViewPanel_v2),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));



        static ExtensibleViewPanel_v2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtensibleViewPanel_v2),
                   new FrameworkPropertyMetadata(typeof(ExtensibleViewPanel_v2)));
        }
    }
}
