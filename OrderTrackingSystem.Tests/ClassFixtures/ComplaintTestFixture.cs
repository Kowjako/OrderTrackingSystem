using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class ComplaintTestFixture : IDisposable
    {
        public IComplaintService ComplaintService;
        public EntitiesGenerator EntitiesGenerator;
        public IOrderService OrderService;
        public ICustomerService CustomerService;
        public IMailService MailService;

        public ComplaintTestFixture()
        {
            ComplaintService = new ComplaintService();
            EntitiesGenerator = new EntitiesGenerator();
            CustomerService = new CustomerService(new ConfigurationService());
            OrderService = new OrderService(CustomerService);
            MailService = new MailService();
        }

        public void Dispose() { }
    }
}
