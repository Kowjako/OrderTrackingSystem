﻿using OrderTrackingSystem.Presentation.WindowExtension;
using OrderTrackingSystem.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

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
        }

        private void sellsGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DisplayNameBinder.SetDisplayNameIfExists(e);
        }

        private async void main_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            sellsGrid.MaxHeight = sellsGrid.ActualHeight;
            await (DataContext as CurrentAccountViewModel).SetInitializeProperties();
        }
    }
}
