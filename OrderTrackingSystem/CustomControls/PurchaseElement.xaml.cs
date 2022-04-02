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
        static List<PurchaseElement> AllElements = new List<PurchaseElement>();

        public static PurchaseElement CheckedElement => AllElements.FirstOrDefault(e => e.selectedBox.IsChecked == true);

        public static readonly DependencyProperty boxSize =
            DependencyProperty.Register(nameof(BoxSizeSelector), typeof(BoxSize),
            typeof(PurchaseElement), new PropertyMetadata());


        internal BoxSize BoxSizeSelector
        {
            get => (BoxSize)GetValue(boxSize);
            set
            {
                SetValue(boxSize, value);
                OnPropertyChanged(nameof(ImageSize));
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(BoxName));
                OnPropertyChanged(nameof(BoxSize));
                OnPropertyChanged(nameof(BoxPrice));
            }

        }

        static Dictionary<BoxSize, int> BoxImageSizes = Boxes.BoxSizeImage;
        static Dictionary<BoxSize, string> BoxImages = Boxes.BoxImages;
        static Dictionary<BoxSize, float> BoxPrices = Boxes.BoxPrices;
        static Dictionary<BoxSize, string> BoxSizes = Boxes.BoxSizes;
        static Dictionary<BoxSize, string> BoxNames = Boxes.BoxNames;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /* Used as attached event for binding in XAML - SendsView */
        public static readonly RoutedEvent BoxChangedEvent =
                EventManager.RegisterRoutedEvent("BoxChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(PurchaseElement));

        public event RoutedEventHandler BoxChanged
        {
            add
            {
                AddHandler(BoxChangedEvent, value);
            }
            remove
            {
                RemoveHandler(BoxChangedEvent, value);
            }
        }

        public PurchaseElement()
        {
            InitializeComponent();
            AllElements.Add(this);
            DataContext = this;
        }

        public int ImageSize => BoxImageSizes[BoxSizeSelector];
        public string ImagePath => BoxImages[BoxSizeSelector];
        public string BoxSize => BoxSizes[BoxSizeSelector];
        public float BoxPrice => BoxPrices[BoxSizeSelector];
        public string BoxName => BoxNames[BoxSizeSelector];
        public Color FillingColor { get; set; } =  Colors.Orange;

        private void selectedBox_Checked(object sender, RoutedEventArgs e)
        {
            /* Implementation of "GroupName" from RadioButton control */
            AllElements.ForEach(x => x.selectedBox.IsChecked = x.selectedBox.Equals(sender));
            /* Wywołujemy zdarzenie po zmianie wybranego box'a */
            RaiseEvent(new RoutedEventArgs(BoxChangedEvent));
        }
    }

}
