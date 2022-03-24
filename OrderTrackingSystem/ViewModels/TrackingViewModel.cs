using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
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
            var currentCustomer = await CustomerService.GetCurrentCustomer();
            Items = await TrackerService.GetItemsForCustomer(currentCustomer.Id);
        }

        #endregion

        #region Private methods

        private async void OnSelectedItemChanged()
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
