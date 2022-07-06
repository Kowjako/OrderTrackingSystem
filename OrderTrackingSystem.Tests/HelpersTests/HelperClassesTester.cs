using OrderTrackingSystem.Logic.HelperClasses;
using Xunit;
using System;
using System.Diagnostics;
using Xunit.Abstractions;
using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.Services;
using System.Linq;
using System.Collections.Generic;
using OrderTrackingSystem.Logic.DTO;

namespace OrderTrackingSystem.Tests.HelpersTests
{

    public class HelperClassesTester : IDisposable
    {
        private readonly ITestOutputHelper Printer;

        public HelperClassesTester(ITestOutputHelper p)
        {
            Printer = p;
        }

        #region Cryptography.cs

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

        #endregion

        #region EnumMapper.cs

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

        #endregion

        #region Extensions.cs

        [Fact(DisplayName = "Extensions_ToCredentials")]
        public void Extensions_ToCredentials()
        {
            //arrange
            string[] data = new string[] { "1", "2", "3" };
            string[] data2 = new string[] { "1" };
            string[] correctData = new string[] { "login", "password" };

            //act
            (string login, string password) = correctData.ToCredentials();

            //assert
            Assert.Throws<InvalidOperationException>(() => data.ToCredentials());
            Assert.Throws<InvalidOperationException>(() => data2.ToCredentials());
            Assert.Equal(login, correctData[0]);
            Assert.Equal(password, correctData[1]);
        }

        #endregion

        #region ParcesStateFSM.cs

        [Theory(DisplayName = "FSM_States")]
        [MemberData(nameof(GetStatesWithResult))]
        public void FSM_CheckStates(State state, IEnumerable<OrderState> expectedStates)
        {
            //arrange

            //act
            var fetchedStates = state.GetNextStates().Select(p => p.Item1);

            //assert
            Assert.Equal(fetchedStates, expectedStates);
        }

        #endregion

        #region RecursiveTreeFiller.cs

        [Theory(DisplayName = "RecursiveTreeFiller_GetAllStates")]
        [MemberData(nameof(GetICompositePattern))]
        public void RecursiveTreeFiller_GetAllStates()
        {

        }

        #endregion

        #region MemberData

        public static IEnumerable<object[]> GetStatesWithResult()
        {
            yield return new object[] { new StateA(), new[] { OrderState.GetFromSeller } };
            yield return new object[] { new StateB(), new[] { OrderState.GetByLocal } };
            yield return new object[] { new StateC(), new[] { OrderState.SentFromLocal } };
            yield return new object[] { new StateD(), new[] { OrderState.ToDelivery, OrderState.GetByLocal } };
            yield return new object[] { new StateE(), new[] { OrderState.ReadyToPickup } };
            yield return new object[] { new StateF(), new[] { OrderState.ComplaintSet } };
            yield return new object[] { new StateG(), new[] { OrderState.ComplaintResolved, OrderState.ReturnToSeller } };
            yield return new object[] { new StateH(), new[] { OrderState.ReturnToSeller } };
            yield return new object[] { new StateI(), Enumerable.Empty<OrderState>() };
            yield return new object[] { new StateJ(), Enumerable.Empty<OrderState>() };
        }

        public static IEnumerable<object[]> GetICompositePattern()
        {
            yield return new object[] { new CategoryDTO() {
                Name = "1",
                Children = new List<CategoryDTO>()
                {
                    new CategoryDTO() {Name = "1.1", Children = new List<CategoryDTO>()
                    {
                        new CategoryDTO() {Name = "1.1.1"},
                        new CategoryDTO() {Name = "1.1.2"},
                        new CategoryDTO() {Name = "1.1.3"}
                    } },
                    new CategoryDTO() {Name = "1.2"}
                }
            } };
        }

        #endregion

        public void Dispose() { }
    }
}
