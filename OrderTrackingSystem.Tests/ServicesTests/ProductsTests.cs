using OrderTrackingSystem.Tests.DatabaseFixture;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Logic.Services;
using System.Threading.Tasks;
using OrderTrackingSystem.Tests.ClassFixtures;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class ProductsTests : IClassFixture<ProductsTestFixture>
    {
        DBFixture db;
        ProductsTestFixture context;

        public ProductsTests(DBFixture db, ProductsTestFixture fixture)
        {
            this.db = db;
            context = fixture;
        }

        [Fact]
        public async void ProdTests_GivenData_MakesProduct()
        {
            //arrange
            var product = await CreateProductWithSeller();

            //act
            await context.ProductService.SaveNewProduct(product);

            //assert
            Assert.True(product.Id > 0);
        }

        #region Private methods

        private async Task<Products> CreateProductWithSeller()
        {
            var seller = OF.ObjectFactory.CreateSeller();
            var localization = OF.ObjectFactory.CreateLocalization();
            await context.LocalizationService.AddNewLocalization(localization);
            await context.CustomerService.AddNewSeller(seller, localization.Id, ("login", "pass"));

            var product = OF.ObjectFactory.CreateProduct();
            product.SellerId = seller.Id;
            return product;
        }

        #endregion
    }
}
