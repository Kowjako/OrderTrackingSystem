using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using Xunit;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class OrdersTests : IClassFixture<OrderTestFixture>
    {
        DBFixture db;
        OrderTestFixture context;

        public OrdersTests(DBFixture db, OrderTestFixture fixture)
        {
            this.db = db;
            context = fixture;
        }
    }
}
