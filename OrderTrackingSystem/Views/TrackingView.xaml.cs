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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Presentation.Views
{
    /// <summary>
    /// Interaction logic for TrackingView.xaml
    /// </summary>
    public partial class TrackingView : UserControl
    {
        public TrackingView()
        {
            InitializeComponent();
            Loaded += TrackingView_Loaded;
        }

        private void TrackingView_Loaded(object sender, RoutedEventArgs e)
        {
            timeLine.AddNode("W trakcie przygotowania", DateTime.Now, "Przesyłka jest przygotowywana przez producenta");
            timeLine.AddNode("Odebrana od nadawcy", DateTime.Now, "Przesyłka jest odebrana od nadawcy");
            timeLine.AddNode("Przyjęta w oddziale", DateTime.Now, "Przesyłka jest procesowana przez lokalny oddział");
            timeLine.AddNode("Wysłana z oddziału", DateTime.Now, "Przesyłka opusciła lokalny oddział");
            timeLine.AddNode("Wydana do doręczenia", DateTime.Now, "Przesyłka jest przekazana kurierowi");
            timeLine.AddNode("Gotowa do odbioru", DateTime.Now, "Przesyłka czeka na odbiór");
            timeLine.AddNode("Odebrana", DateTime.Now, "Towar został odebrany przez klienta");
        }

        private void btnShowProgress_Click(object sender, RoutedEventArgs e)
        {
            var trackerAnimation = new DoubleAnimation();
            trackerAnimation.From = 0;
            trackerAnimation.To = 340;
            trackerAnimation.Duration = TimeSpan.FromSeconds(2);
            trackerAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            tracker.BeginAnimation(DockPanel.WidthProperty, trackerAnimation);

            var gridAnimation = new ThicknessAnimation();
            gridAnimation.From = new Thickness(0, 0, 0, 0);
            gridAnimation.To = new Thickness(0, 0, 340, 0);
            gridAnimation.Duration = TimeSpan.FromSeconds(2);
            gridAnimation.EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut };
            elementGrid.BeginAnimation(MarginProperty, gridAnimation);
        }
    }
}
