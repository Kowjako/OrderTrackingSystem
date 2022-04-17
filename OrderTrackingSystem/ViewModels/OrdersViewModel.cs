using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class OrdersViewModel : IOrdersViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Services

        private readonly ConfigurationService ConfigService;
        private readonly CustomerService CustomerService;
        private readonly ProductService ProductService;
        private readonly OrderService OrderService;

        #endregion

        #region Private objects & methods

        private void RecalculateCartPrice()
        {
            var finalPriceNetto = 0.0m;
            foreach(var product in ProductsInCart)
            {
                var amount = int.Parse(product.Amount);
                var price = decimal.Parse(product.Cena.Replace(',','.'), CultureInfo.InvariantCulture);
                finalPriceNetto += amount * price;
            }
            TotalPriceNetto = finalPriceNetto;
            OnPropertyChanged(nameof(TotalPriceNetto));
            OnPropertyChanged(nameof(TotalPriceBrutto));
            OnPropertyChanged(nameof(FullPrice));
        }

        #endregion

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
                if(value == true)
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

        #region Ctor

        public OrdersViewModel()
        {
            ConfigService = new ConfigurationService();
            CustomerService = new CustomerService();
            ProductService = new ProductService();
            OrderService = new OrderService();
        }


        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        { 
            CurrentCustomer = await CustomerService.GetCustomer((await CustomerService.GetCurrentCustomer()).Id);
            PickupsList = await ConfigService.GetPickupPoints();
            AllProductsList = await ProductService.GetAllProducts();
            ProductsList = AllProductsList;
            VouchersList = await ProductService.GetVouchersForCurrentCustomer();
        }

        #endregion

        #region Commands

        private RelayCommand _addToCart;
        public RelayCommand AddToCart =>
            _addToCart ?? (_addToCart = new RelayCommand(obj =>
            {
                try
                {
                    if (ProductsInCart.Any(x => x.Nazwa.Equals(SelectedProduct.Nazwa)))
                    {
                        var existingProduct = ProductsInCart.First(x => x.Nazwa.Equals(SelectedProduct.Nazwa));
                        var elementIndex = ProductsInCart.IndexOf(existingProduct);
                        existingProduct.Amount = (int.Parse(existingProduct.Amount) + CurrentProductAmount).ToString();
                        ProductsInCart[elementIndex] = existingProduct;
                    }
                    else
                    {
                        var price = decimal.Parse(SelectedProduct.Netto.Substring(0, SelectedProduct.Netto.IndexOf(" ")), CultureInfo.InvariantCulture);
                        var discount = decimal.Parse(SelectedProduct.Rabat.Substring(0, SelectedProduct.Rabat.IndexOf(" ")), CultureInfo.InvariantCulture);
                        var priceWithDiscount = price - price * discount / 100;
                        ProductsInCart.Add(new CartProductDTO()
                        {
                            Id = SelectedProduct.Id,
                            Nazwa = SelectedProduct.Nazwa,
                            Cena = priceWithDiscount.ToString(),
                            Amount = CurrentProductAmount.ToString(),
                            Rabat = decimal.Parse(SelectedProduct.Rabat.Substring(0, SelectedProduct.Rabat.IndexOf(" ")), CultureInfo.InvariantCulture)
                        });
                        SelectedSellerId = SelectedProduct.SellerId;
                        ProductsList = AllProductsList.Where(p => p.SellerId == SelectedSellerId).ToList();
                    }
                    CurrentProductAmount = 0;
                    RecalculateCartPrice();
                    OnPropertyChanged(nameof(CurrentProductAmount));
                    OnPropertyChanged(nameof(ProductsInCart));
                }
                catch (Exception ex)
                {
                    OnFailure?.Invoke("Nie udało się dodać do koszyka");
                }
            }));


        private RelayCommand _findSeller;
        public RelayCommand FindSeller =>
            _findSeller ?? (_findSeller = new RelayCommand(obj =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(obj as string))
                    {
                        if (!ProductsList.Any(p => p.Sprzedawca.Equals(obj as string)))
                        {
                            OnWarning("Nie ma sprzedawcy o takiej nazwie");
                            return;
                        }
                        ProductsList = new List<ProductDTO>(AllProductsList.Where(p => p.Sprzedawca.Equals(obj as string)));
                        OnPropertyChanged(nameof(ProductsList));
                    }
                    else
                    {
                        if (AllProductsList.Any())
                        {
                            ProductsList = AllProductsList;
                            OnPropertyChanged(nameof(ProductsList));
                            return;
                        }
                        OnWarning("Nazwa nie może być pusta");
                    }
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand minusAmount;
        public RelayCommand MinusAmount =>
            minusAmount ?? (minusAmount = new RelayCommand(obj =>
            {
                if(CurrentProductAmount == 0)
                {
                    return;
                }
                else
                {
                    CurrentProductAmount--;
                    OnPropertyChanged(nameof(CurrentProductAmount));
                }
            }));

        private RelayCommand plusAmount;
        public RelayCommand PlusAmount =>
            plusAmount ?? (plusAmount = new RelayCommand(obj =>
            {
                CurrentProductAmount++;
                OnPropertyChanged(nameof(CurrentProductAmount));
            }));

        private RelayCommand _acceptOrder;
        public RelayCommand AcceptOrder =>
            _acceptOrder ?? (_acceptOrder = new RelayCommand(async obj =>
            {
                try
                {
                    /* 1 - Zapis zamówienia */
                    if (SelectedPickup == null)
                    {
                        OnWarning?.Invoke("Należy wybrać punkt odbioru");
                        return;
                    }
                    if (SelectedDeliveryType == -1)
                    {
                        OnWarning?.Invoke("Należy wybrać typ dostawy");
                        return;
                    }
                    if (int.Parse(CurrentOrder.Oplata) == -1)
                    {
                        OnWarning?.Invoke("Należy wybrać typ opłaty");
                        return;
                    }
                    if (ProductsInCart.Count == 0)
                    {
                        OnWarning?.Invoke("Należy dodać produkt do koszyka");
                        return;
                    }

                    CurrentOrder.PickupId = SelectedPickup.Id;
                    CurrentOrder.Dostawa = SelectedDeliveryType.ToString();
                    CurrentOrder.SellerId = SelectedSellerId;
                    CurrentOrder.CustomerId = CurrentCustomer.Id;

                    await OrderService.SaveOrder(CurrentOrder, ProductsInCart.ToList());
                    OnSuccess?.Invoke("Zamówienie zostało utworzone");
                }
                catch(Exception)
                {
                    OnFailure?.Invoke("Nie udało się zapisać zamówienia");
                }
            }));

        private RelayCommand _clearCart;
        public RelayCommand ClearCart =>
            _clearCart ?? (_clearCart = new RelayCommand(obj =>
            {
                ProductsInCart.Clear();
                RecalculateCartPrice();
                OnPropertyChanged(nameof(ProductsInCart));
                OnSuccess?.Invoke("Koszyk pomyślnie wyczyszczony");
            }));
        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}
