using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
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
    public partial class ComplaintsViewModel : IComplaintsViewModel, INotifyPropertyChanged
    {
        #region Services

        public IComplaintService ComplaintService;
        public ICustomerService CustomerService;

        #endregion

        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Private members

        private Customers CurrentCustomer;

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

        #region Ctor

        public ComplaintsViewModel()
        {
            ComplaintService = new ComplaintService();
            CustomerService = new CustomerService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            ComplaintFolderList = await ComplaintService.GetComplaintFolders();
            ComplaintsList = await ComplaintService.GetComplaintsForCustomer(CurrentCustomer.Id); //TODO: zrobic dla zalogowanego nabywcy
            ComplaintDefinitionList = await ComplaintService.GetComplaintDefinitions();
            AllComplaintFolderList = await ComplaintService.GetComplaintFoldersWithoutComposing();
            OnManyPropertyChanged(new[] { nameof(ComplaintFolderList), nameof(ComplaintsList), nameof(ComplaintDefinitionList), nameof(AllComplaintFolderList) });
        }

        #endregion

        #region Commands

        private RelayCommand _addTemplate;
        public RelayCommand AddTemplate =>
            _addTemplate ??= new RelayCommand(async obj =>
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
            });

        private RelayCommand _addFolder;
        public RelayCommand AddFolder =>
            _addFolder ??= new RelayCommand(async obj =>
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
            });

        private RelayCommand _removeFolder;
        public RelayCommand RemoveFolder =>
            _removeFolder ??= new RelayCommand(async obj =>
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
                    OnFailure?.Invoke("Nie udało się usunąć folderów");
                }
            });

        private RelayCommand _closeComplaint;
        public RelayCommand CloseComplaint =>
            _closeComplaint ??= new RelayCommand(async obj =>
            {
                if(SelectedComplaint.SolutionDate.HasValue)
                {
                    var complaintDAL = new ComplaintStates() { Id = SelectedComplaint.Id };
                    await ComplaintService.CloseComplaint(complaintDAL);
                    OnSuccess?.Invoke("Reklamacja pomyślnie zamknięta");
                }
                else
                {
                    OnWarning?.Invoke("Reklamacja musi najpierw być zatwierdzona poprzez sprzedawcę");
                }
            });
        #endregion
    }
}
