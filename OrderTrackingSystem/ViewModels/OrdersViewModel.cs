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
        public int CurrentProductAmount { get; set; } = 0;

        public CustomerDTO CurrentCustomer { get; private set; }
        public List<PickupDTO> PickupsList { get; set; } = new List<PickupDTO>();
        public List<ProductDTO> AllProductsList { get; set; } = new List<ProductDTO>();
        public List<ProductDTO> ProductsList { get; set; } = new List<ProductDTO>();
        /* Używamy BindingList do śledzenia zmian obiektów z listy */
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
        public decimal VAT { get; set; } = 23;
        public decimal TotalPriceBrutto => TotalPriceNetto * VAT / 100;
        public decimal DeliveryCost
        {
            get
            {
                switch (SelectedDeliveryType)
                {
                    case 0:
                        return 9.99m;
                    case 1:
                        return 11.99m;
                    case 2:
                        return 0.0m;
                    case 3:
                        return 4.99m;
                    default:
                        return 0m;
                }
            }
        }
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
                            Nazwa = SelectedProduct.Nazwa,
                            Cena = priceWithDiscount.ToString(),
                            Amount = CurrentProductAmount.ToString(),
                            Rabat = decimal.Parse(SelectedProduct.Rabat.Substring(0, SelectedProduct.Rabat.IndexOf(" ")), CultureInfo.InvariantCulture)
                        });
                    }
                    CurrentProductAmount = 0;
                    RecalculateCartPrice();
                    OnPropertyChanged(nameof(CurrentProductAmount));
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
