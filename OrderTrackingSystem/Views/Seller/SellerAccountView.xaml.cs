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
    }
}
