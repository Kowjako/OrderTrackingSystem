using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Tests.ObjectFactory
{
    public static class ObjectFactory
    {
        public static Localizations CreateLocalization()
        {
            return new Localizations()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "Reja",
                Flat = 15,
                House = 31,
                ZipCode = "50-339"
            };
        }
    }
}
