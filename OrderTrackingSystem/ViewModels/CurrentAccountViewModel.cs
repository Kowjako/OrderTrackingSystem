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

        public CurrentAccountViewModel()
        {
            CustomerService = new CustomerService();
            CurrentCustomer = CustomerService.GetByPrimary(2);
        }

        public Customers CurrentCustomer { get; set; }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new RelayCommand(obj =>
            {
                CustomerService.Update(CurrentCustomer);
            }));
    }
}
