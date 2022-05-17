using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.HelperClasses.DTOAttributes
{
    /// <summary>
    /// Klasa bazowa dla wszystkich customowych atrybutów
    /// </summary>
    public abstract class UKAttribute : Attribute
    {
        public abstract string GetStringFormat();
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Money : UKAttribute
    {
        public int decimalPlaces { get; private set; }
        public Money(int decimalPlaces)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public override string GetStringFormat()
        {
            return $"C{decimalPlaces}";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Percentage : UKAttribute
    {
        public override string GetStringFormat()
        {
            return "P2";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Amount : UKAttribute
    {
        public int decimalPlaces { get; private set; }
        public Amount(int decimalPlaces)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public override string GetStringFormat()
        {
            return $"F{decimalPlaces}";
        }
    }
}
