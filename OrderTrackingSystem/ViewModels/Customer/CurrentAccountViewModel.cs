using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.ViewModels
{
    public class CurrentAccountViewModel : BaseViewModel, ICurrentAccountViewModel
    {
        #region Services

        private readonly CustomerService CustomerService;
        private readonly LocalizationService LocalizationService;
        private readonly OrderService OrderSerivce;
        private readonly SellService SellService;

        #endregion

        #region Bindable objects

        public Customers CurrentCustomer { get; set; }
        public List<LocalizationDTO> Localization { get; set; } = new List<LocalizationDTO>();
        public List<OrderDTO> Orders { get; set; } = new List<OrderDTO>();
        public List<SellDTO> Sells { get; set; } = new List<SellDTO>();

        #endregion

        #region Ctor

        public CurrentAccountViewModel()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            OrderSerivce = new OrderService(CustomerService);
            SellService = new SellService();
        }

        #endregion

        #region Commands

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??= new RelayCommand(async obj =>
            {
                try
                {
                    ValidatorWrapper.Validate(new CustomerValidator(), CurrentCustomer);
                    if (ValidatorWrapper.IsValid)
                    {
                        ValidatorWrapper.Validate(new LocalizationValidator(), Localization[0]);
                        if (ValidatorWrapper.IsValid)
                        {
                            /* Update customer */
                            await CustomerService.UpdateCustomer(CurrentCustomer);

                            /* Update localization */
                            var currentLocalization = Localization[0];

                            var localization = new Localizations
                            {
                                Id = currentLocalization.Id,
                                City = currentLocalization.City,
                                Country = currentLocalization.Country,
                                Street = currentLocalization.Street,
                                Flat = (byte)currentLocalization.Apartment,
                                House = (byte)currentLocalization.House,
                                ZipCode = currentLocalization.ZipCode
                            };
                            await LocalizationService.UpdateLocalization(localization);
                            ShowSuccess("Zmiany zostały zapisane");
                        }
                        else
                        {
                            ShowError(ValidatorWrapper.ErrorMessage);
                        }
                    }
                    else
                    {
                        ShowError(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    ShowError("Błąd podczas aktualizacji danych");
                }
            });

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            Localization = new List<LocalizationDTO> { await LocalizationService.GetLocalizationById(CurrentCustomer.LocalizationId) };
            Orders = await OrderSerivce.GetOrdersForCustomer(CurrentCustomer.Id);
            Sells = await SellService.GetSellsForCustomer(CurrentCustomer.Id);

            //Zmienilismy calkowicie listy poprzez = new ..., wiec musimy wywolac zeby odswiezyc wiazania
            OnManyPropertyChanged(new[] { nameof(CurrentCustomer), nameof(Localization), nameof(Orders), nameof(Sells) });
        }

        #endregion
    }
}
