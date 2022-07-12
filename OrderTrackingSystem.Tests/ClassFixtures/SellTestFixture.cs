using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class SellTestFixture : IDisposable
    {
        public EntitiesGenerator EntitiesGenerator;
        public ISellService SellService;
        public ICustomerService CustomerService;
        public IProductService ProductService;

        public SellTestFixture()
        {
            EntitiesGenerator = new EntitiesGenerator();
            CustomerService = new CustomerService(new ConfigurationService());
            SellService = new SellService(CustomerService);
            ProductService = new ProductService(new ConfigurationService());
        }

        public void Dispose() { }
    }
}
