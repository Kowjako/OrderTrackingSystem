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
    /// <summary>
    /// Interaction logic for ExtensibleViewPanel.xaml
    /// </summary>
    public partial class ExtensibleViewPanel : UserControl, INotifyPropertyChanged
    {
        #region Dependency properties

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set
            {
                SetValue(CaptionProperty, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Caption)));
            }
        }

        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption",
                typeof(string),
                typeof(ExtensibleViewPanel),
                new FrameworkPropertyMetadata("Hello"));



        public object ViewContent
        {
            get { return (object)GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }


        public static readonly DependencyProperty ViewContentProperty =
            DependencyProperty.Register("ViewContent", 
                typeof(object), 
                typeof(ExtensibleViewPanel), 
                new PropertyMetadata(null));



        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public ExtensibleViewPanel()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
