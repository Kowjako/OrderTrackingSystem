using OrderTrackingSystem.Presentation.CustomControls;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels;
using OrderTrackingSystem.Presentation.WindowExtension;
using OrderTrackingSystem.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

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
            DataContext = new StartupViewModel();
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
            var viewModel = new CurrentAccountViewModel();
            AttachEventsToViewModel(viewModel);
            await viewModel.SetInitializeProperties();
            DataContext = viewModel;
        }

        private async void trackingMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var viewModel = new TrackingViewModel();
            AttachEventsToViewModel(viewModel);
            await viewModel.SetInitializeProperties();
            DataContext = viewModel;
        }

        private async void OrdersMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new OrdersViewModel();
            AttachEventsToViewModel(viewModel);
            await viewModel.SetInitializeProperties();
            DataContext = viewModel;
        }

        private async void SendsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new SendsViewModel();
            AttachEventsToViewModel(viewModel);
            await viewModel.SetInitializeProperties();
            DataContext = viewModel;
        }

        private async void MailboxMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var viewModel = new MailboxViewModel();
            AttachEventsToViewModel(viewModel);
            await viewModel.SetInitializeProperties();
            DataContext = viewModel;
        }

        private async void ComplaintsMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataContext = new ComplaintsViewModel();
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
            DataContext = new StartupViewModel();
        }

        #endregion

    }
}
