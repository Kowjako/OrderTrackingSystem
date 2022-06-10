using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class ComplaintsViewModel
    {
        #region Bindable properties

        public List<ComplaintFolderDTO> ComplaintFolderList { get; set; } = new List<ComplaintFolderDTO>();
        public List<ComplaintsDTO> ComplaintsList { get; set; } = new List<ComplaintsDTO>();
        public List<ComplaintDefinitionDTO> ComplaintDefinitionList { get; set; } = new List<ComplaintDefinitionDTO>();

        public ComplaintDefinitionDTO CurrentComplaint { get; set; } = new ComplaintDefinitionDTO();
        public List<ComplaintFolderDTO> AllComplaintFolderList { get; set; }
        public ComplaintFolderDTO SelectedFolder { get; set; }
        public ComplaintFolderDTO SelectedFolderToAdd { get; set; }

        public string FolderToAddName { get; set; }
        public byte SelectedFolderDeleteType { get; set; } = 2;
        public int SelectedComplaintState { get; set; } = 0;
        public List<DateTime?> SelectedComplaintStateDates { get; set; }

        private ComplaintsDTO _selectedComplaint;
        public ComplaintsDTO SelectedComplaint
        {
            get => _selectedComplaint;
            set
            {
                _selectedComplaint = value;
                SelectedComplaintState = value.StateId;
                SelectedComplaintStateDates = value.ComplaintStateDates.Where(p => p.HasValue).ToList();
                OnManyPropertyChanged(new[] { nameof(SelectedComplaintState), nameof(SelectedComplaintStateDates) });
            }
        }
        #endregion
    }
}
