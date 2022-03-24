using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class OrdersViewModel : IOrdersViewModel
    {
        #region Services

        private readonly ConfigurationService ConfigService;
        private readonly CustomerService CustomerService;

        #endregion


        #region Bindable objects

        public CustomerDTO CurrentCustomer { get; private set; }
        public List<PickupDTO> PickupsList { get; set; } = new List<PickupDTO>();

        #endregion

        #region Ctor

        public OrdersViewModel()
        {
            ConfigService = new ConfigurationService();
            CustomerService = new CustomerService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        { 
            CurrentCustomer = await CustomerService.GetCustomer((await CustomerService.GetCurrentCustomer()).Id);
            PickupsList = await ConfigService.GetPickupPoints();
        }

        #endregion

    }
}
