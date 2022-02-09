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
        ICollection<string> LinkedItems = new List<string>();

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
                LinkedItems.Add(refNumber);
            }
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

            Grid.SetColumn(secondaryRectangle, 2);
            secondaryGrid.Children.Add(secondaryRectangle);
        }
    }
}
