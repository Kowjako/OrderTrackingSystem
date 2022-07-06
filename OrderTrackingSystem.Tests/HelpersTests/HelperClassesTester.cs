using OrderTrackingSystem.Logic.HelperClasses;
using Xunit;
using System;
using System.Diagnostics;
using Xunit.Abstractions;
using OrderTrackingSystem.Logic.EnumMappers;

namespace OrderTrackingSystem.Tests.HelpersTests
{

    public class HelperClassesTester : IDisposable
    {
        private readonly ITestOutputHelper Printer;

        public HelperClassesTester(ITestOutputHelper p)
        {
            Printer = p;
        }

        [Theory(DisplayName = "Cryptography_Encrypt/DecryptRSA")]
        [InlineData("QWERYIOPASDGJKLCXZVB<NM!@#%#^&*)(12415425216713")]
        [InlineData("")]
        [InlineData("1234567890")]
        [InlineData("~!@#$%^&*()_+|?><")]
        public void Cryptography_Encrypt_Decrypt(string passwordData)
        {
            //arrange
            var password = passwordData;

            //act
            var encrypted = Cryptography.EncryptWithRSA(password);
            var decrypted = Cryptography.DecryptFromRSA(encrypted);

            //assert
            Assert.Equal(password, decrypted);
        }

        [Theory(DisplayName = "EnumMapper_GetNameById_PayType")]
        [InlineData((int)PayType.ApplePay, "Apple Pay")]
        [InlineData((int)PayType.BLIK, "BLIK")]
        [InlineData((int)PayType.Card, "Karta")]
        [InlineData((int)PayType.Cash, "Gotówka")]
        public void EnumMapper_PayType(int id, string expectedName)
        {
            //arrange
            var result = expectedName;

            //act
            var converter = EnumConverter.GetNameById<PayType>(id);

            //assert
            Assert.Equal(converter, result);
        }

        [Theory(DisplayName = "EnumMapper_GetNameById_DeliveryType")]
        [InlineData((int)DeliveryType.Courier, "Kurier")]
        [InlineData((int)DeliveryType.Paczkomat, "Paczkomat")]
        [InlineData((int)DeliveryType.Post, "Poczta")]
        [InlineData((int)DeliveryType.Takeself, "Odbiór osobisty")]
        public void EnumMapper_DeliveryType(int id, string expectedName)
        {
            //arrange
            var result = expectedName;

            //act
            var converter = EnumConverter.GetNameById<DeliveryType>(id);

            //assert
            Assert.Equal(converter, result);
        }

        [Theory(DisplayName = "EnumMapper_GetNameById_ComplaintState")]
        [InlineData((int)ComplaintState.Cancelled, "Anulowana")]
        [InlineData((int)ComplaintState.ComplaintCreate, "Założenie reklamacji")]
        [InlineData((int)ComplaintState.ComplaintSolved, "Rozwiązanie reklamacji")]
        [InlineData((int)ComplaintState.SellerDecision, "Decyzja sprzedawcy")]
        public void EnumMapper_ComplaintState(int id, string expectedName)
        {
            //arrange
            var result = expectedName;

            //act
            var converter = EnumConverter.GetNameById<ComplaintState>(id);

            //assert
            Assert.Equal(converter, result);
        }

        public void Dispose() { }
    }
}
