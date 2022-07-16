using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using OrderTrackingSystem.Tests.ObjectFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Tests.ClassFixtures
{
    public class TrackerTestFixture : IDisposable
    {
        public ITrackerService TrackerService;
        public EntitiesGenerator EntitiesGenerator;

        public TrackerTestFixture()
        {
            EntitiesGenerator = new EntitiesGenerator();
            TrackerService = new TrackerService();
        }

        public void Dispose() { }
    }
}
