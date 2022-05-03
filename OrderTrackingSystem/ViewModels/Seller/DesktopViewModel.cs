using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.Interfaces.Seller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion

        #region Bindable properties

        public List<MailDTO> SentMessages { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public List<OrderDTO> CustomersOrder { get; set; }
        public List<ComplaintsDTO> CustomersComplaint { get; set; }
        public List<Tuple<string, OrderState>> ParcelAvailableStates { get; set; }

        private OrderDTO _selectedOrder;
        public OrderDTO SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
                SetAvailableStates(SelectedOrder.CurrentOrderState);
            }
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentSeller = await CustomerService.GetCurrentSeller();
            ParcelAvailableStates = new List<Tuple<string, OrderState>>(ConfigurationService.GetAllStates());
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
