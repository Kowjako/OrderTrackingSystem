using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.ViewModels
{
    public class CurrentAccountViewModel
    {
        #region Services

        private readonly CustomerService CustomerService;
        private readonly LocalizationService LocalizationService;

        #endregion

        #region Bindable objects

        public Customers CurrentCustomer { get; set; }
        public LocalizationDTO[] Localization { get; set; }

        #endregion

        #region Ctor

        public CurrentAccountViewModel()
        {
            CustomerService = new CustomerService();
            LocalizationService = new LocalizationService();
            Localization = new LocalizationDTO[1];
        }

        #endregion

        #region Commands

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(obj =>
            {
                //CustomerService.Update(CurrentCustomer);
                //if (Localization != null && Localization.Any())
                //{
                //    LocalizationService.Update(Localization.First());
                //}
            }));

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            Localization[0] = await LocalizationService.GetLocalizationById(CurrentCustomer.LocalizationId);
        }

        #endregion
    }
}
