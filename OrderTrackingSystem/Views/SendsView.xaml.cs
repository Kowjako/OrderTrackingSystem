using OrderTrackingSystem.CustomControls;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for SendsView.xaml
    /// </summary>
    public partial class SendsView : UserControl
    {
        public SendsView()
        {
            InitializeComponent();
        }

        private void elementGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void cartGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void boxPanel_BoxChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            (DataContext as SendsViewModel).BoxPrice = (e.OriginalSource as PurchaseElement).BoxPrice;
        }

        private void productsTree_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedSubCategory = (e.NewValue as TreeViewItem).Tag;
            (DataContext as SendsViewModel).ProductSubCategory = selectedSubCategory == null ? -1 : int.Parse(selectedSubCategory.ToString());
        }
    }
}
