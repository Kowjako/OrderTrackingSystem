using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace OrderTrackingSystem.Presentation.Views
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

        private void btnShowProgress_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as TrackingViewModel).SelectedItem !=null &&
                !(DataContext as TrackingViewModel).SelectedItem.IsOrder) return;

            var trackerAnimation = new DoubleAnimation();
            trackerAnimation.From = 0;
            trackerAnimation.To = 340;
            trackerAnimation.Duration = TimeSpan.FromSeconds(2);
            trackerAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            tracker.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);

            var gridAnimation = new ThicknessAnimation();
            gridAnimation.From = new Thickness(0, 0, 0, 0);
            gridAnimation.To = new Thickness(0, 0, 340, 0);
            gridAnimation.Duration = TimeSpan.FromSeconds(2);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            elementGrid.BeginAnimation(MarginProperty, gridAnimation);
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

        private void hideProgress_Click(object sender, RoutedEventArgs e)
        {
            var trackerAnimation = new DoubleAnimation();
            trackerAnimation.From = 340;
            trackerAnimation.To = 0;
            trackerAnimation.Duration = TimeSpan.FromSeconds(2);
            trackerAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            tracker.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);

            var gridAnimation = new ThicknessAnimation();
            gridAnimation.From = new Thickness(0, 0, 340, 0);
            gridAnimation.To = new Thickness(0, 0, 0, 0);
            gridAnimation.Duration = TimeSpan.FromSeconds(2);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            elementGrid.BeginAnimation(MarginProperty, gridAnimation);
        }
    }
}
