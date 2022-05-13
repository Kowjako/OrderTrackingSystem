using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class ComplaintsViewModel : INotifyPropertyChanged, INotifyableViewModel
    {
        #region Services

        public IComplaintService ComplaintService;

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
            foreach (var prop in props)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion

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
            ComplaintsList = await ComplaintService.GetComplaintsForCustomer(1); //TODO: zrobic dla zalogowanego nabywcy
            ComplaintDefinitionList = await ComplaintService.GetComplaintDefinitions();
            AllComplaintFolderList = await ComplaintService.GetComplaintFoldersWithoutComposing();
            OnManyPropertyChanged(new[] { nameof(ComplaintFolderList), nameof(ComplaintsList), nameof(ComplaintDefinitionList), nameof(AllComplaintFolderList) });
        }

        #endregion

        #region Commands

        private RelayCommand _addTemplate;
        public RelayCommand AddTemplate =>
            _addTemplate ?? (_addTemplate = new RelayCommand(async obj =>
            {
                try
                {
                    if(SelectedFolder != null)
                    {
                        await ComplaintService.SaveComplaintTemplate(CurrentComplaint, SelectedFolder);
                        OnSuccess?.Invoke("Wzorzec został zapisany");
                    }
                    else
                    {
                        OnWarning?.Invoke("Należy wybrać katalog gdzie wzorzec umieścić");
                    }
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Nie udało się zapisać wzorca");
                }
            }));

        private RelayCommand _addFolder;
        public RelayCommand AddFolder =>
            _addFolder ?? (_addFolder = new RelayCommand(async obj =>
            {
                try
                {
                    await ComplaintService.AddNewFolder(FolderToAddName, SelectedFolderToAdd);
                    OnSuccess?.Invoke("Folder został dodany");
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Nie udało się zapisać wzorca");
                }
            }));

        private RelayCommand _removeFolder;
        public RelayCommand RemoveFolder =>
            _removeFolder ?? (_removeFolder = new RelayCommand(async obj =>
            {
                try
                {
                    switch(SelectedFolderDeleteType)
                    {
                        case 0:
                            await ComplaintService.DeleteWithAncestor(SelectedFolder);
                            break;
                        case 1:
                            await ComplaintService.DeleteAndMoveToAncestor(SelectedFolder);
                            break;
                        default:
                            break;
                    }
                    OnSuccess?.Invoke("Folder usunięty pomyślnie");
                }
                catch (Exception ex)
                {
                    OnFailure?.Invoke("Nie udało się zapisać wzorca");
                }
            }));

        #endregion
    }
}
