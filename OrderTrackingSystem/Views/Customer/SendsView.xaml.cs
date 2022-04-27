using OrderTrackingSystem.CustomControls;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.Views.Customer
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
            (DataContext as SendsViewModel).SelectedSubCategory = e.NewValue as CategoryDTO;
        }

        private async void sendsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            elementGrid.MaxHeight = elementGrid.ActualHeight;
            productsTree.MaxHeight = productsTree.ActualHeight;
            await (DataContext as SendsViewModel).SetInitializeProperties();
        }
    }
}
