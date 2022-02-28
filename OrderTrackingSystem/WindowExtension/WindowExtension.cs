using OrderTrackingSystem.Presentation.CustomControls;
using System.Windows;
using System.Windows.Controls;

namespace OrderTrackingSystem.Presentation.WindowExtension
{
    public static class WindowExtension
    {
        /* Extension-method dla klasy Window dla uzycia Notifyer'a */
        public static Notifyer Notifyer = new Notifyer();
        public static void WithNotifying<T>(this T window, NotifyType type, FrameworkElement mainContentControl, string msg) where T:UserControl
        {
            Notifyer.ShowNotifyer(type, mainContentControl, msg);
        }
    }
}
