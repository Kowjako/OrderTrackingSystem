using OrderTrackingSystem.Interfaces;
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
    public class OrdersViewModel : IOrdersViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Services

        private readonly ConfigurationService ConfigService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Private objects & methods

        #endregion

        #region Bindable objects

        private PickupDTO _selectedPickup;
        public PickupDTO SelectedPickup
        {
            get => _selectedPickup;
            set
            {
                _selectedPickup = value;
                OnPropertyChanged(nameof(SelectedPickup));
            }
        }

        public CustomerDTO CurrentCustomer { get; private set; }
        public List<PickupDTO> PickupsList { get; set; } = new List<PickupDTO>();

        /* Tworzenie zamówienia */
        public OrderDTO CurrentOrder { get; set; } = new OrderDTO();

        #endregion

        #region Ctor

        public OrdersViewModel()
        {
            ConfigService = new ConfigurationService();
            CustomerService = new CustomerService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        { 
            CurrentCustomer = await CustomerService.GetCustomer((await CustomerService.GetCurrentCustomer()).Id);
            PickupsList = await ConfigService.GetPickupPoints();
        }

        #endregion

        #region Commands

        private RelayCommand _addToCart;

        public RelayCommand AddToCart =>
            _addToCart ?? (_addToCart = new RelayCommand(async obj =>
            {
                try
                {
                    var x = CurrentOrder;

                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Błąd podczas aktualizacji danych");
                }
            }));


        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
