using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ComplaintProgress : UserControl, INotifyPropertyChanged
    {
        private static event Action RefreshBindedDates;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public IEnumerable<DateTime?> ComplaintDates
        {
            get { return (IEnumerable<DateTime?>)GetValue(ComplaintDatesProperty); }
            set { SetValue(ComplaintDatesProperty, value); }
        }
        public static readonly DependencyProperty ComplaintDatesProperty =
            DependencyProperty.Register("ComplaintDates", 
                typeof(IEnumerable<DateTime?>), 
                typeof(ComplaintProgress), 
                new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, PropertyChangedCallback = CollectionChangedCallback });

        private static void CollectionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshBindedDates?.Invoke();
        }

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
            mainContainer.DataContext = this;
            RefreshBindedDates += RefreshDates;
        }

        private void RefreshDates()
        {
            OnPropertyChanged(nameof(ComplaintDates));
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            line1.Width = mainContainer.ColumnDefinitions[1].ActualWidth;
            line2.Width = line1.Width;
        }
    }
}
