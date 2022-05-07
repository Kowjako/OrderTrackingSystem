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
        }

    }
}
