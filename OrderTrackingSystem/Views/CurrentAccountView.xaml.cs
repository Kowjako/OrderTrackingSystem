using OrderTrackingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CurrentAccountView : UserControl
    {
        public CurrentAccountView()
        {
            InitializeComponent();
            Loaded += CurrentAccountView_Loaded;
            dgLocalization.SourceUpdated += DgLocalization_SourceUpdated;
            DataContextChanged += CurrentAccountView_DataContextChanged;
            dgLocalization.Columns.CollectionChanged += Columns_CollectionChanged;
        }

        private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (dgLocalization.Columns.Count != 0)
                dgLocalization.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void DgLocalization_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (dgLocalization.Columns.Count != 0)
                dgLocalization.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void CurrentAccountView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (dgLocalization.Columns.Count != 0)
                dgLocalization.Columns[0].IsReadOnly = true;
            dgLocalization.InvalidateVisual();
        }

        private void CurrentAccountView_Initialized(object sender, EventArgs e)
        {
            dgLocalization.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void CurrentAccountView_Loaded(object sender, RoutedEventArgs e)
        {
            /* Hide id column */
            dgLocalization.Columns[0].Visibility = Visibility.Collapsed;
        }
    }
}
