using OrderTrackingSystem.Interfaces;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.Interfaces
{
    interface IMailboxViewModel
    {
        MailDTO OriginalMail { get; set; }
        CustomerDTO CurrentSender { get; set; }
        List<MailDTO> ReceivedMessages { get; set; }
        List<MailDTO> SentMessages { get; set; }
        CustomerDTO MailReceiver { get; set; }
        ObservableCollection<string> RelatedToCurrentMailOrders { get; set; }
        List<OrderDTO> CustomerOrders { get; set; }
        OrderDTO SelectedOrder { get; set; }
        MailDTO SelectedMail { get; set; }

        Task SetInitializeProperties();

        RelayCommand FindReceiver { get; }
        RelayCommand FilterCommand { get; }
        RelayCommand SendMessage { get; }
    }
}
