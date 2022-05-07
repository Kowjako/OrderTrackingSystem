using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace OrderTrackingSystem.Presentation.Views.Customer
{
    /// <summary>
    /// Interaction logic for TrackingView.xaml
    /// </summary>
    public partial class TrackingView : UserControl
    {
        public TrackingView()
        {
            InitializeComponent();
        }

        private void elementGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void filterChecked(object sender, RoutedEventArgs e)
        {
            var selectedToggle = e.OriginalSource as ToggleButton;

            (DataContext as TrackingViewModel).ItemsSelection = selectedToggle.Name switch
            {
                "All" => 0,
                "OnlyOrders" => 1,
                "OnlySends" => 2,
                _ => -1
            };
        }

        private async void trackView_Loaded(object sender, RoutedEventArgs e)
        {
            elementGrid.MaxHeight = elementGrid.ActualHeight;
            await (DataContext as TrackingViewModel).SetInitializeProperties();
            (DataContext as TrackingViewModel).ShowProgressBar += ShowBar;
        }

        private void ShowBar()
        {
            var trackerAnimation = new DoubleAnimation();
            //trackerAnimation.From = 0;
            trackerAnimation.To = 340;
            trackerAnimation.Duration = TimeSpan.FromSeconds(1);
            trackerAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            tracker.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);

            var gridAnimation = new ThicknessAnimation();
            //gridAnimation.From = new Thickness(0, 0, 0, 0);
            gridAnimation.To = new Thickness(0, 0, 340, 0);
            gridAnimation.Duration = TimeSpan.FromSeconds(1);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            elementGrid.BeginAnimation(MarginProperty, gridAnimation);
        }
    }
}
