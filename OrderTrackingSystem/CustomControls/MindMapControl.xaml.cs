using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OrderTrackingSystem.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for MindMapControl.xaml
    /// </summary>
    public partial class MindMapControl : UserControl
    {
        IList<string> LinkedItems = new List<string>();

        public MindMapControl()
        {
            InitializeComponent();
        }

        private void expandBtn_Click(object sender, RoutedEventArgs e)
        {
            AddNode("XY123412412412");
        }

        public void AddNode(string refNumber)
        {
            if (LinkedItems.Count == 3) return;
            if (LinkedItems.Count == 0)
            {
                DrawMainConnector();
            }
            LinkedItems.Add(refNumber);
            OnNodeAdded(refNumber);
        }

        private void DrawMainConnector()
        {
            var rectangle = new Rectangle()
            {
                VerticalAlignment = VerticalAlignment.Center,
                Height = 2,
                Fill = new SolidColorBrush { Color = Colors.Black}
            };

            Grid.SetColumn(rectangle, 2);
            helpGrid.Children.Add(rectangle);
        }

        private void DrawOrderElements()
        {
            /* Usuwamy poprzednie polaczenia */
            var copy = secondaryGrid.Children.Count; /* tworzymy value-type kopie */
            for (int i = 1; i < copy; i++)
            {
                secondaryGrid.Children.RemoveAt(1);
            }

            for (int i = 0; i < LinkedItems.Count; i++)
            {
                var border = new Border()
                {
                    CornerRadius = new CornerRadius(15),
                    BorderBrush = new SolidColorBrush
                    {
                        Color = i == 0 ? Colors.Red :
                        i == 1 ? Colors.Green :
                        Colors.Blue
                    },
                    BorderThickness = new Thickness(3),
                    Margin = new Thickness(0, 2, 5, 2),
                };

                var header = new TextBlock()
                {
                    Text = LinkedItems[i],
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                border.Child = header;

                var rowNumber = CalculateRowNumber(i);
                
                Grid.SetRow(border, rowNumber);
                ordersGrid.Children.Add(border);
            }
            DrawHelperConnector();
        }

        private void OnNodeAdded(string refNumber)
        {
            BuildDefaultGrid();
            ordersGrid.Children.Clear();
            DrawOrderElements();
        }
        
        private int CalculateRowNumber(int elementIndex)
        {
            if (LinkedItems.Count == 1)
            {
                return 1;
            }
            else 
                return elementIndex;
        }

        private void DrawHelperConnector()
        {
            if (LinkedItems.Count == 1 | LinkedItems.Count == 3)
            {
                var line = new Rectangle
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 2,
                    Fill = new SolidColorBrush { Color = Colors.Black }
                };

                Grid.SetColumn(line, 0);
                secondaryGrid.Children.Add(line);
            }
            if (LinkedItems.Count != 1)
            {
                #region Vertical Connector

                var line2 = new Rectangle
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 1.5,
                    Fill = new SolidColorBrush { Color = Colors.Black }
                };

                line2.Width = secondaryGrid.ActualHeight - ordersGrid.RowDefinitions[0].ActualHeight / 2;

                /* Ustawiamy obrot + dublowanie */
                var transformGroup = new TransformGroup();
                var rotateTransform = new RotateTransform() { Angle = 270 };
                var scaleTransform = new ScaleTransform() { ScaleY = -1 };

                transformGroup.Children.Add(rotateTransform);
                transformGroup.Children.Add(scaleTransform);

                line2.LayoutTransform = transformGroup;

                Grid.SetColumn(line2, 0);
                secondaryGrid.Children.Add(line2);

                #endregion

                #region Horizontal Connector

                var topConnector = new Rectangle
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 2,
                    Fill = new SolidColorBrush { Color = Colors.Black }
                };

                topConnector.Margin = new Thickness(0, 0, 0, line2.Width);
                Grid.SetColumn(topConnector, 0);
                secondaryGrid.Children.Add(topConnector);

                var bottomConnector = new Rectangle
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Height = 2,
                    Fill = new SolidColorBrush { Color = Colors.Black }
                };

                bottomConnector.Margin = new Thickness(0, line2.Width, 0, 0);
                Grid.SetColumn(bottomConnector, 0);
                secondaryGrid.Children.Add(bottomConnector);

                #endregion
            }
        }

        private void BuildDefaultGrid()
        {
            if (LinkedItems.Count == 2)
            {
                ordersGrid.RowDefinitions.Clear();
                ordersGrid.RowDefinitions.Add(new RowDefinition());
                ordersGrid.RowDefinitions.Add(new RowDefinition());
            }
            else
            {
                if (LinkedItems.Count == 3) ordersGrid.RowDefinitions.Clear();
                ordersGrid.RowDefinitions.Add(new RowDefinition());
                ordersGrid.RowDefinitions.Add(new RowDefinition());
                ordersGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
    }
}
