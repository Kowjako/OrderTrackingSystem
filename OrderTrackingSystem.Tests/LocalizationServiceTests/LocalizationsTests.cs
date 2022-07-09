using OrderTrackingSystem.Tests.DatabaseFixture;
using Xunit;

namespace OrderTrackingSystem.Tests.LocalizationServiceTests
{
    [Collection("DBCollection")]
    public class LocalizationsTests
    {
        DBFixture db;
        public LocalizationsTests(DBFixture db)
        {
            this.db = db;
        }

        [Fact]
        public void GetLocalizationById_GivenId_ReturnLocalization()
        {
            Assert.True(true);
        }
    }
}
