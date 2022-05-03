using OrderTrackingSystem.Presentation.ViewModels.Seller;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views.Seller
{
    /// <summary>
    /// Interaction logic for DesktopView.xaml
    /// </summary>
    public partial class DesktopView : UserControl
    {
        public DesktopView()
        {
            InitializeComponent();
        }

        private void elementGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private async void desktopView_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as DesktopViewModel).SetInitializeProperties();
        }
    }
}
