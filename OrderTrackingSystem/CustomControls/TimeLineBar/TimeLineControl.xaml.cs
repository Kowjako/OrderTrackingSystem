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

        public int NodeCount { get; set; } = 1;

        public TimeLineControl()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            mainContrainer.Children.Clear();
            for(int i=0;i<NodeCount;i++)
            {
                DrawEllipse(3 * i, 0);
                PlaceTitle(3 * i, 1);
                PlaceDateTime(3 * i + 1,  1);
                PlaceDescription(3* i + 2, 1);
                DrawConnector(3 * i + 1, 0);
            }
            
        }

        private void PlaceDescription(int row, int column)
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 15;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = "Introducing the all-new Make it Big Podcast — a thought leadership audio series for retailers, entrepreneurs and ecommerce professionals.Tune in for expert insights, strategies and tactics to help grow your business.";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            AddControlToMainContainer(textBlock, row, column);
            mainContrainer.Children.Add(textBlock);
        }

        private void PlaceTitle(int row, int column)
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 15;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = "Złożenie zamówienia";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            AddControlToMainContainer(textBlock, row, column);
            mainContrainer.Children.Add(textBlock);
        }

        private void DrawEllipse(int row, int column)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.White);
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.StrokeThickness = 2;
            ellipse.Height = RADIUSY * 2;
            ellipse.Width = RADIUSX * 2;
            AddControlToMainContainer(ellipse, row, column);
            mainContrainer.Children.Add(ellipse);
        }

        private void PlaceDateTime(int row, int column)
        {
            var textBlock = new TextBlock();
            textBlock.FontSize = 12;
            textBlock.Foreground = new SolidColorBrush(Colors.Gray);
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = "2021-15-10 18:56";
            textBlock.Margin = new Thickness(10, 0, 15, 0);
            AddControlToMainContainer(textBlock, row, column);
            mainContrainer.Children.Add(textBlock);
        }

        public void AddNode(string title, DateTime date, string description)
        {
            /* Add new container for next state */
            mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
            mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });
            Height *= 2;
            NodeCount++;
            InvalidateVisual();
        }

        private void DrawConnector(int row, int column)
        {
            var panel = new StackPanel();
            panel.Height = 1;
            panel.LayoutTransform = new RotateTransform(-90.0);
            panel.Background = new SolidColorBrush(Colors.Black);
            AddControlToMainContainer(panel, row, column);
            Grid.SetRowSpan(panel, 2);
            mainContrainer.Children.Add(panel);
        }

        private void AddControlToMainContainer(UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
        }

    }
}
