using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class ComplaintsViewModel
    {
        #region Bindable properties

        public List<ComplaintFolderDTO> ComplaintFolderList { get; set; } = new List<ComplaintFolderDTO>();
        public List<ComplaintsDTO> ComplaintsList { get; set; } = new List<ComplaintsDTO>();
        public List<ComplaintDefinitionDTO> ComplaintDefinitionList { get; set; } = new List<ComplaintDefinitionDTO>();
        public List<ComplaintDefinitionDTO> AllComplaintDefinitions { get; private set; }

        public ComplaintDefinitionDTO CurrentComplaint { get; set; } = new ComplaintDefinitionDTO();
        public List<ComplaintFolderDTO> AllComplaintFolderList { get; set; }

        private ComplaintFolderDTO _selectedFolder;
        public ComplaintFolderDTO SelectedFolder
        {
            get => _selectedFolder;
            set
            {
                _selectedFolder = value;
                ComplaintDefinitionList = AllComplaintDefinitions.Where(p => p.ComplaintFolderId == value.Id).ToList();
                OnPropertyChanged(nameof(ComplaintDefinitionList));
            }
        }

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
