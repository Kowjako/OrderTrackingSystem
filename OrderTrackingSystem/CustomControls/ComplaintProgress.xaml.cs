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

namespace OrderTrackingSystem.Presentation.CustomControls
{
    /// <summary>
    /// Interaction logic for ComplaintProgress.xaml
    /// </summary>
    public partial class ComplaintProgress : UserControl
    {
        private const int RADIUS_X = 10;
        private const int RADIUS_Y = 10;


        public int ActualComplaintState
        {
            get { return (int)GetValue(ActualComplaintStateProperty); }
            set { SetValue(ActualComplaintStateProperty, value); }
        }

        public static readonly DependencyProperty ActualComplaintStateProperty =
            DependencyProperty.Register("ActualComplaintState", typeof(int), typeof(ComplaintProgress), new PropertyMetadata(0));



        public ComplaintProgress()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            GenerateStates();
        }

        private void GenerateStates()
        {
            for (int i = 0; i < 3; i++)
            {
                var ellipse = new Border
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1.5),
                    Height = RADIUS_Y * 2,
                    Margin = i != 0 ? new Thickness(0,10,0,0) : new Thickness(0),
                    Width = RADIUS_X * 2
                };


                if (i < ActualComplaintState)
                {
                    ellipse.Child = GenerateChildRectangle();
                }

                Grid.SetColumn(ellipse, 0);
                Grid.SetRow(ellipse, i);
                mainContainer.Children.Add(ellipse);
            }
        }

        private UIElement GenerateChildRectangle()
        {
            return new Rectangle
            {
                Height = RADIUS_Y,
                Width = RADIUS_X,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Fill = new SolidColorBrush(Colors.DodgerBlue)
            };
        }
    }
}
