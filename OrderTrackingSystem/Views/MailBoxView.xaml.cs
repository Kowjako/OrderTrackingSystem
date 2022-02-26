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
            var popup = new Popup
            {
                Width = 400,
                Child = new Notifyer(),
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Slide,
                PlacementTarget = mainGrid,
                Placement = PlacementMode.Custom,
                StaysOpen = false
            };
            popup.CustomPopupPlacementCallback = (ppSize, tgSize, pointX) =>
            {
                return new[] { new CustomPopupPlacement(new Point(mainGrid.ActualWidth - (popup.Child as Notifyer).ActualWidth - 15,15), PopupPrimaryAxis.Horizontal) };
            };
            popup.IsOpen = true;
        }
    }
}
