using OrderTrackingSystem.Presentation.WindowExtension;
using OrderTrackingSystem.Presentation;
using System.Windows.Controls;
using System.Windows.Data;
using OrderTrackingSystem.ViewModels;

namespace OrderTrackingSystem.Views.Customer
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CurrentAccountView : UserControl
    {
        public CurrentAccountView()
        {
            InitializeComponent();
        }

        private void sellsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private async void main_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            sellsGrid.MaxHeight = sellsGrid.ActualHeight;
            ordersGrid.MaxHeight = ordersGrid.ActualHeight;
            dgLocalization.MaxHeight = dgLocalization.ActualHeight;
            await (DataContext as CurrentAccountViewModel).SetInitializeProperties();
        }
    }
}
