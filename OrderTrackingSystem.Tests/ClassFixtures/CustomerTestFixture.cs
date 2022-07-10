using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class CustomerTestFixture : IDisposable
    {
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;

        public CustomerTestFixture()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
        }

        public void Dispose() { }
    }
}
