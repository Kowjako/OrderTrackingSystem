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
            line1.Width = mainContainer.ColumnDefinitions[1].ActualWidth;
            line2.Width = line1.Width;
        }
    }
}
