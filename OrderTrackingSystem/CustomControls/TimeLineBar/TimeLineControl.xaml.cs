using OrderTrackingSystem.CustomControls.Classes;
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

        #region Constants

        private const double RADIUSX = 10.0;
        private const double RADIUSY = 10.0;

        #endregion

        #region Read-only fields

        private VisualBrush dashedBrush = new VisualBrush()
        {
            Visual = new Rectangle
            {
                StrokeThickness = 5,
                Stroke = new SolidColorBrush(Colors.Red),
                Height = 5,
                Width = 95.5,
                StrokeDashArray = new DoubleCollection { 1, 2 }
            }
        };

        #endregion

        public int NodeCount { get; set; } = 1;
        internal ICollection<TimeLineNode> TimeLineNodes { get; set; } = new List<TimeLineNode>();

        public TimeLineControl()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            mainContrainer.Children.Clear();
            /* Render all timeline nodes */
            for (int i = 0; i < NodeCount; i++) 
            {
                DrawEllipse(3 * i, 0);
                PlaceTitle(3 * i, 1);
                PlaceDateTime(3 * i + 1,  1);
                PlaceDescription(3* i + 2, 1);
                /* Skip drawing connector for leaf */
                if (i == NodeCount - 1) continue;
                DrawConnector(3 * i + 1, 0, i == NodeCount - 2) ;
            }
            
        }

        #region Generate components

        private void PlaceDescription(int row, int column)
        {
            var textBlock = new TextBlock
            {
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Text = "Introducing the all-new Make it Big Podcast — a thought leadership audio series for retailers, entrepreneurs and ecommerce professionals.Tune in for expert insights, strategies and tactics to help grow your business.",
                Margin = new Thickness(10, 0, 15, 0)
            };
            AddControlToMainContainer(textBlock, row, column);
            mainContrainer.Children.Add(textBlock);
        }

        private void PlaceTitle(int row, int column)
        {
            var textBlock = new TextBlock
            {
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "Złożenie zamówienia",
                Margin = new Thickness(10, 0, 15, 0)
            };
            AddControlToMainContainer(textBlock, row, column);
            mainContrainer.Children.Add(textBlock);
        }

        private void DrawEllipse(int row, int column)
        {
            var ellipse = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.White),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2,
                Height = RADIUSY * 2,
                Width = RADIUSX * 2
            };
            AddControlToMainContainer(ellipse, row, column);
            mainContrainer.Children.Add(ellipse);
        }

        private void PlaceDateTime(int row, int column)
        {
            var textBlock = new TextBlock
            {
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Center,
                Text = "2021-15-10 18:56",
                Margin = new Thickness(10, 0, 15, 0)
            };
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

        private void DrawConnector(int row, int column, bool lastNode = false)
        {
            Brush usedBrush = new SolidColorBrush(Colors.Black);
            if(lastNode)
            {
                usedBrush = dashedBrush;
            }
            var panel = new Border
            {
                Height = 2,
                LayoutTransform = new RotateTransform(-90.0),
                Background = usedBrush
            };

            AddControlToMainContainer(panel, row, column);
            Grid.SetRowSpan(panel, 2);
            mainContrainer.Children.Add(panel);
        }

        #endregion

        private void AddControlToMainContainer(UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
        }
    }
}
