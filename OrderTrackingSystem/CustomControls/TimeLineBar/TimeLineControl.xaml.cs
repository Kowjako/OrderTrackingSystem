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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTrackingSystem.CustomControls.TimeLineBar
{
    /// <summary>
    /// Interaction logic for TimeLineControl.xaml
    /// </summary>
    public partial class TimeLineControl : UserControl
    {
        private const double RADIUSX = 10.0;
        private const double RADIUSY = 10.0;
        private readonly Point CENTER = new Point(25, 25);

        public TimeLineControl()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            DrawEllipse();
            PlaceTitle();
            PlaceDateTime();
            PlaceDescription();
        }

        private void PlaceDescription()
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 15;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = "Introducing the all-new Make it Big Podcast — a thought leadership audio series for retailers, entrepreneurs and ecommerce professionals.Tune in for expert insights, strategies and tactics to help grow your business.";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            Grid.SetColumn(textBlock, 1);
            Grid.SetRow(textBlock, 2);
            mainContrainer.Children.Add(textBlock);
        }

        private void PlaceTitle()
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 15;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = "Złożenie zamówienia";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            Grid.SetColumn(textBlock, 1);
            Grid.SetRow(textBlock, 0);
            mainContrainer.Children.Add(textBlock);
        }

        private void DrawEllipse()
        {
            var ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.White);
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.StrokeThickness = 2;
            ellipse.Height = RADIUSY * 2;
            ellipse.Width = RADIUSX * 2;
            Grid.SetColumn(ellipse, 0);
            Grid.SetRow(ellipse, 0);
            mainContrainer.Children.Add(ellipse);
            PlaceTitle();
        }

        private void PlaceDateTime()
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 12;
            textBlock.Foreground = new SolidColorBrush(Colors.Gray);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = "2021-15-10 18:56";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            Grid.SetColumn(textBlock, 1);
            Grid.SetRow(textBlock, 1);
            mainContrainer.Children.Add(textBlock);
        }

        private void AddNode(string title, DateTime date, string description)
        {
            throw new NotImplementedException();
        }

    }
}
