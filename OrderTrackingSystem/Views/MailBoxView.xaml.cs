using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MailBoxView.xaml
    /// </summary>
    public partial class MailBoxView : UserControl
    {
        public MailBoxView()
        {
            InitializeComponent();
        }

        private void sentGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void receiveGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void orderLinker_ClickAddOrder(object sender, RoutedEventArgs e)
        {
            (DataContext as MailboxViewModel).OnLinkToOrderAdded();
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            var selectedToggle = e.OriginalSource as ToggleButton;
            switch (selectedToggle.Name)
            {
                case "sentMsg":
                    (DataContext as MailboxViewModel).SelectedFilterMsgType = 0;
                    break;
                case "receivedMsg":
                    (DataContext as MailboxViewModel).SelectedFilterMsgType = 1;
                    break;
                default:
                    break;
            }
        }
    }
}
