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

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void createNewAcc_Click(object sender, RoutedEventArgs e)
        {
            var gridAnimation = new DoubleAnimation();
            gridAnimation.From = 800;
            gridAnimation.To = 1180;
            gridAnimation.Duration = TimeSpan.FromSeconds(2);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            logWindow.BeginAnimation(DockPanel.WidthProperty, gridAnimation);
        }
    }
}
