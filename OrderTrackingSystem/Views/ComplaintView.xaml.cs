﻿using System;
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

        private void complaintsView_Loaded(object sender, RoutedEventArgs e)
        {
            complaintFolders.MaxHeight = complaintFolders.ActualHeight;
            elementGrid.MaxHeight = elementGrid.ActualHeight;
        }
    }
}
