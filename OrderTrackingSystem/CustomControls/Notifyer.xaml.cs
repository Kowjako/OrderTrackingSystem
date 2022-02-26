using System;
using System.Collections.Generic;
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
    /// Interaction logic for Notifyer.xaml
    /// </summary>

    public enum NotifyType
    {
        None = 0,
        Warning = 1,
        Error = 2,
        Success = 3
    }

    public partial class Notifyer : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Notifyer), new PropertyMetadata(string.Empty));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(Notifyer), new PropertyMetadata(string.Empty));


        public NotifyType NotifyType
        {
            get { return (NotifyType)GetValue(NotifyTypeProperty); }
            set { SetValue(NotifyTypeProperty, value); }
        }

        public static readonly DependencyProperty NotifyTypeProperty =
            DependencyProperty.Register("NotifyType", typeof(NotifyType), typeof(Notifyer), new PropertyMetadata(NotifyType.None));


        public Notifyer()
        {
            InitializeComponent();
            DataContext = this;
        }
    }

}
