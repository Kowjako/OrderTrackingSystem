using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert; /* Assert bedzie uzywany z XUnit */

namespace OrderTrackingSystem.Tests
{
    [TestClass]
    public class APITester
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.Equal(1, 2);
        }
    }
}
