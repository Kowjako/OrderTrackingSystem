using MaterialDesignThemes.Wpf;
using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class MailboxViewModel : IMailboxViewModel, INotifyableViewModel, INotifyPropertyChanged
    {
        #region Services

        private readonly MailService MailService;
        private readonly CustomerService CustomerService;
        private readonly OrderService OrderService;

        #endregion

        #region Private variables

        private MailDirectionType MailDirection;

        #endregion

        #region Bindable objects

        public MailDTO OriginalMail { get; set; } = new MailDTO();
        public CustomerDTO CurrentSender { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public List<MailDTO> SentMessages { get; set; }
        public CustomerDTO MailReceiver { get; set; }
        public ObservableCollection<string> RelatedToCurrentMailOrders { get; set; } = new ObservableCollection<string>();
        public List<OrderDTO> CustomerOrders { get; set; }
        public OrderDTO SelectedOrder { get; set; }

        public DateTime DateFrom { get; set; } = DateTime.Now;
        public DateTime DateTo { get; set; } = DateTime.Now;
        public int SelectedFilterMsgType = -1;
        public DesignerSerializationVisibility SplitterVisibility { get; set; } = DesignerSerializationVisibility.Hidden;

        public double ActualMailHeight { get; set; } = 0;

        private MailDTO _selectedMail;
        public MailDTO SelectedMail
        {
            get => _selectedMail;
            set
            {
                _selectedMail = value;
                OnPropertyChanged(nameof(SelectedMail));
                ActualMailHeight = Double.NaN; /* equals to Height = Auto */
                SplitterVisibility = DesignerSerializationVisibility.Visible;
                OnPropertyChanged(nameof(ActualMailHeight));
                OnPropertyChanged(nameof(SplitterVisibility));
            }
        }

        #endregion

        #region Ctor

        public MailboxViewModel()
        {
            MailService = new MailService();
            CustomerService = new CustomerService();
            OrderService = new OrderService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentSender = await CustomerService.GetCustomer((await CustomerService.GetCurrentCustomer()).Id);
            ReceivedMessages = await MailService.GetReceivedMailsForCustomer(CurrentSender.Id);
            SentMessages = await MailService.GetSendMailsForCustomer(CurrentSender.Id);
            CustomerOrders = await OrderService.GetOrdersForCustomer(CurrentSender.Id);
            OnManyPropertyChanged(new[] { nameof(CurrentSender), nameof(ReceivedMessages), nameof(SentMessages), nameof(CustomerOrders) });
        }

        public void OnLinkToOrderAdded()
        {
            if(RelatedToCurrentMailOrders.Any(i => i.Equals(SelectedOrder.Number)))
            {
                OnWarning?.Invoke("Zamówienie o podanym numerze już dołączone");
                return;
            }
            if(SelectedOrder == null || string.IsNullOrEmpty(SelectedOrder.Number))
            {
                OnFailure?.Invoke("Nie można dołączyć pustego zamówienia");
                return;
            }
            if (RelatedToCurrentMailOrders.Count == 3)
            {
                OnFailure?.Invoke("Maksymalnie można dołączyć 3 zamówienia");
                return;
            }

            RelatedToCurrentMailOrders.Add(SelectedOrder.Number);
            RelatedToCurrentMailOrders = new ObservableCollection<string>(RelatedToCurrentMailOrders);
            OnPropertyChanged(nameof(RelatedToCurrentMailOrders));
        }

        #endregion

        #region Commands

        private RelayCommand _findReceiver;
        public RelayCommand FindReceiver =>
            _findReceiver ?? (_findReceiver = new RelayCommand(async obj =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(obj as string))
                    {
                        /* Sprawdzamy czy to customer */
                        MailReceiver = await CustomerService.GetCustomerByName(obj as string);
                        /* Sprawdzamy czy to sklep */
                        if(MailReceiver == null)
                        {
                            MailReceiver = await CustomerService.GetSellerByName(obj as string);
                            if (MailReceiver == null)
                            {
                                OnWarning?.Invoke("Nie istnieje takiej osoby/sklepu");
                                return;
                            }
                            MailDirection = MailDirectionType.CustomerToSeller;
                        }
                        else
                        {
                            MailDirection = MailDirectionType.CustomerToCustomer;
                        }                        
                        OnPropertyChanged(nameof(MailReceiver));
                    }
                    else
                    {
                        OnWarning?.Invoke("Nazwa nie może być pusta");
                    }
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ?? (_filterCommand = new RelayCommand(obj =>
            {
                try
                {
                    if (DateFrom != DateTo && DateFrom < DateTo)
                    {
                        switch (SelectedFilterMsgType)
                        {
                            case 0:
                                SentMessages = SentMessages.Where(p => p.SendDate <= DateTo &&
                                                                       p.SendDate >= DateFrom).ToList();
                                OnPropertyChanged(nameof(SentMessages));
                                break;
                            case 1:
                                ReceivedMessages = SentMessages.Where(p => p.SendDate <= DateTo &&
                                                                           p.SendDate >= DateFrom).ToList();
                                OnPropertyChanged(nameof(ReceivedMessages));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        OnWarning?.Invoke("Proszę ustawić różne daty, oraz data początkowa musi być mniejsza niż końcowa");
                    }
                }
                catch (Exception)
                {

                }
            }));

        private RelayCommand _sendMessage;
        public RelayCommand SendMessage =>
            _sendMessage ?? (_sendMessage = new RelayCommand(async obj =>
            {
                try
                {
                    ValidatorWrapper.Validate(new MailValidator(), OriginalMail);
                    if(ValidatorWrapper.IsValid)
                    {
                        OriginalMail.SellerId = CurrentSender.Id;
                        OriginalMail.ReceiverId = MailReceiver.Id;
                        OriginalMail.MailRelation = (byte)MailDirection;
                        await MailService.SendMail(OriginalMail, RelatedToCurrentMailOrders.ToArray());
                        OnSuccess?.Invoke("Wiadomość pomyślnie wysłana");
                    }
                    else
                    {
                        OnFailure?.Invoke(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    OnFailure?.Invoke("Nie udało się wykonać operacji");
                }
            }));

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
    }
}
