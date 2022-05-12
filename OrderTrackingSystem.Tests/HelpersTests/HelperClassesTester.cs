using OrderTrackingSystem.Logic.EnumMappers;
using OrderTrackingSystem.Logic.HelperClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace OrderTrackingSystem.Tests.HelpersTests
{ 

    public class HelperClassesTester
    {
        [Fact]
        public void Cryptography_Encrypt_Decrypt()
        {
            var password = "QWERYIOPASDGJKLCXZVB<NM!@#%#^&*)(12415425216713";
            var encrypted = Cryptography.EncryptWithRSA(password);

            var decrypted = Cryptography.DecryptFromRSA(encrypted);
            Assert.Equal(password, decrypted);
        }

    }
}
