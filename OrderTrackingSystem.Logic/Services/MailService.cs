using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Services
{
    public class MailService : IService<MailService>
    {
        public async Task<MailDTO> GetSendMails(int sellerId)
        {

        }

        public async Task<MailDTO> GetReceivedMails(int receiverId)
        {

        }
    }
}
