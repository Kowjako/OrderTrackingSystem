using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
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
    public partial class SendsViewModel : ISendsViewModel, INotifyPropertyChanged
    {
        #region Private methods

        private void RecalculateCartPrice()
        {
            var finalPriceNetto = 0.0m;
            foreach (var product in ProductsInCart)
            {
                var amount = product.Amount;
                var price = product.Price;
                finalPriceNetto += amount * price;
            }
            TotalPriceNetto = finalPriceNetto;
            OnPropertyChanged(nameof(TotalPriceNetto));
            OnPropertyChanged(nameof(TotalPriceBrutto));
            OnPropertyChanged(nameof(FullPrice));
        }

        #endregion

        #region Services

        private readonly CustomerService CustomerService;
        private readonly ProductService ProductService;
        private readonly SellService SellService;
        private readonly MailService MailService;

        #endregion

        #region Ctor

        public SendsViewModel()
        {
            CustomerService = new CustomerService();
            ProductService = new ProductService();
            SellService = new SellService();
            MailService = new MailService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            AllProductsList = await ProductService.GetAllProducts();
            CurrentSeller = await CustomerService.GetCurrentCustomer();
            CategoriesList = await ProductService.GetProductCategories();
            ProductsList = AllProductsList;
            OnManyPropertyChanged(new[] { nameof(ProductsList), nameof(CurrentSeller), nameof(CategoriesList) });
        }

        #endregion

        #region Commands

        private RelayCommand minusAmount;
        public RelayCommand MinusAmount =>
            minusAmount ??= new RelayCommand(obj =>
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
            });

        private RelayCommand plusAmount;
        public RelayCommand PlusAmount =>
            plusAmount ??= new RelayCommand(obj =>
            {
                CurrentProductAmount++;
                OnPropertyChanged(nameof(CurrentProductAmount));
            });

        private RelayCommand _findReceiver;
        public RelayCommand FindReceiver =>
            _findReceiver ??= new RelayCommand(async obj =>
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
            });

        private RelayCommand _addToCart;
        public RelayCommand AddToCart =>
            _addToCart ??= new RelayCommand(obj =>
            {
                try
                {
                    if (ProductsInCart.Any(x => x.Name.Equals(SelectedProduct.Name)))
                    {
                        var existingProduct = ProductsInCart.First(x => x.Name.Equals(SelectedProduct.Name));
                        var elementIndex = ProductsInCart.IndexOf(existingProduct);
                        existingProduct.Amount += CurrentProductAmount;
                        ProductsInCart[elementIndex] = existingProduct;
                    }
                    else
                    {
                        var priceWithDiscount = SelectedProduct.PriceNetto - (SelectedProduct.PriceNetto * SelectedProduct.Discount / 100);
                        ProductsInCart.Add(new CartProductDTO()
                        {
                            Id = SelectedProduct.Id,
                            Name = SelectedProduct.Name,
                            Price = priceWithDiscount,
                            Amount = CurrentProductAmount,
                            Discount = SelectedProduct.Discount
                        });
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
            });

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    ProductsList = AllProductsList;

                    if (MaxPrice == 0m)
                    {
                        ProductsList = ProductsList.Where(p => p.PriceNetto >= MinPrice).ToList();
                    }
                    else
                    {
                        ProductsList = ProductsList.Where(p => p.PriceNetto >= MinPrice && p.PriceNetto <= MaxPrice).ToList();
                    }

                    if(SelectedSubCategory != null)
                    {
                        /* Ustawiamy ID grupy glownej i jej grup podrzednych */
                        var list = SelectedSubCategory.Children.Select(p => p.Id).ToList();
                        list.Add(SelectedSubCategory.Id);
                        ProductsList = ProductsList.Where(p => p.CategoryId.In(list.ToArray())).ToList();
                    }
                    OnPropertyChanged(nameof(ProductsList));
                }
                catch (Exception)
                {

                }
            });

        private RelayCommand _clearCart;
        public RelayCommand ClearCart =>
            _clearCart ??= new RelayCommand(obj =>
            {
                ProductsInCart.Clear();
                RecalculateCartPrice();
                OnPropertyChanged(nameof(ProductsInCart));
                OnSuccess?.Invoke("Koszyk pomyślnie wyczyszczony");
            });

        private RelayCommand _acceptSend;
        public RelayCommand AcceptSell =>
            _acceptSend ??= new RelayCommand(async obj =>
            {
                try
                {
                    if (ProductsInCart.Count == 0)
                    {
                        OnWarning?.Invoke("Należy dodać produkt do koszyka");
                        return;
                    }

                    var currentSell = new SellDTO()
                    {
                        SellerId = CurrentSeller.Id,
                        CustomerId = CurrentReceiver.Id,
                        PickupDays = IsPickupDaysDefined ? PickupDays : 0
                    };

                    await SellService.SaveSell(currentSell, ProductsInCart.ToList());
                    if(SendAutomaticMail)
                    {
                        await MailService.GenerateAutomaticMessageAfterSend(CurrentReceiver.Id, CurrentSeller.Id, currentSell.Number);
                    }
                    OnSuccess?.Invoke("Wysyłka została utworzona");
                }
                catch
                {
                    OnFailure?.Invoke("Nie udało się zapisać wysyłki");
                }
            });

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

        public void OnManyPropertyChanged(IEnumerable<string> props)
        {
            foreach (var prop in props)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion

    }
}
