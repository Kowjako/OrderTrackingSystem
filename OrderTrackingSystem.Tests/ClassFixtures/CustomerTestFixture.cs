using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class CustomerTestFixture : IDisposable
    {
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;
        public EntitiesGenerator EntitiesGenerator;

        public CustomerTestFixture()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            EntitiesGenerator = new EntitiesGenerator();
        }

        public void Dispose() { }
    }
}
