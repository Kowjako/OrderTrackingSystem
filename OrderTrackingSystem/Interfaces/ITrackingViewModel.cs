using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface ITrackingViewModel
    {
        List<TrackableItemDTO> Items { get; set; }
        TrackableItemDTO SelectedItem { get; set; }

        CustomerDTO Customer { get; set; }
        CustomerDTO Seller { get; set; }

        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        int ItemsSelection { get; set; }

        Task SetInitializeProperties();

        RelayCommand FilterCommand { get; }
    }
}
