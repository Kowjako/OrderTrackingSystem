using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class TrackingViewModel : ITrackingViewModel, INotifyPropertyChanged
    {
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
                            Items = Items.Where(p => p.CustomerId == CurrentCustomer.Id).ToList();
                            break;
                        case 2:
                            Items = Items.Where(p => p.SellerId == CurrentCustomer.Id).ToList();
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
                    Items = new List<TrackableItemDTO>() { Items.FirstOrDefault(p => p.Numer.Equals(obj as string)) };
                    OnPropertyChanged(nameof(Items));
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

        #endregion
    }
}
