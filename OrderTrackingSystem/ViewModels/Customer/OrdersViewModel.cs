using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class OrdersViewModel : BaseViewModel, IOrdersViewModel
    {
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

        public override async Task SetInitializeProperties()
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
            _addToCart ??= new RelayCommand(obj =>
            {
                try
                {
                    if (ProductsInCart.Any(x => x.Id == SelectedProduct.Id))
                    {
                        var existingProduct = ProductsInCart.First(x => x.Id == SelectedProduct.Id);
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
                        SelectedSellerId = SelectedProduct.SellerId;
                        ProductsList = AllProductsList.Where(p => p.SellerId == SelectedSellerId).ToList();
                    }
                    CurrentProductAmount = 0;
                    RecalculateCartPrice();
                    OnManyPropertyChanged(new[] { nameof(CurrentProductAmount), nameof(ProductsInCart), nameof(ProductsList) });
                }
                catch (Exception ex)
                {
                    ShowError("Nie udało się dodać do koszyka");
                }
            });


        private RelayCommand _findSeller;
        public RelayCommand FindSeller =>
            _findSeller ??= new RelayCommand(obj =>
            {
                if (!string.IsNullOrEmpty(obj as string))
                {
                    if (!ProductsList.Any(p => p.Seller.Equals(obj as string)))
                    {
                        ShowWarning("Nie ma sprzedawcy o takiej nazwie");
                        return;
                    }
                    ProductsList = new List<ProductDTO>(AllProductsList.Where(p => p.Seller.Equals(obj as string)));
                }
                else
                {
                    if (AllProductsList.Any())
                    {
                        ProductsList = AllProductsList;
                        return;
                    }
                    ShowWarning("Nazwa nie może być pusta");
                }
                OnPropertyChanged(nameof(ProductsList));
            });

        private RelayCommand minusAmount;
        public RelayCommand MinusAmount =>
            minusAmount ??= new RelayCommand(obj =>
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
            });

        private RelayCommand plusAmount;
        public RelayCommand PlusAmount =>
            plusAmount ??= new RelayCommand(obj =>
            {
                CurrentProductAmount++;
                OnPropertyChanged(nameof(CurrentProductAmount));
            });

        private RelayCommand _acceptOrder;
        public RelayCommand AcceptOrder =>
            _acceptOrder ??= new RelayCommand(async obj =>
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
                                    ShowWarning("Kwota odliczalna nie może być większa niż kwota bonu");
                                    return;
                                }
                                else
                                {
                                    if (VoucherValueToMinus > FullPrice)
                                    {
                                        ShowWarning("Kwota odliczalna nie może być większa niż kwota zamówienia");
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
                        ShowSuccess("Zamówienie zostało utworzone");
                    }
                    else
                    {
                        ShowWarning(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch(Exception)
                {
                    ShowError("Nie udało się zapisać zamówienia");
                }
            });

        private RelayCommand _clearCart;
        public RelayCommand ClearCart =>
            _clearCart ??= new RelayCommand(obj =>
            {
                ProductsInCart.Clear();
                RecalculateCartPrice();
                ProductsList = AllProductsList;

                OnManyPropertyChanged(new[] { nameof(ProductsInCart), nameof(ProductsList) });
                ShowSuccess("Koszyk pomyślnie wyczyszczony");
            });
        #endregion

    }
}
