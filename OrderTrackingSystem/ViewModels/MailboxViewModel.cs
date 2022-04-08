using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Presentation.ViewModels
{
    public class MailboxViewModel : IMailboxViewModel, INotifyableViewModel
    {
        #region Services

        private readonly MailService MailService;
        private readonly CustomerService CustomerService;

        #endregion

        #region Bindable objects

        public MailDTO OriginalMail { get; set; }
        public CustomerDTO CurrentSender { get; set; }

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
        }

        #endregion

        #region INotifyableViewModel implementation

        public event Action<string> OnSuccess;
        public event Action<string> OnFailure;
        public event Action<string> OnWarning;

        #endregion

    }
}
