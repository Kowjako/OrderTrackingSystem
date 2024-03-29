﻿using OrderTrackingSystem.CustomControls.Classes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrderTrackingSystem.CustomControls
{
    /// <summary>
    /// Interaction logic for PurchaseElement.xaml
    /// </summary>
    public partial class PurchaseElement : UserControl, INotifyPropertyChanged
    {
        static List<PurchaseElement> AllElements = new List<PurchaseElement>();

        public static PurchaseElement CheckedElement => AllElements.FirstOrDefault(e => e.selectedBox.IsChecked == true);

        /* Used to set UI Control property */
        public static readonly DependencyProperty boxSize =
            DependencyProperty.Register(nameof(BoxSizeSelector), 
            typeof(BoxSize),
            typeof(PurchaseElement), 
            new FrameworkPropertyMetadata(Classes.BoxSize.Small, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Callback)));

        internal BoxSize BoxSizeSelector
        {
            get { return (BoxSize)GetValue(boxSize); }
            set { SetValue(boxSize, value); }
        }

        private static void Callback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

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

        internal Dictionary<BoxSize, int> BoxImageSizes = Boxes.BoxSizeImage;
        internal Dictionary<BoxSize, string> BoxImages = Boxes.BoxImages;
        internal Dictionary<BoxSize, float> BoxPrices = Boxes.BoxPrices;
        internal Dictionary<BoxSize, string> BoxSizes = Boxes.BoxSizes;
        internal Dictionary<BoxSize, string> BoxNames = Boxes.BoxNames;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public PurchaseElement()
        {
            InitializeComponent();
            AllElements.Add(this);
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
