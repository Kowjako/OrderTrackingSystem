using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using EnumConverter = OrderTrackingSystem.Logic.EnumMappers.EnumConverter;

namespace OrderTrackingSystem.Presentation.ViewModels.Seller
{
    public class DesktopViewModel : IDesktopViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

        #region Services

        private readonly MailService MailService;
        private readonly ComplaintService ComplaintService;
        private readonly OrderService OrderService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Ctor

        public DesktopViewModel()
        {
            MailService = new MailService();
            ComplaintService = new ComplaintService();
            OrderService = new OrderService();
            CustomerService = new CustomerService();
        }

        #endregion

        #region Private members

        private Sellers CurrentSeller { get; set; }

        private void SetAvailableStates()
        {
            var context = new FSMContext((OrderState)SelectedOrder.CurrentOrderState); /* ustawiamy obecny stan automatu */
            ParcelAvailableStates.Clear();
            foreach (var state in context.State.GetNextStates())
            {
                ParcelAvailableStates.Add(new Tuple<string, OrderState, int>
                                         (EnumConverter.GetNameById<OrderState>((int)state.Item1), state.Item1, state.Item2));
                ParcelAvailableStates = new List<Tuple<string, OrderState, int>>(ParcelAvailableStates);
            }
            OnPropertyChanged(nameof(ParcelAvailableStates));
        }

        #endregion

        #region Bindable properties

        public List<MailDTO> SentMessages { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public List<OrderDTO> CustomersOrder { get; set; }
        public List<ComplaintsDTO> CustomersComplaint { get; set; }
        public List<Tuple<string, OrderState, int>> ParcelAvailableStates { get; set; } = new List<Tuple<string, OrderState, int>>();
        public Tuple<string, OrderState, int> SelectedState { get; set; }

        private OrderDTO _selectedOrder;
        public OrderDTO SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                SetAvailableStates();
            }
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentSeller = await CustomerService.GetCurrentSeller();
            CustomersOrder = await OrderService.GetOrdersFromCompany(CurrentSeller.Id);
            CustomersComplaint = await ComplaintService.GetComplaintsForSeller(CurrentSeller.Id);
            ReceivedMessages = await MailService.GetReceivedMailsForSeller(CurrentSeller.Id);
            SentMessages = await MailService.GetSendMailsForSeller(CurrentSeller.Id);
            OnManyPropertyChanged(new[] {nameof(ParcelAvailableStates), nameof(CustomersOrder), nameof(CustomersComplaint),
                                         nameof(ReceivedMessages), nameof(SentMessages)});
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
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
