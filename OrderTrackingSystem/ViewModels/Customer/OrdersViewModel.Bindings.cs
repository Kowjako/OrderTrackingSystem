using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class OrdersViewModel
    {
        #region Bindable objects

        private PickupDTO _selectedPickup;
        public PickupDTO SelectedPickup
        {
            get => _selectedPickup;
            set
            {
                _selectedPickup = value;
                OnPropertyChanged(nameof(SelectedPickup));
            }
        }

        public ProductDTO SelectedProduct { get; set; }
        private VoucherDTO _selectedVoucher;
        public VoucherDTO SelectedVoucher
        {
            get => _selectedVoucher;
            set
            {
                _selectedVoucher = value;
                OnPropertyChanged(nameof(SelectedVoucher));
                OnPropertyChanged(nameof(VouchersVisibility));
            }
        }

        public decimal VoucherValueToMinus { get; set; }
        private bool _isVoucherFullChecked;
        public bool IsVoucherFullChecked
        {
            get => _isVoucherFullChecked;
            set
            {
                _isVoucherFullChecked = value;
                if (value == true)
                {
                    VoucherValueToMinus = 0m;
                    OnPropertyChanged(nameof(VoucherValueToMinus));
                }
            }
        }
        public Visibility VouchersVisibility => SelectedVoucher != null ? Visibility.Visible : Visibility.Collapsed;

        public int CurrentProductAmount { get; set; } = 0;

        public CustomerDTO CurrentCustomer { get; private set; }
        public int SelectedSellerId { get; private set; }
        public List<PickupDTO> PickupsList { get; set; } = new List<PickupDTO>();
        public List<ProductDTO> AllProductsList { get; set; } = new List<ProductDTO>();
        public List<ProductDTO> ProductsList { get; set; } = new List<ProductDTO>();
        public List<VoucherDTO> VouchersList { get; set; } = new List<VoucherDTO>();
        /* Używamy BindingList do śledzenia zmian obiektów z listy*/
        public BindingList<CartProductDTO> ProductsInCart { get; set; } = new BindingList<CartProductDTO>();

        private int _selectedDeliveryType = -1;
        public int SelectedDeliveryType
        {
            get => _selectedDeliveryType;
            set
            {
                _selectedDeliveryType = value;
                OnPropertyChanged(nameof(DeliveryCost));
                OnPropertyChanged(nameof(FullPrice));
            }
        }
        /* Tworzenie zamówienia */
        public OrderDTO CurrentOrder { get; set; } = new OrderDTO();

        public decimal TotalPriceNetto { get; set; } = 0;
        public decimal VAT { get; } = 23;
        public decimal TotalPriceBrutto => TotalPriceNetto * VAT / 100;

        /* Analogiczne z get - switch - return */
        public decimal DeliveryCost => SelectedDeliveryType switch
        {
            0 => 9.99m,
            1 => 11.99m,
            2 => 0.0m,
            3 => 4.99m,
            _ => 9m
        };

        public decimal FullPrice => TotalPriceBrutto + TotalPriceNetto + DeliveryCost;

        #endregion
    }
}
