using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.ViewModels
{
    public class CurrentAccountViewModel
    {
        private readonly IBusinessService<Customers> CustomerService;
        private readonly LocalizationService LocalizationService;

        public CurrentAccountViewModel()
        {
            CustomerService = new CustomerService();
            LocalizationService = new LocalizationService();
            CurrentCustomer = CustomerService.GetByPrimary(2);
            Localization = new List<LocalizationRow>();

            Localization.Add(LocalizationService.GetLocalizationRowById(CurrentCustomer.LocalizationId));
        }

        public Customers CurrentCustomer { get; set; }
        public List<LocalizationRow> Localization { get; set; }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(obj =>
            {
                CustomerService.Update(CurrentCustomer);
                if(Localization != null && Localization.Any())
                {
                    LocalizationService.Update(Localization.First());
                }
            }));
    }
}
