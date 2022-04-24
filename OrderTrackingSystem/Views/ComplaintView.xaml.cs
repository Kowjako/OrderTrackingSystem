using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for ComplaintView.xaml
    /// </summary>
    public partial class ComplaintView : UserControl
    {
        public ComplaintView()
        {
            InitializeComponent();

        }

        private async void complaintsView_Loaded(object sender, RoutedEventArgs e)
        {
            complaintsFolders.MaxHeight = complaintsFolders.ActualHeight;
            //elementGrid.MaxHeight = elementGrid.ActualHeight;
            await (DataContext as ComplaintsViewModel).SetInitializeProperties();
        }

        private void complaintsFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (DataContext as ComplaintsViewModel).SelectedFolder = e.NewValue as ComplaintFolderDTO;
        }

        private void elementGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void complaintDefinitionsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private void StackPanel_Checked(object sender, RoutedEventArgs e)
        {
            var selectedToggle = e.OriginalSource as ToggleButton;

            (DataContext as ComplaintsViewModel).SelectedFolderDeleteType = selectedToggle.Name switch
            {
                "alsoFromFolder" => 0,
                "moveToParent" => 1,
                _ => 2
            };
        }
    }
}
