using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class ComplaintsViewModel : INotifyPropertyChanged, INotifyableViewModel
    {
        #region Services

        public ComplaintService ComplaintService;

        #endregion

        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void OnManyPropertyChanged(IEnumerable<string> props)
        {
            foreach(var prop in props)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion

        #region Bindable properties

        public List<ComplaintFolderDTO> ComplaintFolderList { get; set; } = new List<ComplaintFolderDTO>();
        public List<ComplaintsDTO> ComplaintsList { get; set; } = new List<ComplaintsDTO>();
        public List<ComplaintDefinitionDTO> ComplaintDefinitionList { get; set; } = new List<ComplaintDefinitionDTO>();

        #endregion

        #region Ctor

        public ComplaintsViewModel()
        {
            ComplaintService = new ComplaintService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            ComplaintFolderList = await ComplaintService.GetComplaintFolders();
            ComplaintsList = await ComplaintService.GetComplaints();
            ComplaintDefinitionList = await ComplaintService.GetComplaintDefinitions();
            OnManyPropertyChanged(new[] { nameof(ComplaintFolderList), nameof(ComplaintsList), nameof(ComplaintDefinitionList) });
        }

        #endregion
    }
}
