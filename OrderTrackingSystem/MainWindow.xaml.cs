using OrderTrackingSystem.Presentation.CustomControls;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.ViewModels.Seller;
using OrderTrackingSystem.Presentation.WindowExtension;
using OrderTrackingSystem.Presentation;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using OrderTrackingSystem.ViewModels;

namespace OrderTrackingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMenuExpanded;

        private bool _isAppForClient = true;
        public bool IsAppForClient
        {
            get => _isAppForClient;
            set
            {
                _isAppForClient = value;
                if(_isAppForClient)
                {
                    SellerMenu.Visibility = Visibility.Collapsed;
                    SellerData.Visibility = Visibility.Collapsed;
                    SellerProcess.Visibility = Visibility.Collapsed;
                }
                else
                {
                    accountMenu.Visibility = Visibility.Collapsed;
                    trackingMenu.Visibility = Visibility.Collapsed;
                    OrdersMenu.Visibility = Visibility.Collapsed;
                    SendsMenu.Visibility = Visibility.Collapsed;
                    MailboxMenu.Visibility = Visibility.Collapsed;
                    ComplaintsMenu.Visibility = Visibility.Collapsed;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new StartupViewModel();
        }

        #region Common behaviour

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        #pragma warning disable IDE1006 
        private void exit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void fullScreenBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            MaxHeight = SystemParameters.WorkArea.Height;
            fullScreenBtn.Content = WindowState == WindowState.Maximized ? Presentation.Properties.Resources.ResourceManager.GetString("EnableFullscreen", CultureInfo.CurrentCulture)
                                                                         : Presentation.Properties.Resources.ResourceManager.GetString("DisableFullscreen", CultureInfo.CurrentCulture);
            exit.Content = Presentation.Properties.Resources.ResourceManager.GetString("Exit", CultureInfo.CurrentCulture);
            
        }

        private void menuExpander_Click(object sender, RoutedEventArgs e)
        {
            var trackerAnimation = new DoubleAnimation()
            {
                From = isMenuExpanded ? 0 : 70,
                To = isMenuExpanded ? 70 : 0,
                Duration = TimeSpan.FromSeconds(2),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut }
            };
            tabControl.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);
            isMenuExpanded = !isMenuExpanded;
        }

        #endregion

        #region DataContext

        private void TabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new CurrentAccountViewModel();
            AttachEventsToViewModel(viewModel);           
            DataContext = viewModel;
        }

        private void trackingMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new TrackingViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void OrdersMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new OrdersViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void SendsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new SendsViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void MailboxMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new MailboxViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void ComplaintsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new ComplaintsViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void SellerMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new DesktopViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void SellerData_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new SellerAccountViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        private void SellerProcess_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new SellerProcessesViewModel();
            AttachEventsToViewModel(viewModel);
            DataContext = viewModel;
        }

        #endregion

        #region Generic viewmodel methods

        private void AttachEventsToViewModel<T>(T viewModel) where T : INotifyableViewModel
        {
            viewModel.OnFailure += NotifyError;
            viewModel.OnSuccess += NotifySuccess;
            viewModel.OnWarning += NotifyWarning;
        }

        #endregion

        #region Notifyable methods

        private void NotifyError(string message)
        {
            (this as ContentControl).WithNotifying(NotifyType.Error, mainControl, message);
        }

        private void NotifyWarning(string message)
        {
            (this as ContentControl).WithNotifying(NotifyType.Warning, mainControl, message);
        }

        private void NotifySuccess(string message)
        {
            (this as ContentControl).WithNotifying(NotifyType.Success, mainControl, message);
        }

        #endregion

        #region Localization - Change App Language

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cultureName = (e.OriginalSource as Button).Tag as string;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
            DataContext = new StartupViewModel();
        }


        #endregion

    }
}
