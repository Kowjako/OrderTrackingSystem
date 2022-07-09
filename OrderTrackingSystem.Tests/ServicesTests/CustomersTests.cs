using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.DatabaseFixture;
using Xunit;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class CustomersTests
    {
        DBFixture db;
        ICustomerService service;

        public CustomersTests(DBFixture db)
        {
            this.db = db;
            service = new CustomerService();
        }

        [Fact, Order(1)]
        public async void 

    }
}
