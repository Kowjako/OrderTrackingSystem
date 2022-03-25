using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.ViewModels
{
    public class CurrentAccountViewModel : ICurrentAccountViewModel, INotifyableViewModel
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

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
            CustomerService = new CustomerService();
            LocalizationService = new LocalizationService();
            OrderSerivce = new OrderService();
            SellService = new SellService();
        }

        #endregion

        #region Commands

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(async obj =>
            {
                try
                {
                    /* Update customer */
                    CustomerService.UpdateCustomer(CurrentCustomer);

                    /* Update localization */
                    var currentLocalization = Localization[0];
                    var localization = new Localizations
                    {
                        Id = currentLocalization.Id,
                        City = currentLocalization.Miasto,
                        Country = currentLocalization.Kraj,
                        Street = currentLocalization.Ulica,
                        Flat = (byte)currentLocalization.Mieszkanie,
                        House = (byte)currentLocalization.Budynek,
                        ZipCode = currentLocalization.Kod
                    };

                    await LocalizationService.UpdateLocalization(localization);
                    OnSuccess?.Invoke("Zmiany zostały zapisane");
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Wystąpił błąd podczas zapisywania danych");
                }
            }));

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            Localization.Add(await LocalizationService.GetLocalizationById(CurrentCustomer.LocalizationId));
            Orders = await OrderSerivce.GetOrdersForCustomer(CurrentCustomer.Id);
            Sells = await SellService.GetSellsForCustomer(CurrentCustomer.Id);
        }

        #endregion
    }
}
