using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Validators;
using OrderTrackingSystem.Presentation.Interfaces;
using OrderTrackingSystem.Presentation.ViewModels.Common;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public partial class MailboxViewModel : BaseViewModel, IMailboxViewModel
    {
        #region Services

        private readonly MailService MailService;
        private readonly CustomerService CustomerService;
        private readonly OrderService OrderService;

        #endregion

        #region Private variables

        private MailDirectionType MailDirection;

        #endregion

        #region Ctor

        public MailboxViewModel()
        {
            MailService = new MailService();
            CustomerService = new CustomerService(new ConfigurationService());
            OrderService = new OrderService();
        }

        #endregion

        #region Public methods

        public override async Task SetInitializeProperties()
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
                ShowWarning("Zamówienie o podanym numerze już dołączone");
                return;
            }
            if(SelectedOrder == null || string.IsNullOrEmpty(SelectedOrder.Number))
            {
                ShowWarning("Nie można dołączyć pustego zamówienia");
                return;
            }
            if (RelatedToCurrentMailOrders.Count == 3)
            {
                ShowWarning("Maksymalnie można dołączyć 3 zamówienia");
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
            _findReceiver ??= new RelayCommand(async obj =>
            {
                if (!string.IsNullOrEmpty(obj as string))
                {
                    /* Sprawdzamy czy to customer */
                    MailReceiver = await CustomerService.GetCustomerByName(obj as string);
                    /* Sprawdzamy czy to sklep */
                    if (MailReceiver == null)
                    {
                        MailReceiver = await CustomerService.GetSellerByName(obj as string);
                        if (MailReceiver == null)
                        {
                            ShowWarning("Nie istnieje takiej osoby/sklepu");
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
                    ShowWarning("Nazwa nie może być pusta");
                }
            });

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand =>
            _filterCommand ??= new RelayCommand(obj =>
            {
                if (DateFrom != DateTo && DateFrom < DateTo)
                {
                    //local function
                    bool filterCondition(MailDTO m) => m.SendDate <= DateTo && m.SendDate >= DateFrom;
                    
                    switch (SelectedFilterMsgType)
                    {
                        case 0:
                            SentMessages = SentMessages.Where(filterCondition).ToList();
                            OnPropertyChanged(nameof(SentMessages));
                            break;
                        case 1:
                            ReceivedMessages = SentMessages.Where(filterCondition).ToList();
                            OnPropertyChanged(nameof(ReceivedMessages));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ShowWarning("Proszę ustawić różne daty, oraz data początkowa musi być mniejsza niż końcowa");
                }
            });

        private RelayCommand _sendMessage;
        public RelayCommand SendMessage =>
            _sendMessage ??= new RelayCommand(async obj =>
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
                        ShowSuccess("Wiadomość pomyślnie wysłana");
                    }
                    else
                    {
                        ShowError(ValidatorWrapper.ErrorMessage);
                    }
                }
                catch (Exception)
                {
                    ShowError("Nie udało się wykonać operacji");
                }
            });

        #endregion
    }
}
