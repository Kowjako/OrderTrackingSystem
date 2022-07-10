using Moq;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.DatabaseFixture;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions.Ordering;
using OF = OrderTrackingSystem.Tests.ObjectFactory;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class CustomersTests
    {
        DBFixture db;
        ICustomerService service;
        ILocalizationService locService;

        public CustomersTests(DBFixture db)
        {
            this.db = db;
            service = new CustomerService(new ConfigurationService());
            locService = new LocalizationService();
        }

        [Fact, Order(1)]
        public async void CusTests_ProvidedData_ShouldAddCustomer()
        {
            //arrange
            var customer = OF.ObjectFactory.CreateCustomer();
            var localization = OF.ObjectFactory.CreateLocalization();
            await locService.AddNewLocalization(localization);

            //act
            await service.AddNewCustomer(customer, localization.Id, ("login", "pass"));

            //assert
            Assert.True(customer.Id > 0);
        }

        [Fact, Order(2)]
        public async void CusTests_ProvidedName_ShouldReturnByName()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var correctResult = await service.GetCustomerByName(customer.Name + " " + customer.Surname);
            var uncorrectResult = await service.GetCustomerByName(string.Empty);

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
            service = new CustomerService(mock);

            //act
            var result = await service.GetCurrentCustomer();

            //assert
            Assert.Equal(customer.Id, result.Id);
        }

        [Fact, Order(4)]
        public async void CusTests_NoData_ReturnLoggedSeller()
        {
            //arrange
            var seller = await AddNewSellerToDb();

            var mock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(seller.Id));
            service = new CustomerService(mock);

            //act
            var result = await service.GetCurrentSeller();

            //assert
            Assert.Equal(seller.Id, result.Id);
        }

        [Fact, Order(5)]
        public async void CusTests_GivenEmain_ReturnsCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var result = await service.GetCustomerByMail(customer.Email);

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
            await service.UpdateCustomer(customer);

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
            await service.UpdateSeller(seller);

            //assert
            Assert.Equal(seller.Name, changedName);
        }

        [Fact, Order(8)]
        public async void CusTests_ProvidedId_ReturnsCustomer()
        {
            //arrange
            var customer = await AddNewCustomerToDb();

            //act
            var correctData = await service.GetCustomer(customer.Id);
            var incorrectData = await service.GetCustomer(10000);

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
            var customers = await service.GetAllCustomers();

            //assert
            Assert.All(customers, item => Assert.True(item.Id.In(ids)));
        }

        #region Private methods

        private async Task<int> CreateLocalization()
        {
            var localization = OF.ObjectFactory.CreateLocalization();
            await locService.AddNewLocalization(localization);
            return localization.Id;
        }

        private async Task<Customers> AddNewCustomerToDb()
        {
            var customer = OF.ObjectFactory.CreateCustomer();
            await service.AddNewCustomer(customer, await CreateLocalization(), ("login", "pass"));
            return customer;
        }

        private async Task<Sellers> AddNewSellerToDb()
        {
            var seller = OF.ObjectFactory.CreateSeller();
            await service.AddNewSeller(seller, await CreateLocalization(), ("login", "pass"));
            return seller;
        }

        #endregion
    }
}
