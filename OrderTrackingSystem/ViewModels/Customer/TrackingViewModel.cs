using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class TrackingViewModel : ITrackingViewModel, INotifyPropertyChanged
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Services

        private readonly TrackerService TrackerService;
        private readonly CustomerService CustomerService;
        private readonly ComplaintService ComplaintService;
        private readonly MailService MailService;

        #endregion

        #region Private propeties

        public Customers CurrentCustomer { get; private set; }
        private List<TrackableItemDTO> CurrentItems { get; set; }

        #endregion

        #region Ctor

        public TrackingViewModel()
        {
            TrackerService = new TrackerService();
            CustomerService = new CustomerService();
            ComplaintService = new ComplaintService();
            MailService = new MailService();
        }
        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            Items = await TrackerService.GetItemsForCustomer(CurrentCustomer.Id);
            CurrentItems = Items;
            ComplaintDefinitionsList = await ComplaintService.GetComplaintDefinitions();
            OnManyPropertyChanged(new[] { nameof(CurrentCustomer), 
                                          nameof(Items), 
                                          nameof(CurrentItems), 
                                          nameof(ComplaintDefinitionsList) });
        }

        #endregion

        #region Commands

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    Items = CurrentItems;
                    switch(ItemsSelection)
                    {
                        case 1:
                            Items = Items.Where(p => p.IsOrder).ToList();
                            break;
                        case 2:
                            Items = Items.Where(p => !p.IsOrder).ToList();
                            break;
                        default:
                            break;
                    }

                    if(StartDate != EndDate)
                    {
                        Items = Items.Where(p => p.Date < EndDate && 
                                                 p.Date > StartDate).ToList();
                    }
                    OnPropertyChanged(nameof(Items));
                }
                catch (Exception)
                {

                }
            });

        private RelayCommand _findParcel;
        public RelayCommand FindParcel =>
            _findParcel ??= new RelayCommand(obj =>
            {
                if (!string.IsNullOrEmpty(obj as string))
                {
                    if (!Items.Any(p => p.Number.Equals(obj as string)))
                    {
                        OnWarning("Nie ma elementu o takim numerze");
                        return;
                    }
                    Items = new List<TrackableItemDTO>() { Items.FirstOrDefault(p => p.Number.Equals(obj as string)) };
                    OnPropertyChanged(nameof(Items));
                }
                else
                {
                    OnWarning("Numer nie może być pusty");
                }
            });

        private RelayCommand _showProgress;
        public RelayCommand ShowProgress =>
            _showProgress ??= new RelayCommand(async obj =>
            {
                if (SelectedItem.IsOrder)
                {
                    var parcelId = SelectedItem.Id;
                    /* Load current selected parcel states */
                    ParcelStates = new ObservableCollection<ParcelStateDTO>(await TrackerService.GetParcelState(parcelId));
                    OnPropertyChanged(nameof(ParcelStates));
                }
                else
                {
                    OnWarning("Progres można zobaczyć tylko dla zamówień");
                }
            });

        private RelayCommand _makeComplaint;
        public RelayCommand MakeComplaint =>
            _makeComplaint ??= new RelayCommand(async obj =>
            {
                try
                {
                    if (SelectedItem.IsOrder)
                    {
                        var orderId = SelectedItem.Id;
                        var transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted };
                        using(var transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                        {
                            /* Dodanie kolejnego statusu na zalozenie reklamacji */
                            await TrackerService.AddNewStateForOrder(orderId, OrderState.ComplaintSet);
                            /* Dodac reklamacje */
                            await ComplaintService.RegisterNewComplaint(SelectedComplaint.Id, orderId);
                            if(SentMessageWithComplaint)
                            {
                                /* wysylamy automatyczne powiadomienie */
                                var receiverId = SelectedItem.SellerId;
                                await MailService.SendComplaintMessage(receiverId, SelectedItem.CustomerId, SelectedItem.Id);
                            }
                            transactionScope.Complete();
                        }
                        OnSuccess("Reklamacja założona poprawnie");
                    }
                    else
                    {
                        OnWarning("Rezygnację można złożyć tylko na zamówienia.");
                    }
                }
                catch
                {
                    OnFailure("Błąd podczas założenia reklamacji");
                }
            });

        private RelayCommand _confirmDelivery;
        public RelayCommand ConfirmDelivery =>
            _confirmDelivery ??= new RelayCommand(async obj =>
            {
                if (SelectedItem.IsOrder)
                {
                    var parcelStates = await TrackerService.GetParcelState(SelectedItem.Id);
                    if(!parcelStates.Any(p => p.StateId == (int)OrderState.Getted))
                    {
                        await TrackerService.AddNewStateForOrder(SelectedItem.Id, OrderState.Getted);
                        OnSuccess("Odbiór został potwierdzony.");
                    }
                    else
                    {
                        OnWarning("Przesyłka już jest odebrana.");
                    } 
                }
                else
                {
                    OnWarning("Potwierdzenie odbioru dostępne dla zamówionych elementów.");
                }
            });

        #endregion

        #region Private methods

        private async void OnSelectedItemChanged()
        {
            if (_selectedItem != null)
            {
                Customer = await CustomerService.GetCustomer(_selectedItem.CustomerId);
                if (_selectedItem.IsOrder)
                {
                    Seller = await CustomerService.GetSeller(_selectedItem.SellerId);
                }
                else
                {
                    Seller = await CustomerService.GetCustomer(_selectedItem.SellerId);
                }

                /* Update UI bindings */
                OnPropertyChanged(nameof(Customer));
                OnPropertyChanged(nameof(Seller));
            }
        }

        #endregion

        #region INotifyPropertyChanged

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
    }
}
