using Moq;
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
            var customer = OF.ObjectFactory.CreateCustomer();
            var localization = OF.ObjectFactory.CreateLocalization();
            await locService.AddNewLocalization(localization);

            await service.AddNewCustomer(customer, localization.Id, ("login", "pass"));

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
            var customer = OF.ObjectFactory.CreateCustomer();
            var localization = OF.ObjectFactory.CreateLocalization();
            await locService.AddNewLocalization(localization);
            await service.AddNewCustomer(customer, localization.Id, ("login", "pass"));

            var mock = Mock.Of<IConfigurationService>(ld => ld.GetCurrentSessionId() == Task.FromResult(customer.Id));
            service = new CustomerService(mock);

            //act
            var result = await service.GetCurrentCustomer();

            //assert
            Assert.Equal(customer.Id, result.Id);
        }
    }
}
