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
        public virtual string PropertyName { get; set; } = string.Empty;
    }

    public abstract class UKFormatAttribute : UKAttribute
    {
        public abstract string GetStringFormat();
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MoneyField : UKFormatAttribute
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
    public class PercentageField : UKFormatAttribute
    {
        public override string GetStringFormat()
        {
            return "{0} %";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AmountField : UKFormatAttribute
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
    public class ShortDateField : UKFormatAttribute
    {
        public override string GetStringFormat()
        {
            return $"dd-MM-yyyy";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LongDateField : UKFormatAttribute
    {
        public override string GetStringFormat()
        {
            return $"dd-MM-yyyy hh:mm:ss";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ImageField : UKAttribute
    {
        public override string PropertyName { get; set; }
    }

}
