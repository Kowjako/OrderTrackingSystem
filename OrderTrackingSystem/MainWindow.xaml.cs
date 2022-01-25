using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace OrderTrackingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Common behaviour

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ListBoxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void fullScreenBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            MaxHeight = SystemParameters.WorkArea.Height;
            fullScreenBtn.Content = WindowState == WindowState.Maximized ? "Wyjdz z Full Screen" : "Włącz Full Screen";
        }

        #endregion

        #region DataContext

        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new CurrentAccountViewModel();
        }

        private void trackingMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new TrackingViewModel();
        }

        private void OrdersMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new OrdersViewModel();
        }

        private void SendsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new SendsViewModel();
        }

        private void MailboxMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new MailboxViewModel();
        }


        #endregion
        
    }
}
