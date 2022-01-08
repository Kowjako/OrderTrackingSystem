using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.ViewModels
{
    public class CustomerViewModel
    {
        private readonly IBusinessService<Customers> CustomerService;

        public CustomerViewModel()
        {
            CustomerService = new CustomerService();
            CurrentCustomer = CustomerService.GetByPrimary(2);
        }

        public Customers CurrentCustomer { get; set; }
    }
}
