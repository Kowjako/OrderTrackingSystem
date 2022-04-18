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

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class TrackingViewModel : ITrackingViewModel, INotifyPropertyChanged, INotifyableViewModel
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Services

        private readonly TrackerService TrackerService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Private propeties

        public Customers CurrentCustomer { get; private set; }
        private List<TrackableItemDTO> CurrentItems { get; set; }

        #endregion

        #region Bindable objects

        public List<TrackableItemDTO> Items { get; set; } = new List<TrackableItemDTO>();
        public ObservableCollection<ParcelStateDTO> ParcelStates { get; set; } = new ObservableCollection<ParcelStateDTO>();

        private TrackableItemDTO _selectedItem;

        public TrackableItemDTO SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnSelectedItemChanged();
            }
        }

        public CustomerDTO Customer { get; set; }
        public CustomerDTO Seller { get; set; }

        /* Filtering bindings */
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int ItemsSelection { get; set; }

        #endregion

        #region Ctor

        public TrackingViewModel()
        {
            TrackerService = new TrackerService();
            CustomerService = new CustomerService();
        }
        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentCustomer = await CustomerService.GetCurrentCustomer();
            Items = await TrackerService.GetItemsForCustomer(CurrentCustomer.Id);
            CurrentItems = Items;
            OnManyPropertyChanged(new[] { nameof(CurrentCustomer), nameof(Items), nameof(CurrentItems) });
        }

        #endregion

        #region Commands

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ?? (_filterCommand = new RelayCommand(obj =>
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
                        Items = Items.Where(p => DateTime.Parse(p.Data) < EndDate && 
                                                 DateTime.Parse(p.Data) > StartDate).ToList();
                    }
                    OnPropertyChanged(nameof(Items));
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand _findParcel;
        public RelayCommand FindParcel =>
            _findParcel ?? (_findParcel = new RelayCommand(obj =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(obj as string))
                    {
                        if(!Items.Any(p => p.Numer.Equals(obj as string)))
                        {
                            OnWarning("Nie ma elementu o takim numerze");
                            return;
                        }
                        Items = new List<TrackableItemDTO>() { Items.FirstOrDefault(p => p.Numer.Equals(obj as string)) };
                        OnPropertyChanged(nameof(Items));
                    }
                    else
                    {
                        OnWarning("Numer nie może być pusty");
                    }
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand _showProgress;
        public RelayCommand ShowProgress =>
            _showProgress ?? (_showProgress = new RelayCommand(async obj =>
            {
                try
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
                }
                catch (Exception)
                {

                }
            }));

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
