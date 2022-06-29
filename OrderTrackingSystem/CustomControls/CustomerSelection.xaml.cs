using OrderTrackingSystem.Presentation.CustomControls.Classes;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomerSelection.xaml
    /// </summary>
    public partial class CustomerSelection : UserControl
    { 
        public CustomerSelection()
        {
            InitializeComponent();
        }

        public CustomerSelection(CustomerSelectionViewModel vm) : this() /* trzeba wywolac InitializeComponent() */
        {
            DataContext = vm;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowExtension.WindowExtension.ActualPopupWindow.Close();
        }
    }
}
