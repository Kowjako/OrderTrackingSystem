using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface ITrackingViewModel : INotifyableViewModel
    {
        List<TrackableItemDTO> Items { get; set; }
        ObservableCollection<ParcelStateDTO> ParcelStates { get; set; }
        TrackableItemDTO SelectedItem { get; set; }

        CustomerDTO Customer { get; set; }
        CustomerDTO Seller { get; set; }

        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        int ItemsSelection { get; set; }

        Task SetInitializeProperties();

        RelayCommand FilterCommand { get; }
        RelayCommand FindParcel { get; }
        RelayCommand ShowProgress { get; }
    }
}
