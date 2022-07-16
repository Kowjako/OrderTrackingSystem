using OrderTrackingSystem.Tests.ClassFixtures;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.Services;
using System.Linq;
using System;

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

        [Fact]
        public async void TrackTest_StartStateWhenChanged_ShouldThrowException()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDbAndSave();

            //act + assert
            foreach(var orderState in Enum.GetValues(typeof(OrderState)))
            {
                if ((int)orderState != (int)OrderState.GetFromSeller)
                {
                    await Assert.ThrowsAsync<InvalidOperationException>(async () => await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.ComplaintSet));
                }
            }
        }

        [Fact]
        public async void TrackTest_OrderStateReady_ShouldCreateComplaintSet()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDbAndSave();
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.GetFromSeller);
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.GetByLocal);
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.SentFromLocal);
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.ToDelivery);
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.ReadyToPickup);

            //act
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.ComplaintSet);
            var states = await context.TrackerService.GetParcelState(order.Id);

            //assert
            Assert.True(states.Last().StateId == (int)OrderState.ComplaintSet);
        }

        [Fact]
        public async void TrackTest_AddedStates_ShouldReturnAll()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDbAndSave();
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.GetFromSeller);
            await context.TrackerService.AddNewStateForOrder(order.Id, OrderState.GetByLocal);

            //act
            var states = await context.TrackerService.GetParcelState(order.Id);

            //assert
            Assert.Equal(3, states.Count);
        }
    }
}
