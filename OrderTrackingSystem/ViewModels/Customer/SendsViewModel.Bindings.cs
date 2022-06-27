using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class SendsViewModel
    {
        #region Bindable properties

        public decimal TotalPriceNetto { get; set; } = 0;

        private float _boxPrice;
        public float BoxPrice
        {
            get => _boxPrice;
            set
            {
                _boxPrice = value;
                OnPropertyChanged(nameof(BoxPrice));
                OnPropertyChanged(nameof(FullPrice));
            }
        }

        public decimal VAT => 23;
        public decimal TotalPriceBrutto => TotalPriceNetto * VAT / 100;
        public decimal FullPrice => TotalPriceBrutto + TotalPriceNetto + (decimal)BoxPrice;

        /* Filtering */
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public bool IsPickupDaysDefined { get; set; }
        public bool SendAutomaticMail { get; set; }
        public int PickupDays { get; set; }

        public CategoryDTO SelectedSubCategory { get; set; }

        public Customers CurrentSeller { get; private set; }
        public CustomerDTO CurrentReceiver { get; private set; }
        public List<ProductDTO> AllProductsList { get; set; } = new List<ProductDTO>();
        public List<ProductDTO> ProductsList { get; set; } = new List<ProductDTO>();
        public List<CategoryDTO> CategoriesList { get; set; } = new List<CategoryDTO>();
        public ProductDTO SelectedProduct { get; set; }
        /* Używamy BindingList do śledzenia zmian obiektów z listy */
        public BindingList<CartProductDTO> ProductsInCart { get; set; } = new BindingList<CartProductDTO>();

        public int CurrentProductAmount { get; set; } = 0;

        #endregion
    }
}
