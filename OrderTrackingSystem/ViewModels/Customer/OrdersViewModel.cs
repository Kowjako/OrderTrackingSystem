using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
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
    public partial class OrdersViewModel : IOrdersViewModel, INotifyPropertyChanged
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
            OnManyPropertyChanged(new[] {nameof(CurrentCustomer), nameof(PickupsList), nameof(AllProductsList), nameof(ProductsList),
                                          nameof(VouchersList)});
        }

        #endregion

        #region Commands

        private RelayCommand _addToCart;
        public RelayCommand AddToCart =>
            _addToCart ?? (_addToCart = new RelayCommand(obj =>
            {
                try
                {
                    if (ProductsInCart.Any(x => x.Name.Equals(SelectedProduct.Name)))
                    {
                        var existingProduct = ProductsInCart.First(x => x.Name.Equals(SelectedProduct.Name));
                        var elementIndex = ProductsInCart.IndexOf(existingProduct);
                        existingProduct.Amount = existingProduct.Amount + CurrentProductAmount;
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
                        if (!ProductsList.Any(p => p.Seller.Equals(obj as string)))
                        {
                            OnWarning("Nie ma sprzedawcy o takiej nazwie");
                            return;
                        }
                        ProductsList = new List<ProductDTO>(AllProductsList.Where(p => p.Seller.Equals(obj as string)));
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
                    CurrentOrder.PickupDTO = SelectedPickup;
                    CurrentOrder.DeliveryType = SelectedDeliveryType.ToString();
                    CurrentOrder.CartProducts = ProductsInCart;
                    CurrentOrder.PickupId = SelectedPickup.Id;
                    CurrentOrder.SellerId = SelectedSellerId;
                    CurrentOrder.CustomerId = CurrentCustomer.Id;

                    if(ValidatorWrapper.ValidateWithResult(new OrderValidator(), CurrentOrder))
                    {
                        decimal valueToMinusFromBalance = 0.0m;

                        /* Został wybrany bon */
                        if (SelectedVoucher != null)
                        {
                            if (IsVoucherFullChecked)
                            {
                                if (FullPrice < SelectedVoucher.Value)
                                {
                                    SelectedVoucher.Value -= FullPrice;
                                }
                                else
                                {
                                    /* Obliczamy kwote co musimy odjac z glownego konta */
                                    valueToMinusFromBalance = FullPrice - SelectedVoucher.Value;
                                    /* Rozliczamy bon w calosci */
                                    SelectedVoucher.Value = 0;
                                }
                            }
                            else
                            {
                                if (VoucherValueToMinus > SelectedVoucher.Value)
                                {
                                    OnWarning?.Invoke("Kwota odliczalna nie może być większa niż kwota bonu");
                                    return;
                                }
                                else
                                {
                                    if (VoucherValueToMinus > FullPrice)
                                    {
                                        OnWarning?.Invoke("Kwota odliczalna nie może być większa niż kwota zamówienia");
                                        return;
                                    }
                                    else
                                    {
                                        SelectedVoucher.Value -= FullPrice;
                                        valueToMinusFromBalance = FullPrice - VoucherValueToMinus;
                                    }
                                }
                            }
                        }
                        await OrderService.SaveOrder(CurrentOrder, ProductsInCart.ToList(), valueToMinusFromBalance, SelectedVoucher ?? null);
                        OnSuccess?.Invoke("Zamówienie zostało utworzone");
                    }
                    else
                    {
                        OnWarning?.Invoke(ValidatorWrapper.ErrorMessage);
                    }
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
