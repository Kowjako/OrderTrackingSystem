using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
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
    public class SendsViewModel : ISendsViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region Private methods

        private void RecalculateCartPrice()
        {
            var finalPriceNetto = 0.0m;
            foreach (var product in ProductsInCart)
            {
                var amount = int.Parse(product.Amount);
                var price = decimal.Parse(product.Cena.Replace(',', '.'), CultureInfo.InvariantCulture);
                finalPriceNetto += amount * price;
            }
            TotalPriceNetto = finalPriceNetto;
            OnPropertyChanged(nameof(TotalPriceNetto));
            OnPropertyChanged(nameof(TotalPriceBrutto));
            OnPropertyChanged(nameof(FullPrice));
        }

        #endregion

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

        public decimal VAT { get; } = 23;
        public decimal TotalPriceBrutto => TotalPriceNetto * VAT / 100;
        public decimal FullPrice => TotalPriceBrutto + TotalPriceNetto + (decimal)BoxPrice;

        /* Filtering */
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

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

        #region Services

        private readonly CustomerService CustomerService;
        private readonly ProductService ProductService;
        private readonly SellService SellService;

        #endregion

        #region Ctor

        public SendsViewModel()
        {
            CustomerService = new CustomerService();
            ProductService = new ProductService();
            SellService = new SellService();
        }


        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            AllProductsList = await ProductService.GetAllProducts();
            CurrentSeller = await CustomerService.GetCurrentCustomer();
            CategoriesList = await ProductService.GetProductCategories();
            ProductsList = AllProductsList;
        }

        #endregion

        #region Commands

        private RelayCommand minusAmount;
        public RelayCommand MinusAmount =>
            minusAmount ?? (minusAmount = new RelayCommand(obj =>
            {
                if (CurrentProductAmount == 0)
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

        private RelayCommand _findReceiver;
        public RelayCommand FindReceiver =>
            _findReceiver ?? (_findReceiver = new RelayCommand(async obj =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(obj as string))
                    {
                        CurrentReceiver = await CustomerService.GetCustomerByName(obj as string);
                        OnPropertyChanged(nameof(CurrentReceiver));
                    }
                    else
                    {
                        OnWarning?.Invoke("Pole odbiorca nie może być puste");
                    }
                }
                catch (Exception ex)
                {

                }
            }));

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
                        OnPropertyChanged(nameof(ProductsList));
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

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ?? (_filterCommand = new RelayCommand(obj =>
            {
                try
                {
                    ProductsList = AllProductsList;
                    if(MinPrice == 0m)
                    {
                        if (MaxPrice != 0m)
                        ProductsList = ProductsList.Where(p => decimal.Parse(p.Netto
                                                   .Substring(0, p.Netto.IndexOf(" ")), CultureInfo.InvariantCulture) <= MaxPrice)
                                                   .ToList();
                    }
                    else
                    {
                        if(MaxPrice == 0m)
                        {
                            ProductsList = ProductsList.Where(p => decimal.Parse(p.Netto
                                                                               .Substring(0, p.Netto.IndexOf(" ")), CultureInfo.InvariantCulture) >= MinPrice)
                                                                               .ToList();
                        }
                        else
                        {
                            ProductsList = ProductsList.Where(p => decimal.Parse(p.Netto
                                                                          .Substring(0, p.Netto.IndexOf(" ")), CultureInfo.InvariantCulture) >= MinPrice &&
                                                                   decimal.Parse(p.Netto
                                                                          .Substring(0, p.Netto.IndexOf(" ")), CultureInfo.InvariantCulture) <= MaxPrice)
                                                                          .ToList();
                        }
                    }

                    if(SelectedSubCategory != null)
                    {
                        ProductsList = ProductsList.Where(p => p.SubCategoryId == SelectedSubCategory.Id).ToList();
                    }
                    OnPropertyChanged(nameof(ProductsList));
                }
                catch (Exception)
                {

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

        private RelayCommand _acceptSend;
        public RelayCommand AcceptSend =>
            _acceptSend ?? (_acceptSend = new RelayCommand(async obj =>
            {
                try
                {
                    if (ProductsInCart.Count == 0)
                    {
                        OnWarning?.Invoke("Należy dodać produkt do koszyka");
                        return;
                    }

                    var currentSell = new SellDTO();
                    currentSell.Numer = ConfigurationService.GenerateElementNumber();
                    currentSell.SellerId = CurrentSeller.Id;
                    currentSell.CustomerId = CurrentReceiver.Id;

                    await SellService.SaveSell(currentSell, ProductsInCart.ToList());
                    OnSuccess?.Invoke("Wysyłka została utworzona");
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Nie udało się zapisać wysyłki");
                }
            }));

        #endregion

        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

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
