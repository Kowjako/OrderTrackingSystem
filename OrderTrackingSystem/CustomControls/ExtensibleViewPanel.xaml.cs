using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Presentation.CustomControls
{
    public partial class ExtensibleViewPanel : ContentControl
    {
        #region Dependency properties

        /* DP to set panel header name */
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption",
                typeof(string),
                typeof(ExtensibleViewPanel),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));


        /* DP to set panel content */
        public object ViewContent
        {
            get { return (object)GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }


        public static readonly DependencyProperty ViewContentProperty =
            DependencyProperty.Register("ViewContent", 
                typeof(object), 
                typeof(ExtensibleViewPanel), 
                new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender));


        #endregion

        public ExtensibleViewPanel()
        {
            InitializeComponent();
        }
    }
}
