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
            if(LinkedItems.Count == 0)
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

            var secondaryRectangle = new Rectangle
            {
                VerticalAlignment = VerticalAlignment.Center,
                Height = 2,
                Fill = new SolidColorBrush { Color = Colors.Black }
            };

            Grid.SetColumn(rectangle, 2);
            helpGrid.Children.Add(rectangle);

            Grid.SetColumn(secondaryRectangle, 0);
            secondaryGrid.Children.Add(secondaryRectangle);
        }

        private void DrawOrderElements()
        {
            for(int i = 0; i < LinkedItems.Count; i++)
            {
                var border = new Border()
                {
                    CornerRadius = new CornerRadius(15),
                    BorderBrush = new SolidColorBrush { Color = Colors.Maroon },
                    BorderThickness = new Thickness(3),
                    Margin = new Thickness(0, 5, 5, 5),
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
                //DrawHelperConnector(rowNumber);
                ordersGrid.Children.Add(border);
            }
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

        private void DrawHelperConnector(int rowNumber)
        {
            if (LinkedItems.Count == 1) return;

            var line = new Line
            {
                Stroke = new SolidColorBrush { Color = Colors.Black },
                StrokeThickness = 2,
                Tag = "Connector"
            };

            line.X1 = 0;
            line.Y1 = secondaryGrid.ActualHeight / 2;

            line.X2 = secondaryGrid.ColumnDefinitions[0].ActualWidth;
            if (rowNumber == 2)
            {
                line.Y2 = secondaryGrid.ActualHeight - ordersGrid.RowDefinitions[rowNumber].ActualHeight / 2;
            }
            else
            {
                line.Y2 = ordersGrid.RowDefinitions[rowNumber].ActualHeight / 2;
            }

            secondaryGrid.Children.Add(line);
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
