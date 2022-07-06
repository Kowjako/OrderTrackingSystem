using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OrderTrackingSystem.Tests.ValidatorsTests
{
    class ValidatorTests : IDisposable, IClassFixture<ValidatorFixture>
    {
        private readonly ValidatorFixture ValidatorFixture;

        public ValidatorTests(ValidatorFixture fixture)
        {
            ValidatorFixture = fixture;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
