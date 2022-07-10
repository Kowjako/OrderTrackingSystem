using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System.Linq;

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

        [Fact]
        public async void OrderTest_ProvidedData_ShouldSaveOrder()
        {
            //arrange
            var order = OF.ObjectFactory.CreateOrder();
            var pickup = await context.EntitiesGenerator.AddNewPickupToDb();
            var seller = await context.EntitiesGenerator.AddNewSellerToDb();
            var customer = await context.EntitiesGenerator.AddNewCustomerToDb();
            var product = OF.ObjectFactory.CreateProduct();

            order.SellerId = seller.Id;
            order.CustomerId = customer.Id;
            order.PickupId = pickup.Id;

            product.SellerId = seller.Id;
            await context.ProductService.SaveNewProduct(product);

            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };

            //act
            await context.OrderService.SaveOrder(order, elemList, 100.0m);
            var newCustomer = await context.CustomerService.GetCustomer(customer.Id);

            //assert
            Assert.True(order.Id > 0);
            Assert.Equal(350.0m, newCustomer.Balance);
        }
    }
}
