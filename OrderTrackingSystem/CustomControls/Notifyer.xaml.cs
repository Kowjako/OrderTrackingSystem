using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public string Title { get; set; }
        public string Caption { get; set; }
        public SolidColorBrush BackgroundColor { get; set; } = new SolidColorBrush(Colors.Gray);
        public string ImagePath { get; set; }

        public NotifyType NotifyType
        {
            get { return (NotifyType)GetValue(NotifyTypeProperty); }
            set
            {
                SetValue(NotifyTypeProperty, value);
                OnNotifyTypeUpdated();
            }
        }

        public static readonly DependencyProperty NotifyTypeProperty =
            DependencyProperty.Register("NotifyType", typeof(NotifyType), typeof(Notifyer), new FrameworkPropertyMetadata(NotifyType.None));

       
        private void OnNotifyTypeUpdated()
        {
            switch(NotifyType)
            {
                case NotifyType.Warning:
                    Title = "Ostrzeżenie!";
                    Caption = "Dane mogą być nieprawidłowe";
                    BackgroundColor.Color = Colors.Orange;
                    ImagePath = "../Images/warning.png";
                    break;
                case NotifyType.Success:
                    Title = "Sukces!";
                    Caption = "Wykonanie operacji zakończono poprawnie!";
                    BackgroundColor.Color = Colors.Chartreuse;
                    ImagePath = "../Images/ok.png";
                    break;
                case NotifyType.Error:
                    Title = "Błąd!";
                    Caption = "Operacja nie została prawidłowo wykonana";
                    BackgroundColor.Color = Colors.Crimson;
                    ImagePath = "../Images/close.png";
                    break;
                default:
                    break;
            }
        }

        public void ShowNotifyer(NotifyType type, FrameworkElement mainContainer)
        {
            NotifyType = type;
            var popup = new Popup
            {
                Width = 400,
                /* Ustawiamy Child na obecny Notifyer */
                Child = this,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Slide,
                PlacementTarget = mainContainer,
                Placement = PlacementMode.Custom,
                StaysOpen = false
            };
            popup.CustomPopupPlacementCallback = (ppSize, tgSize, pointX) =>
            {
                return new[] { new CustomPopupPlacement(new Point(mainContainer.ActualWidth - (popup.Child as Notifyer).ActualWidth - 15, 15), PopupPrimaryAxis.Horizontal) };
            };
            popup.IsOpen = true;
        }
        public Notifyer()
        {
            InitializeComponent();
            DataContext = this;
        }

    }

}
