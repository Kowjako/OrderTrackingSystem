using OrderTrackingSystem.Presentation.ViewModels;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Presentation.Views.Common
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new StartupViewModel();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void createAcc_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is TextBlock tb && tb.Tag.Equals("customer"))
            {
                sellerGrid.Width = 0;
                customerGrid.Width = 190;
                (DataContext as StartupViewModel).CreationForClient = true;
            }
            else
            {
                sellerGrid.Width = 190;
                customerGrid.Width = 0;
                (DataContext as StartupViewModel).CreationForClient = false;
            }
            var gridAnimation = new DoubleAnimation();
            gridAnimation.From = 800;
            gridAnimation.To = 1180;
            gridAnimation.Duration = TimeSpan.FromSeconds(2);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            logWindow.BeginAnimation(DockPanel.WidthProperty, gridAnimation);
        }

        #pragma warning disable IDE1006
        private void closeBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as StartupViewModel).Credentials[1] = (sender as PasswordBox).Password;
        }
    }
}
