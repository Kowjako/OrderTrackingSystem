﻿using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class SellerAccountViewModel : ISellerAccountViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

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
            CustomerService = new CustomerService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
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
            _saveCommand ?? (_saveCommand = new RelayCommand(async obj =>
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
                    else
                    {
                        OnFailure?.Invoke(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Błąd podczas aktualizacji danych");
                }
            }));


        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
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
