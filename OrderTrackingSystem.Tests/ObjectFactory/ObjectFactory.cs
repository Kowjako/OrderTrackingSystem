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
                Email = Guid.NewGuid().ToString(),
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

        public static Products CreateProduct()
        {
            return new Products()
            {
                Name = "Paracetamol",
                PriceNetto = 40.85m,
                VAT = 23,
                PriceBrutto = 45.85m,
                Category = 1,
                Weight = 45m,
                Discount = 0,
            };
        }
    }
}
