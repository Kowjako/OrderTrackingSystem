using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class SendsViewModel : BaseViewModel, ISendsViewModel
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
            CustomerService = new CustomerService(new ConfigurationService());
            ProductService = new ProductService(new ConfigurationService());
            SellService = new SellService();
            MailService = new MailService();
        }

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
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
                CurrentProductAmount--;
                OnPropertyChanged(nameof(CurrentProductAmount));
            }, (e) => CurrentProductAmount > 0);

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
                if (!string.IsNullOrEmpty(obj as string))
                {
                    CurrentReceiver = await CustomerService.GetCustomerByName(obj as string);
                    OnPropertyChanged(nameof(CurrentReceiver));
                }
                else
                {
                    ShowWarning("Pole odbiorca nie może być puste");
                }
            });

        private RelayCommand _addToCart;
        public RelayCommand AddToCart =>
            _addToCart ??= new RelayCommand(obj =>
            {
                if (CurrentProductAmount == default) return;
                if (ProductsInCart.Any(x => x.Id.Equals(SelectedProduct.Id)))
                {
                    var existingProduct = ProductsInCart.First(x => x.Id.Equals(SelectedProduct.Id));
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
            }, (e) => SelectedProduct != null && CurrentProductAmount > 0);

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ??= new RelayCommand(obj =>
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

                if (SelectedSubCategory != null)
                {
                    /* Ustawiamy ID grupy glownej i jej grup podrzednych */
                    var list = SelectedSubCategory.Children.Select(p => p.Id).ToList();
                    list.Add(SelectedSubCategory.Id);
                    ProductsList = ProductsList.Where(p => p.CategoryId.In(list.ToArray())).ToList();
                }
                OnPropertyChanged(nameof(ProductsList));
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

        private RelayCommand _acceptSend;
        public RelayCommand AcceptSell =>
            _acceptSend ??= new RelayCommand(async obj =>
            {
                try
                {
                    if (ProductsInCart.Count == 0)
                    {
                        ShowWarning("Należy dodać produkt do koszyka");
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
                    ShowSuccess("Wysyłka została utworzona");
                }
                catch (InvalidOperationException ex)
                {
                    ShowError(ex.Message);
                }
            });

        #endregion

    }
}
