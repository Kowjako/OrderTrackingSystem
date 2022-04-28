using OrderTrackingSystem.Presentation.ViewModels.Seller;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views.Seller
{
    /// <summary>
    /// Interaction logic for SellerAccountView.xaml
    /// </summary>
    public partial class SellerAccountView : UserControl
    {
        public SellerAccountView()
        {
            InitializeComponent();
        }

        private void dgLocalization_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private async void sellerAccountView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            complaintsGrid.MaxHeight = complaintsGrid.ActualHeight;
            ordersGrid.MaxHeight = ordersGrid.ActualHeight;
            dgLocalization.MaxHeight = dgLocalization.ActualHeight;
            await (DataContext as SellerAccountViewModel).SetInitializeProperties();
        }
    }
}
