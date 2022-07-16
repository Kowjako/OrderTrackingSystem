using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class ComplaintsViewModel : BaseViewModel, IComplaintsViewModel
    {
        #region Services

        public IComplaintService ComplaintService;
        public ICustomerService CustomerService;

        #endregion

        #region Private members

        private Customers CurrentCustomer;

        #endregion

        #region Ctor

        public ComplaintsViewModel()
        {
            ComplaintService = new ComplaintService();
            CustomerService = new CustomerService(new ConfigurationService());
        }

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            ComplaintFolderList = await ComplaintService.GetComplaintFolders();
            ComplaintsList = await ComplaintService.GetComplaintsForCustomer(CurrentCustomer.Id); //TODO: zrobic dla zalogowanego nabywcy
            ComplaintDefinitionList = await ComplaintService.GetComplaintDefinitions();
            AllComplaintFolderList = await ComplaintService.GetComplaintFoldersWithoutComposing();
            AllComplaintDefinitions = ComplaintDefinitionList;
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
                        await ComplaintService.SaveComplaintTemplate(CurrentComplaint, SelectedFolder.Id);
                        ShowSuccess("Wzorzec został zapisany");
                    }
                    else
                    {
                        ShowWarning("Należy wybrać katalog gdzie wzorzec umieścić");
                    }
                }
                catch (Exception)
                {
                    ShowError("Nie udało się zapisać wzorca");
                }
            });

        private RelayCommand _addFolder;
        public RelayCommand AddFolder =>
            _addFolder ??= new RelayCommand(async obj =>
            {
                try
                {
                    await ComplaintService.AddNewFolder(FolderToAddName, SelectedFolderToAdd);
                    ShowSuccess("Folder został dodany");
                }
                catch (Exception)
                {
                    ShowError("Nie udało się zapisać wzorca");
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
                    ShowSuccess("Folder usunięty pomyślnie");
                }
                catch (Exception ex)
                {
                    ShowError("Nie udało się usunąć folderów");
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
                    ShowSuccess("Reklamacja pomyślnie zamknięta");
                }
                else
                {
                    ShowWarning("Reklamacja musi najpierw być zatwierdzona poprzez sprzedawcę");
                }
            }, (e) => SelectedComplaint != null && !SelectedComplaint.EndDate.HasValue);
        #endregion
    }
}
