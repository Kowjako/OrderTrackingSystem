using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.DatabaseFixture;
using Xunit;
using Xunit.Extensions.Ordering;
using OF = OrderTrackingSystem.Tests.ObjectFactory;

namespace OrderTrackingSystem.Tests.LocalizationServiceTests
{
    [Collection("DBCollection")]
    public class LocalizationsTests
    {
        ILocalizationService service;

        public LocalizationsTests()
        {
            service = new LocalizationService();
        }

        [Fact, Order(1)]
        public async void LocTests_ProvideLocalization_AddLocalization()
        {
            //arrange
            var localization = OF.ObjectFactory.CreateLocalization();

            //act
            await service.AddNewLocalization(localization);

            //assert
            Assert.True(localization.Id > 0);
        }

        [Fact, Order(2)]
        public async void LocTests_ProvideLocalization_UpdateLocalization()
        {
            //arrange
            var modifiedStreet = "Prusa";
            var localization = OF.ObjectFactory.CreateLocalization();

            //act
            await service.AddNewLocalization(localization);

            localization.Street = modifiedStreet; //update street

            await service.UpdateLocalization(localization);
            await service.GetLocalizationById(localization.Id);

            //assert
            Assert.Equal(localization.Street, modifiedStreet);
        }

        [Fact, Order(3)]
        public async void LocTests_GivenId_ShouldReturnLocalization()
        {
            //arrange
            var localization = OF.ObjectFactory.CreateLocalization();

            //act
            await service.AddNewLocalization(localization);
            var gettedLocalization = service.GetLocalizationById(localization.Id);

            //assert
            Assert.NotNull(gettedLocalization);
        }
    }
}
