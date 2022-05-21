using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class MoneyField : UKAttribute
    {
        public int decimalPlaces { get; private set; }
        public MoneyField(int decimalPlaces)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public override string GetStringFormat()
        {
            return "{0:C}";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PercentageField : UKAttribute
    {
        public override string GetStringFormat()
        {
            return "{0} %";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AmountField : UKAttribute
    {
        public int decimalPlaces { get; private set; }
        public AmountField(int decimalPlaces)
        {
            this.decimalPlaces = decimalPlaces;
        }

        public override string GetStringFormat()
        {
            return $"F{decimalPlaces}";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ShortDateField : UKAttribute
    {
        public override string GetStringFormat()
        {
            return $"dd-MM-yyyy";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LongDateField : UKAttribute
    {
        public override string GetStringFormat()
        {
            return $"dd-MM-yyyy hh:mm:ss";
        }
    }

}
