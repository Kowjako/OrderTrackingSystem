using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    interface IComplaintsViewModel
    {
        List<ComplaintFolderDTO> ComplaintFolderList { get; set; }
        List<ComplaintsDTO> ComplaintsList { get; set; }
        List<ComplaintDefinitionDTO> ComplaintDefinitionList { get; set; }
        ComplaintDefinitionDTO CurrentComplaint { get; set; }
        List<ComplaintFolderDTO> AllComplaintFolderList { get; set; }
        ComplaintFolderDTO SelectedFolder { get; set; }
        ComplaintFolderDTO SelectedFolderToAdd { get; set; }
        ComplaintsDTO SelectedComplaint { get; set; }

        Task SetInitializeProperties();

        RelayCommand AddTemplate { get; }
        RelayCommand AddFolder { get; }
        RelayCommand RemoveFolder { get; }
    }
}
