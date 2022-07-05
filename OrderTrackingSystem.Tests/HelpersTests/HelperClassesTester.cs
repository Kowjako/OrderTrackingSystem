using OrderTrackingSystem.Logic.HelperClasses;
using Xunit;
using System;
using System.Diagnostics;

namespace OrderTrackingSystem.Tests.HelpersTests
{

    public class HelperClassesTester : IDisposable
    {
        //SetUp
        public HelperClassesTester()
        {
            Debug.WriteLine("Testy HelperClasses rozpoczete");
        }

        [Fact]
        public void Cryptography_Encrypt_Decrypt()
        {
            var password = "QWERYIOPASDGJKLCXZVB<NM!@#%#^&*)(12415425216713";
            var encrypted = Cryptography.EncryptWithRSA(password);

            var decrypted = Cryptography.DecryptFromRSA(encrypted);
            Assert.Equal(password, decrypted);
        }

        //TearDown
        public void Dispose()
        {
            Debug.WriteLine("Testy HelperClasses ukonczone");
        }
    }
}
