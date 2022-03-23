using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    public interface ITrackingViewModel
    {
        List<TrackableItemDTO> Items { get; set; }

        Task SetInitializeProperties();
    }
}
