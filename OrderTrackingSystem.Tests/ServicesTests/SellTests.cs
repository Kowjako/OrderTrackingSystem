using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using OrderTrackingSystem.Logic.Services.Interfaces;
using Moq;
using System.Threading.Tasks;
using OrderTrackingSystem.Logic.Services;
using System;
using System.Linq;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class SellTests : IClassFixture<SellTestFixture>
    {
        SellTestFixture context;

        public SellTests(SellTestFixture fixture)
        {
            context = fixture;
        }

        [Fact]
        public async void SellTest_ProvidedDataWithGreaterAmount_ShouldThrowException()
        {
            //arrange
            (var sell, var product, var customer) = await context.EntitiesGenerator.AddNewSellToDb();

            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            cartElem2.Amount = 100;
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };

            //mocking
            var customerServiceMock = Mock.Of<ICustomerService>(ld => ld.GetCurrentCustomer() == Task.FromResult(customer));
            context.SellService = new SellService(customerServiceMock);

            //act + assert - kwota wysylki > kwota konta
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await context.SellService.SaveSell(sell, elemList));
        }

        [Fact]
        public async void SellTest_ProvidedData_ShouldSaveOrder()
        {
            //arrange
            (var sell, var product, var customer) = await context.EntitiesGenerator.AddNewSellToDb();

            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2 };

            //mocking
            var configurationServiceMock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer.Id));
            context.SellService = new SellService(new CustomerService(configurationServiceMock));

            //act
            await context.SellService.SaveSell(sell, elemList);
            var newCustomer = await context.CustomerService.GetCustomer(customer.Id);

            //assert
            Assert.True(sell.Id > 0);
            Assert.Equal(450.0m - 45.85m * 2, newCustomer.Balance);
        }

        [Fact]
        public async void SellTest_AddedSell_ShouldReturnForCustomer()
        {
            //arrange
            (var sell, var product, var customer) = await context.EntitiesGenerator.AddNewSellToDb();
            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2 };
            await context.SellService.SaveSell(sell, elemList);

            //act
            var sells = await context.SellService.GetSellsForCustomer(customer.Id);

            //assert
            Assert.Contains(sell.Id, sells.Select(p => p.Id));
        }
    }
}
