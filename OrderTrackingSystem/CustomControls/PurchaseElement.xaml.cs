using OrderTrackingSystem.CustomControls.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace OrderTrackingSystem.CustomControls
{
    /// <summary>
    /// Interaction logic for PurchaseElement.xaml
    /// </summary>
    public partial class PurchaseElement : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty boxSize =
            DependencyProperty.Register("BoxSizeSelector", typeof(BoxSize),
            typeof(PurchaseElement), new PropertyMetadata());


        internal BoxSize BoxSizeSelector
        {
            get => (BoxSize)GetValue(boxSize);
            set
            {
                SetValue(boxSize, value);
                OnPropertyChanged("ImageSize");
                OnPropertyChanged("ImagePath");
                OnPropertyChanged("BoxName");
                OnPropertyChanged("BoxSize");
                OnPropertyChanged("BoxPrice");
            }

        }

        static Dictionary<BoxSize, int> BoxImageSizes = Boxes.BoxSizeImage;
        static Dictionary<BoxSize, string> BoxImages = Boxes.BoxImages;
        static Dictionary<BoxSize, float> BoxPrices = Boxes.BoxPrices;
        static Dictionary<BoxSize, string> BoxSizes = Boxes.BoxSizes;
        static Dictionary<BoxSize, string> BoxNames = Boxes.BoxNames;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public PurchaseElement()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int ImageSize => BoxImageSizes[BoxSizeSelector];
        public string ImagePath => BoxImages[BoxSizeSelector];
        public string BoxSize => BoxSizes[BoxSizeSelector];
        public float BoxPrice => BoxPrices[BoxSizeSelector];
        public string BoxName => BoxNames[BoxSizeSelector];
        public Color FillingColor { get; set; } =  Colors.Orange;

    }
}
