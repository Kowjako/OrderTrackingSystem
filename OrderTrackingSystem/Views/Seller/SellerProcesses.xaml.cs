using OrderTrackingSystem.Presentation.ViewModels.Seller;
using System.Windows;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views.Seller
{
    /// <summary>
    /// Interaction logic for SellerProcesses.xaml
    /// </summary>
    public partial class SellerProcesses : UserControl
    {
        public SellerProcesses()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as SellerProcessesViewModel).SetInitializeProperties();
        }
    }
}
