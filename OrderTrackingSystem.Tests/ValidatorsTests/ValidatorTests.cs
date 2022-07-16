using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using OrderTrackingSystem.Logic.Validators;
using System;
using Xunit;

namespace OrderTrackingSystem.Tests.ValidatorsTests
{
    public class ValidatorTests : IDisposable
    {
        public ValidatorTests() { }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleCustomersWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void CustomerValidator_ValidateEntity(Customers customer, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new CustomerValidator(), customer);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleLocalizationsWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void LocalizationValidator_ValidateEntity(LocalizationDTO loc, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new LocalizationValidator(), loc);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleMailWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void MailValidator_ValidateEntity(MailDTO mail, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new MailValidator(), mail);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleOrdersWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void OrderValidator_ValidateEntity(OrderDTO order, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new OrderValidator(), order);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleProductsWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void ProductValidator_ValidateEntity(Products product, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new ProductValidator(), product);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleSellerWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void SellerValidator_ValidateEntity(Sellers seller, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new SellerValidator(), seller);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        [Theory]
        [MemberData(nameof(ValidatorDataProvider.GetSampleVoucherWithResult), MemberType = typeof(ValidatorDataProvider))]
        public void VoucherValidator_ValidateEntity(VoucherDTO voucher, bool shouldBeValid)
        {
            //arrange

            //act
            ValidatorWrapper.Validate(new VoucherValidator(), voucher);

            //assert
            Assert.True(ValidatorWrapper.IsValid == shouldBeValid);
        }

        public void Dispose() { }
    }
}
