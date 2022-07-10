using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class OrderTestFixture : IDisposable
    {
        public EntitiesGenerator EntitiesGenerator;
        public IOrderService OrderService;

        public OrderTestFixture()
        {
            EntitiesGenerator = new EntitiesGenerator();
            OrderService = new OrderService();
        }

        public void Dispose() { }
    }
}
