using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MailBoxView.xaml
    /// </summary>
    public partial class MailBoxView : UserControl
    {
        static List<Popup> popups = new List<Popup>();
        public MailBoxView()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WindowExtension.WindowExtension.WithNotifying(this, CustomControls.NotifyType.Success, mainGrid);
        }
    }
}
