using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
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
    }
}
