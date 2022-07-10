using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class ProductsTestFixture : IDisposable
    {
        public IProductService ProductService;
        public ICustomerService CustomerService;
        public ILocalizationService LocalizationService;

        public ProductsTestFixture()
        {
            CustomerService = new CustomerService(new ConfigurationService());
            LocalizationService = new LocalizationService();
            ProductService = new ProductService();
        }

        public void Dispose() { }
    }
}
