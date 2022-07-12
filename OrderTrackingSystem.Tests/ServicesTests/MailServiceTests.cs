using OrderTrackingSystem.Tests.ClassFixtures;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;
using OrderTrackingSystem.Tests.DatabaseFixture;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class MailServiceTests : IClassFixture<MailTestFixture>
    {
        DBFixture db;
        MailTestFixture context;

        public MailServiceTests(DBFixture db, MailTestFixture fixture)
        {
            this.db = db;
            context = fixture;
        }
    }
}
