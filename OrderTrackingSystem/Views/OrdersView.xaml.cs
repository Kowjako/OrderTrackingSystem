using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }

        private void elementGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void pickupsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void cartGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private async void ordersView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cartGrid.MaxHeight = cartGrid.ActualHeight;
            elementGrid.MaxHeight = elementGrid.ActualHeight;
            pickupsGrid.MaxHeight = pickupsGrid.ActualHeight;
            await (DataContext as OrdersViewModel).SetInitializeProperties();
        }
    }
}
