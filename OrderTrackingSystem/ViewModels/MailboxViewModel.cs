using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Bindable objects

        public MailDTO OriginalMail { get; set; }
        public CustomerDTO CurrentSender { get; set; }
        public List<MailDTO> ReceivedMessages { get; set; }
        public List<MailDTO> SentMessages { get; set; }
        public CustomerDTO MailReceiver { get; set; }

        private MailDTO _selectedMail;
        public MailDTO SelectedMail
        {
            get => _selectedMail;
            set
            {
                _selectedMail = value;
                OnPropertyChanged(nameof(SelectedMail));
            }
        }

        #endregion

        #region Ctor

        public MailboxViewModel()
        {
            MailService = new MailService();
            CustomerService = new CustomerService();
        }

        #endregion

        #region Public methods

        public async Task SetInitializeProperties()
        {
            CurrentSender = await CustomerService.GetCustomer((await CustomerService.GetCurrentCustomer()).Id);
            ReceivedMessages = await MailService.GetReceivedMailsForCustomer(CurrentSender.Id);
            SentMessages = await MailService.GetSendMailsForCustomer(CurrentSender.Id);
        }

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

        #endregion
    }
}
