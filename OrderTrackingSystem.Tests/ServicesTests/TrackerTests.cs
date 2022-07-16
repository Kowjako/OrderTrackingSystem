using OrderTrackingSystem.Tests.ClassFixtures;
using OF = OrderTrackingSystem.Tests.ObjectFactory;
using Xunit;

namespace OrderTrackingSystem.Tests.ServicesTests
{
    [Collection("DBCollection")]
    public class TrackerTests : IClassFixture<TrackerTestFixture>
    {
        TrackerTestFixture context;

        public TrackerTests(TrackerTestFixture fixture)
        {
            this.context = fixture;
        }
    }
}
