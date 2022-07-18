using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Logic.Services;
using System.Threading.Tasks;
using OrderTrackingSystem.Tests.ClassFixtures;
using Moq;
using System.Linq;
using OrderTrackingSystem.Logic.DTO;
using System.Collections.Generic;
using System;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class ProductsTests : IClassFixture<ProductsTestFixture>
    {
        ProductsTestFixture context;

        public ProductsTests(ProductsTestFixture fixture)
        {
            context = fixture;
        }

        [Fact]
        public async void ProdTests_GivenData_MakesProduct()
        {
            //arrange
            var product = await context.EntitiesGenerator.CreateProductWithSeller();

            //act
            await context.ProductService.SaveNewProduct(product);

            //assert
            Assert.True(product.Id > 0);
        }

        [Fact]
        public async void ProdTests_AddedProducts_ReturnAllProducts()
        {
            //arrange
            var p1 = await context.EntitiesGenerator.CreateProductWithSeller();
            var p2 = await context.EntitiesGenerator.CreateProductWithSeller();
            var p3 = await context.EntitiesGenerator.CreateProductWithSeller();

            await context.ProductService.SaveNewProduct(p1);
            await context.ProductService.SaveNewProduct(p2);
            await context.ProductService.SaveNewProduct(p3);

            var ids = new[] { p1.Id, p2.Id, p3.Id }.ToList();

            //act
            var products = await context.ProductService.GetAllProducts();

            //assert
            Assert.All(ids, id => Assert.Contains(id, products.Select(p => p.Id)));
        }

        [Fact]
        public async void ProdTests_ProvidedData_GenerateVouchers()
        {
            //arrange
            var customer = await context.EntitiesGenerator.AddNewCustomerToDb();
            var customer2 = await context.EntitiesGenerator.AddNewCustomerToDb();
            var voucher = OF.ObjectFactory.CreateVoucher();

            //act
            await context.ProductService.GenerateVouchersForCustomer(voucher, new[] { customer.Id, customer2.Id });

            var confServiceMock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer.Id));
            context.ProductService = new ProductService(confServiceMock);
            var vouchers = await context.ProductService.GetVouchersForCurrentCustomer();

            confServiceMock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer2.Id));
            context.ProductService = new ProductService(confServiceMock);
            var vouchers2 = await context.ProductService.GetVouchersForCurrentCustomer();

            //assert
            Assert.True(vouchers.Count == 1);
            Assert.True(vouchers2.Count == 1);
        }

        [Fact]
        public async void ProdTests_AddedOrderCart_ShouldSaveOrderCart()
        {
            //arrange
            (var order, var product, var customer) = await context.EntitiesGenerator.AddNewOrderToDb();

            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };

            var mock = Mock.Of<ICustomerService>(ld => ld.GetCurrentCustomer() == Task.FromResult(customer));
            context.OrderService = new OrderService(mock);
            await context.OrderService.SaveOrder(order, elemList, 100.0m);

            //act
            var list = await context.OrderService.GetOrdersForCustomer(customer.Id);
            var expectedSum = Math.Round(product.PriceNetto + product.PriceNetto * product.VAT / 100.0m, 2, MidpointRounding.ToEven) * 4;

            //assert
            Assert.Equal(expectedSum, list.FirstOrDefault().Value);
        }

        [Fact]
        public async void ProdTests_AddedOrderCart_ShouldSaveSellCart()
        {
            //arrange
            (var sell, var product, var customer) = await context.EntitiesGenerator.AddNewSellToDb();

            var cartElem2 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var cartElem3 = OF.ObjectFactory.CreateCartProduct(product.Id);
            var elemList = new List<CartProductDTO>() { cartElem2, cartElem3 };

            var mock = Mock.Of<ICustomerService>(ld => ld.GetCurrentCustomer() == Task.FromResult(customer));
            context.SellService = new SellService(mock);
            await context.SellService.SaveSell(sell, elemList);

            //act
            var list = await context.SellService.GetSellsForCustomer(customer.Id);
            var expectedSum = Math.Round(product.PriceNetto + product.PriceNetto * product.VAT / 100.0m, 2, MidpointRounding.ToEven) * 4;

            //assert
            Assert.Equal(expectedSum, list.FirstOrDefault().Value);
        }
    }
}
