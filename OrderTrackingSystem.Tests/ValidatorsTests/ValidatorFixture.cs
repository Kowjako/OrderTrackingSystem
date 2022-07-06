using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace OrderTrackingSystem.Tests.ValidatorsTests
{
    public class ValidatorFixture : IDisposable
    {
        private readonly ITestOutputHelper Printer;

        public ValidatorFixture(ITestOutputHelper printer)
        {
            Printer = printer;
            Printer.WriteLine("Poczatek testow - walidatory");
        }
        public void Dispose()
        {
            Printer.WriteLine("Koniec testow - walidatory");
        }
    }
}
