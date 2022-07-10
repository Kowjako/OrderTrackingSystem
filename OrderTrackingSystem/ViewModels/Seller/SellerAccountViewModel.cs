using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class SellerAccountViewModel : BaseViewModel, ISellerAccountViewModel
    {
        #region Services

        private readonly LocalizationService LocalizationService;
        private readonly OrderService OrderService;
        private readonly ComplaintService ComplaintService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Bindable properties

        public Sellers CurrentSeller { get; set; }
        public List<OrderDTO> ClientOrders { get; set; } = new List<OrderDTO>();
        public List<ComplaintsDTO> ClientComplaints { get; set; } = new List<ComplaintsDTO>();
        public List<LocalizationDTO> Localizations { get; set; }

        #endregion

        #region Ctor

        public SellerAccountViewModel()
        {
            LocalizationService = new LocalizationService();
            OrderService = new OrderService();
            ComplaintService = new ComplaintService();
            CustomerService = new CustomerService(new ConfigurationService());
        }

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
        {
            CurrentSeller = await CustomerService.GetCurrentSeller();
            Localizations = new List<LocalizationDTO> { await LocalizationService.GetLocalizationById(CurrentSeller.LocalizationId) };
            ClientOrders = await OrderService.GetOrdersFromCompany(CurrentSeller.Id);
            ClientComplaints = await ComplaintService.GetComplaintsForSeller(CurrentSeller.Id);
            OnManyPropertyChanged(new[] { nameof(CurrentSeller), nameof(ClientOrders), nameof(ClientComplaints), nameof(Localizations) });
        }

        #endregion

        #region Commands

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand =>
            _saveCommand ??= new RelayCommand(async obj =>
            {
                try
                {
                    bool result = ValidatorWrapper.ValidateWithResult(new SellerValidator(), CurrentSeller);
                    result &= ValidatorWrapper.ValidateWithResult(new LocalizationValidator(), Localizations[0]);
                    if (result)
                    {
                        /* Update customer */
                        await CustomerService.UpdateSeller(CurrentSeller);

                        /* Update localization */
                        var currentLocalization = Localizations[0];

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
                        ShowWarning(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    ShowError("Błąd podczas aktualizacji danych");
                }
            });


        #endregion
    }
}
