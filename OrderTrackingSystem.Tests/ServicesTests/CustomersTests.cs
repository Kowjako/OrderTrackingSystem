using Moq;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ClassFixtures;
using OrderTrackingSystem.Tests.DatabaseFixture;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;
using OF = OrderTrackingSystem.Tests.ObjectFactory;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class CustomersTests : IClassFixture<CustomerTestFixture>
    {
        DBFixture db;
        CustomerTestFixture context;

        public CustomersTests(DBFixture db, CustomerTestFixture fixture)
        {
            this.db = db;
            context = fixture;
        }

        [Fact, Order(1)]
        public async void CusTests_ProvidedData_ShouldAddCustomer()
        {
            //arrange
            var customer = OF.ObjectFactory.CreateCustomer();
            var localization = OF.ObjectFactory.CreateLocalization();
            await context.LocalizationService.AddNewLocalization(localization);

            //act
            await context.CustomerService.AddNewCustomer(customer, localization.Id, ("login", "pass"));

            //assert
            Assert.True(customer.Id > 0);
        }

        [Fact, Order(2)]
        public async void CusTests_ProvidedName_ShouldReturnByName()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var correctResult = await context.CustomerService.GetCustomerByName(customer.Name + " " + customer.Surname);
            var uncorrectResult = await context.CustomerService.GetCustomerByName(string.Empty);

            //assert
            Assert.NotNull(correctResult);
            Assert.Null(uncorrectResult);
        }

        [Fact, Order(3)]
        public async void CusTests_NoData_ReturnLoggedCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            var mock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer.Id));
            context.CustomerService = new CustomerService(mock);

            //act
            var result = await context.CustomerService.GetCurrentCustomer();

            //assert
            Assert.Equal(customer.Id, result.Id);
        }

        [Fact, Order(4)]
        public async void CusTests_NoData_ReturnLoggedSeller()
        {
            //arrange
            var seller = await AddNewSellerToDb();

            var mock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(seller.Id));
            context.CustomerService = new CustomerService(mock);

            //act
            var result = await context.CustomerService.GetCurrentSeller();

            //assert
            Assert.Equal(seller.Id, result.Id);
        }

        [Fact, Order(5)]
        public async void CusTests_GivenEmain_ReturnsCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var result = await context.CustomerService.GetCustomerByMail(customer.Email);

            //assert
            Assert.True(result.Id == customer.Id);
        }

        [Fact, Order(6)]
        public async void CusTests_ChangeData_ChangeDbEntityCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();
            customer.Name += "XXL";
            var changedName = customer.Name;

            //act
            await context.CustomerService.UpdateCustomer(customer);

            //assert
            Assert.Equal(customer.Name, changedName);
        }

        [Fact, Order(7)]
        public async void CusTests_ChangeData_ChangeDbEntitySellers()
        {
            //arrange
            var seller = await AddNewSellerToDb();
            seller.Name += "XXL";
            var changedName = seller.Name;

            //act
            await context.CustomerService.UpdateSeller(seller);

            //assert
            Assert.Equal(seller.Name, changedName);
        }

        [Fact, Order(8)]
        public async void CusTests_ProvidedId_ReturnsCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var correctData = await context.CustomerService.GetCustomer(customer.Id);
            var incorrectData = await context.CustomerService.GetCustomer(10000);

            //assert
            Assert.NotNull(correctData);
            Assert.Null(incorrectData);
        }

        [Fact, Order(9)]
        public async void CusTests_FillCustomers_ShouldReturnAll()
        {
            //arrange
            var customer1 = await AddNewCustomerToDb();
            var customer2 = await AddNewCustomerToDb();
            var customer3 = await AddNewCustomerToDb();
            var ids = new[] { customer1.Id, customer2.Id, customer3.Id };

            //act
            var customers = await context.CustomerService.GetAllCustomers();

            //assert
            Assert.All(customers, item => Assert.True(item.Id.In(ids)));
        }

        [Fact, Order(10)]
        public async void CusTests_ProvidedName_ShouldReturnSeller()
        {
            //arrange
            var seller = await AddNewSellerToDb();

            //act
            var gettedSeller = await context.CustomerService.GetSellerByName("DOZ.pl");
            var gettedSeller2 = await context.CustomerService.GetSellerByName("doz.pl");

            //assert
            Assert.Equal(gettedSeller.Id, gettedSeller2.Id);
        }

        #region Private methods

        private async Task<int> CreateLocalization()
        {
            var localization = OF.ObjectFactory.CreateLocalization();
            await context.LocalizationService.AddNewLocalization(localization);
            return localization.Id;
        }

        private async Task<Customers> AddNewCustomerToDb()
        {
            var customer = OF.ObjectFactory.CreateCustomer();
            await context.CustomerService.AddNewCustomer(customer, await CreateLocalization(), ("login", "pass"));
            return customer;
        }

        private async Task<Sellers> AddNewSellerToDb()
        {
            var seller = OF.ObjectFactory.CreateSeller();
            await context.CustomerService.AddNewSeller(seller, await CreateLocalization(), ("login", "pass"));
            return seller;
        }

        #endregion
    }
}
