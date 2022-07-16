using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class ProductsTestFixture : IDisposable
    {
        public IProductService ProductService;
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;
        public EntitiesGenerator EntitiesGenerator;
        public IOrderService OrderService;
        public ISellService SellService;

        public ProductsTestFixture()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            ProductService = new ProductService(new ConfigurationService());
            EntitiesGenerator = new EntitiesGenerator();
            OrderService = new OrderService(CustomerService);
            SellService = new SellService(CustomerService);
        }

        public void Dispose() { }
    }
}
