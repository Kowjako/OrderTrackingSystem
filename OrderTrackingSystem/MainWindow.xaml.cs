using OrderTrackingSystem.Presentation.CustomControls;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using OrderTrackingSystem.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OrderTrackingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMenuExpanded;
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

        private void menuExpander_Click(object sender, RoutedEventArgs e)
        {
            var trackerAnimation = new DoubleAnimation();
            trackerAnimation.From = isMenuExpanded ? 0 : 70;
            trackerAnimation.To = isMenuExpanded ? 70 : 0;
            trackerAnimation.Duration = TimeSpan.FromSeconds(2);
            trackerAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            tabControl.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);
            isMenuExpanded = !isMenuExpanded;
        }

        #endregion

        #region DataContext

        private async void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                throw new Exception();
                //var viewModel = new CurrentAccountViewModel();
                //await viewModel.SetInitializeProperties();
                //DataContext = viewModel;
            }
            catch (Exception ex)
            {
                (this as ContentControl).WithNotifying(NotifyType.Error, mainControl, "Wystąpił błąd podczas pobierania danych");
            }
        }

        private async void trackingMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void ComplaintsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new ComplaintsViewModel();
        }
        #endregion

    }
}
