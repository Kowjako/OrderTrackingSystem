﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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

    public partial class Notifyer : UserControl, INotifyPropertyChanged
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

        /* Implementing INotifyPropertyChanged to refresh UI bindings */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnNotifyTypeUpdated()
        {
            switch(NotifyType)
            {
                case NotifyType.Warning:
                    Title = "Ostrzeżenie!";
                    BackgroundColor.Color = Colors.Orange;
                    ImagePath = "../Images/warning.png";
                    break;
                case NotifyType.Success:
                    Title = "Sukces!";
                    BackgroundColor.Color = Colors.Chartreuse;
                    ImagePath = "../Images/ok.png";
                    break;
                case NotifyType.Error:
                    Title = "Błąd!";
                    BackgroundColor.Color = Colors.Crimson;
                    ImagePath = "../Images/close.png";
                    break;
                default:
                    break;
            }
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(ImagePath));
        }

        public void ShowNotifyer(NotifyType type, FrameworkElement mainContainer, string msg)
        {
            NotifyType = type;
            Caption = msg;
            OnPropertyChanged(nameof(Caption));
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
            /* Zwalniamy Child od MainWindow */
            popup.Closed += (sender, e) => (sender as Popup).Child = null;
            popup.IsOpen = true;
        }

        public Notifyer()
        {
            InitializeComponent();
            DataContext = this;
        }

    }

}
