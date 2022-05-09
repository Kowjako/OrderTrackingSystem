using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private const double COMPONENT_HEIGHT = 95;

        #endregion

        public static event Action OnNodeSetted;

        public TimeLineControl()
        {
            InitializeComponent();
            /* Podpinamy co się dzieje gdy zmieniana kolekcja node'ów */
            OnNodeSetted += AddNode;
        }

        #region Properties

        /* Dependency property żeby zrobić binding do ParcelStates z naszego
         * TrackingViewModel do tej kontrolki */
        public readonly static DependencyProperty StatesSourceProperty =
        DependencyProperty.Register(nameof(TimeLineNodes),
        typeof(IEnumerable<ParcelStateDTO>),
        typeof(TimeLineControl),
        new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, PropertyChangedCallback = CollectionChangedCallback });

        public int NodeCount => TimeLineNodes == null ? 0 : TimeLineNodes.Count();
        /* Jeżeli DependencyProperty ustawiamy na XAMLu a nie w code-behind to set i get wywoływane tak
         * że debugger nam tego nie pokaże, więc dodawać logikę do get/set nie ma sensu bo i tak się 
         * nie uruchomi gdy bindings ustawiany z poziomu XAML */
        public IEnumerable<ParcelStateDTO> TimeLineNodes
        {
            get => GetValue(StatesSourceProperty) as IEnumerable<ParcelStateDTO>;
            set => SetValue(StatesSourceProperty, value);
        }

        public static void CollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OnNodeSetted?.Invoke();
        }

        #endregion

        #region Generate components

        private void PlaceDescription(int row, int column, string description)
        {
            var textBlock = new TextBlock
            {
                FontSize = 13,
                TextWrapping = TextWrapping.Wrap,
                Text = description,
                Margin = new Thickness(10, 0, 15, 0)
            };
            AddControlToMainContainer(textBlock, row, column);
            
        }

        private void PlaceTitle(int row, int column, string title)
        {
            var textBlock = new TextBlock
            {
                FontSize = 15,
                VerticalAlignment = VerticalAlignment.Center,
                Text = title,
                Margin = new Thickness(10, 0, 15, 0)
            };
            AddControlToMainContainer(textBlock, row, column);
        }

        private void DrawEllipseWithNumber(int row, int column)
        {
            var ellipse = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.DarkSlateGray),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(10),
                Height = RADIUSY * 2.5,
                Width = RADIUSX * 2.5,
            };
            var number = new TextBlock
            {
                FontSize = 15,
                FontWeight = FontWeights.DemiBold,
                Text = (row / 3 + 1).ToString(),
                Foreground = new SolidColorBrush(Colors.RoyalBlue),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            ellipse.Child = number;
            AddControlToMainContainer(ellipse, row, column);
        }

        private void PlaceDateTime(int row, int column, DateTime time)
        {
            var textBlock = new TextBlock
            {
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Center,
                Text = time.ToString("yyyy-MM-dd HH:mm"),
                Margin = new Thickness(10, 0, 15, 0)
            };
            AddControlToMainContainer(textBlock, row, column);
        }

        public void AddNode()
        {
            for (int i = 0; i < NodeCount; i++)
            {
                /* Add new container for next state */
                mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
                mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(15) });
                mainContrainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(60) });

                Height += COMPONENT_HEIGHT;
            }
            InvalidateVisual();
        }

        private void DrawConnector(int row, int column, bool lastNode = false)
        {
            Brush usedBrush = new SolidColorBrush(Colors.Black);
            var panel = new Border
            {
                Height = 2,
                LayoutTransform = new RotateTransform(-90.0),
                Background = usedBrush
            };

            AddControlToMainContainer(panel, row, column);
            Grid.SetRowSpan(panel, 2);
        }

        #endregion

        #region Overrided methods

        /* Fires when control created or InvalidateVisual was invoked */
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            mainContrainer.Children.Clear();
            /* Render all timeline nodes */
            for (int i = 0; i < NodeCount; i++)
            {
                var actualNode = TimeLineNodes.ElementAt(i);
                DrawEllipseWithNumber(3 * i, 0);
                PlaceTitle(3 * i, 1, actualNode.Name);
                PlaceDateTime(3 * i + 1, 1, actualNode.Data);
                PlaceDescription(3 * i + 2, 1, actualNode.Description);
                /* Skip drawing connector for leaf */
                if (i == NodeCount - 1)
                    continue;
                DrawConnector(3 * i + 1, 0, i == NodeCount - 2);
            }
        }

        #endregion

        #region Private methods

        private void AddControlToMainContainer(UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
            mainContrainer.Children.Add(element);
        }

        #endregion
    }
}
