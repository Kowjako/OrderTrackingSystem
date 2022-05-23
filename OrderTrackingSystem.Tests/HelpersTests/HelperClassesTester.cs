using OrderTrackingSystem.Logic.HelperClasses;
using OrderTrackingSystem.Logic.Services;
using OrderTrackingSystem.Logic.Services.Interfaces;
using Xunit;
using Moq;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;
using System;

namespace OrderTrackingSystem.Tests.HelpersTests
{

    public class HelperClassesTester : IClassFixture<HelperClassesTester>, IDisposable
    {
        //SetUp
        public HelperClassesTester()
        {
                
        }

        [Fact(DisplayName = "HelperClassesTester.GetCustomerByMail")]
        public void Cryptography_Encrypt_Decrypt()
        {
            var password = "QWERYIOPASDGJKLCXZVB<NM!@#%#^&*)(12415425216713";
            var encrypted = Cryptography.EncryptWithRSA(password);

            var decrypted = Cryptography.DecryptFromRSA(encrypted);
            Assert.Equal(password, decrypted);

            /* Mock test
            var mock = new Mock<ICustomerService>();
            mock.Setup(cs => cs.GetCustomerByMail(It.IsAny<string>())).ReturnsAsync(new CustomerDTO());
            */

        }

        //TearDown
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
