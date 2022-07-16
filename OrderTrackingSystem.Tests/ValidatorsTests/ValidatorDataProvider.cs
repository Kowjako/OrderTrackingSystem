using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.EnumMappers;
using System;
using System.Collections.Generic;

namespace OrderTrackingSystem.Tests.ValidatorsTests
{
    public class ValidatorDataProvider
    {
        public static IEnumerable<object[]> GetSampleCustomersWithResult()
        {
            yield return new object[] {new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "Kowjako",
                Age = 21,
                Number = "575-275-533",
                Email = "kowyako@yandex.ru"
            }, true};
            yield return new object[] {new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "Kowjako",
                Age = 0,
                Number = "575-275-533",
                Email = "kowyako@yandex.ru"
            }, false};
            yield return new object[] {new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "Kowjako",
                Age = 0,
                Number = "575-275-533",
                Email = "notemailaddress"
            }, false};
            yield return new object[] {new Customers()
            {
                Name = "",
                Surname = "Kowjako",
                Age = 21,
                Number = "575-275-533",
                Email = "kowyako@yandex.ru"
            }, false};
            yield return new object[] {new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "",
                Age = 21,
                Number = "575-275-533",
                Email = "kowyako@yandex.ru"
            }, false};
            yield return new object[] {new Customers()
            {
                Name = "Wlodzimierz",
                Surname = "Kowjako",
                Age = 21,
                Number = "",
                Email = "kowyako@yandex.ru"
            }, false};
            yield return new object[] {new Customers()
            {
                Name = "",
                Surname = "",
                Age = 21,
                Number = "",
                Email = "kowyako@yandex.ru"
            }, false};
        }

        public static IEnumerable<object[]> GetSampleLocalizationsWithResult()
        {
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "Reja",
                Apartment = 9,
                House = 16,
                ZipCode = "50-338"
            }, true};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "Reja",
                Apartment = -1,
                House = 16,
                ZipCode = "50-338"
            }, false};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "Reja",
                Apartment = 9,
                House = -1,
                ZipCode = "50-338"
            }, false};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "",
                City = "Wroclaw",
                Street = "Reja",
                Apartment = 9,
                House = 16,
                ZipCode = "50-338"
            }, false};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "",
                Street = "Reja",
                Apartment = 9,
                House = 16,
                ZipCode = "50-338"
            }, false};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "",
                Apartment = 9,
                House = 16,
                ZipCode = "50-338"
            }, false};
            yield return new object[] { new LocalizationDTO()
            {
                Country = "Poland",
                City = "Wroclaw",
                Street = "Reja",
                Apartment = 9,
                House = 16,
            }, true};
        }

        public static IEnumerable<object[]> GetSampleMailWithResult()
        {
            yield return new object[] {new MailDTO()
            {
                Caption = "Naglowek",
                Content = "Tresc",
                ReceiverId = 3
            }, true};
            yield return new object[] {new MailDTO()
            {
                Caption = "Naglowek",
                Content = "Tresc",
                ReceiverId = -2
            }, false};
            yield return new object[] {new MailDTO()
            {
                Caption = "",
                Content = "Tresc",
                ReceiverId = 3
            }, false};
            yield return new object[] {new MailDTO()
            {
                Caption = "Naglowek",
                ReceiverId = 3
            }, false};
        }

        public static IEnumerable<object[]> GetSampleOrdersWithResult()
        {
            yield return new object[] {new OrderDTO()
            {
                PickupDTO = new PickupDTO(),
                DeliveryType = EnumConverter.GetNameById<DeliveryType>((int)DeliveryType.Courier),
                PayType =  EnumConverter.GetNameById<PayType>((int)PayType.BLIK),
                CartProducts = new System.ComponentModel.BindingList<CartProductDTO>(new List<CartProductDTO>()
                {
                    new CartProductDTO() { Amount = 2m, Price = 48.50m, Discount = 0m },
                    new CartProductDTO { Amount = 5m, Price = 48.50m, Discount = 2m }
                })
            }, true};
            yield return new object[] {new OrderDTO()
            {
                PickupDTO = null,
                DeliveryType = EnumConverter.GetNameById<DeliveryType>((int)DeliveryType.Courier),
                PayType =  EnumConverter.GetNameById<PayType>((int)PayType.BLIK),
                CartProducts = new System.ComponentModel.BindingList<CartProductDTO>(new List<CartProductDTO>()
                {
                    new CartProductDTO() { Amount = 0m, Price = 48.50m, Discount = 0m },
                    new CartProductDTO { Amount = 5m, Price = 48.50m, Discount = 2m }
                })
            }, false};
            yield return new object[] {new OrderDTO()
            {
                PickupDTO = null,
                DeliveryType = EnumConverter.GetNameById<DeliveryType>((int)DeliveryType.Courier),
                PayType =  EnumConverter.GetNameById<PayType>((int)PayType.BLIK),
                CartProducts = new System.ComponentModel.BindingList<CartProductDTO>(new List<CartProductDTO>()
                {
                    new CartProductDTO() { Amount = 2m, Price = 0m, Discount = 0m },
                    new CartProductDTO { Amount = 5m, Price = 48.50m, Discount = 2m }
                })
            }, false};
            yield return new object[] {new OrderDTO()
            {
                PickupDTO = null,
                DeliveryType = EnumConverter.GetNameById<DeliveryType>((int)DeliveryType.Courier),
                PayType =  EnumConverter.GetNameById<PayType>((int)PayType.BLIK),
                CartProducts = new System.ComponentModel.BindingList<CartProductDTO>(new List<CartProductDTO>()
                {
                    new CartProductDTO() { Amount = 2m, Price = 0m, Discount = -2m },
                    new CartProductDTO { Amount = 5m, Price = 48.50m, Discount = 2m }
                })
            }, false};
        }

        public static IEnumerable<object[]> GetSampleProductsWithResult()
        {
            yield return new object[] {new Products()
            {
                Name = "Nimesil",
                PriceNetto = 25m,
                VAT = 15,
                Weight = 5.05m,
                Discount = 0,
                Category = 1
            }, true};
            yield return new object[] {new Products()
            {
                Name = string.Empty,
                PriceNetto = 25m,
                VAT = 15,
                Weight = 5.05m,
                Discount = 0,
                Category = 1
            }, false};
            yield return new object[] {new Products()
            {
                Name = "Nimesil",
                PriceNetto = 25m,
                VAT = 50,
                Weight = 5.05m,
                Discount = 0,
                Category = 1
            }, false};
            yield return new object[] {new Products()
            {
                Name = string.Empty,
                PriceNetto = 25m,
                VAT = 15,
                Weight = -200m,
                Discount = 0,
                Category = 1
            }, false};
            yield return new object[] {new Products()
            {
                Name = string.Empty,
                PriceNetto = 25m,
                VAT = 15,
                Weight = 5m,
                Discount = 0,
                Category = -1
            }, false};
        }

        public static IEnumerable<object[]> GetSampleSellerWithResult()
        {
            yield return new object[] {new Sellers()
            {
                Name = "Pasaz Grunwaldzki Sp. z o.o.",
                Email = "pasaz@google.com",
                Number = "572-123-213",
                TIN = "5678192012"
            }, true};
            yield return new object[] {new Sellers()
            {
                Name = "",
                Email = "pasaz@google.com",
                Number = "572-123-213",
                TIN = "5678192012"
            }, false};
            yield return new object[] {new Sellers()
            {
                Name = "Pasaz Grunwaldzki Sp. z o.o.",
                Email = "noetmail",
                Number = "572-123-213",
                TIN = "5678192012"
            }, false};
            yield return new object[] {new Sellers()
            {
                Name = "Pasaz Grunwaldzki Sp. z o.o.",
                Email = "pasaz@google.com",
                Number = "",
                TIN = "5678192012"
            }, false};
            yield return new object[] {new Sellers()
            {
                Name = "Pasaz Grunwaldzki Sp. z o.o.",
                Email = "pasaz@google.com",
                Number = "572-123-213",
                TIN = "5678192012233321"
            }, false};
        }

        public static IEnumerable<object[]> GetSampleVoucherWithResult()
        {
            yield return new object[] {new VoucherDTO()
            {
                ExpireDate = DateTime.Now.AddDays(5),
                Value = 150m
            }, true};
            yield return new object[] {new VoucherDTO()
            {
                ExpireDate = DateTime.Now.AddDays(-5),
                Value = 150m
            }, false};
            yield return new object[] {new VoucherDTO()
            {
                ExpireDate = DateTime.Now.AddDays(5),
                Value = 0m
            }, false};
            yield return new object[] {new VoucherDTO()
            {
                ExpireDate = DateTime.Now.AddDays(5),
                Value = -10m
            }, false};
        }
    }
}
