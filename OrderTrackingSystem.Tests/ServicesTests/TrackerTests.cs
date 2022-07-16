using OrderTrackingSystem.Tests.ClassFixtures;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.Services;
using System.Linq;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class TrackerTests : IClassFixture<TrackerTestFixture>
    {
        TrackerTestFixture context;

        public TrackerTests(TrackerTestFixture fixture)
        {
            this.context = fixture;
        }

        [Fact]
        public async void TrackTest_AddedSellsAndOrders_ShouldReturnAll()
        {
            //arrange
            (var sell, var product, var customer) = await context.EntitiesGenerator.AddNewSellToDbAndSave();
            await context.EntitiesGenerator.AddNewOrderToDbAndSave(customer.Id);
            await context.EntitiesGenerator.AddNewOrderToDbAndSave(customer.Id);
            await context.EntitiesGenerator.AddNewOrderToDbAndSave(customer.Id);

            //act
            var list = await context.TrackerService.GetItemsForCustomer(customer.Id);

            //assert
            Assert.Equal(4, list.Count);
        }

        [Fact]
        public async void TrackTest_OrderAdded_CheckState()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDbAndSave();

            //act
            var list = await context.TrackerService.GetParcelState(order.Id);

            //assert
            Assert.Single(list);
            Assert.Equal((int)OrderState.PrepatedBySeller, list.First().StateId);
        }
    }
}
