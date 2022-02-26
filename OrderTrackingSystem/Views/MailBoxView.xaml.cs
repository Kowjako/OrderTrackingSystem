using OrderTrackingSystem.Presentation.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var popup = new Popup();
            popup.Width = 400;
            popup.Child = new Notifyer();
            popup.AllowsTransparency = true;
            popup.PopupAnimation = PopupAnimation.Slide;
            popup.PlacementTarget = mainGrid;
            popup.Placement = PlacementMode.Right;
            popup.IsOpen = true;
            popup.StaysOpen = false;
            popup.HorizontalOffset = -(popup.Child as Notifyer).ActualWidth - 15;
            popup.VerticalOffset = 10;
            popups.Add(popup);

        }
    }
}
