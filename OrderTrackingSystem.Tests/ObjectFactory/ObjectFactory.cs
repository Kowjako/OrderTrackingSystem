using OrderTrackingSystem.Logic.DataAccessLayer;
using System;

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

        public static Customers CreateCustomer()
        {
            return new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "Kowjako",
                Age = 18,
                Number = "111222333",
                Email = "kowyako@sample.com",
                Balance = 450.0m
            };
        }

        public static Sellers CreateSeller()
        {
            return new Sellers()
            {
                Name = "DOZ.pl",
                OpenDate = DateTime.Now,
                TIN = "1234567890",
                Number = "123456612",
                Email = "dozpl@google.com"
            };
        }
    }
}
