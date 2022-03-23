using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class TrackingViewModel : ITrackingViewModel
    {
        #region Services

        private readonly TrackerService TrackerService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Bindable objects

        public List<TrackableItemDTO> Items { get; set; } = new List<TrackableItemDTO>();

        #endregion

        #region Ctor

        public TrackingViewModel()
        {
            TrackerService = new TrackerService();
            CustomerService = new CustomerService();
        }
        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            var currentCustomer = await CustomerService.GetCurrentCustomer();
            Items = await TrackerService.GetItemsForCustomer(currentCustomer.Id);
        }

        #endregion
    }
}
