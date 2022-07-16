using OrderTrackingSystem.Presentation.CustomControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderTrackingSystem.Presentation.WindowExtension
{
    public static class WindowExtension
    {
        /* Extension-method dla klasy Window dla uzycia Notifyer'a */
        public static Notifyer Notifyer = new Notifyer();
        public static Window ActualPopupWindow = null;

        public static void WithNotifying<T>(this T window, NotifyType type, FrameworkElement mainContentControl, string msg) where T : ContentControl
        {
            Notifyer.ShowNotifyer(type, mainContentControl, msg);
        }

        public static void ShowControl(this UserControl control)
        {
            var window = new Window()
            {
                WindowStyle = WindowStyle.None,
                Content = control,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0),
                AllowsTransparency = true,
                Background = new SolidColorBrush(Colors.Transparent),
                SizeToContent = SizeToContent.Manual,
                ResizeMode = ResizeMode.NoResize,
                Width = Application.Current.MainWindow.Width, /* to pozwala zrobic kontrolke na wielkosc aplikacji i pokazac cien */
                Height = Application.Current.MainWindow.Height
            };
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            ActualPopupWindow = window;

            window.ShowDialog();
        }
    }
}
