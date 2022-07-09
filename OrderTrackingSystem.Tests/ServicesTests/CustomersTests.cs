using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.DatabaseFixture;
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
            service = new CustomerService();
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
            var correctResult = await service.GetCustomerByName(customer.Name);
            var uncorrectResult = await service.GetCustomerByName(string.Empty);

            //assert
            Assert.NotNull(correctResult);
            Assert.Null(uncorrectResult);
        }
    }
}
