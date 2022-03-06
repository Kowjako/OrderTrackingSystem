using EnumsNET;
using System.ComponentModel;

namespace OrderTrackingSystem.Logic.EnumMappers
{
    public enum PayType
    {
        [Description("Gotówka")]
        Cash = 0,
        [Description("Apple Pay")]
        ApplePay = 1,
        [Description("Karta")]
        Card = 2,
        [Description("BLIK")]
        BLIK = 3
    }

    public enum DeliveryType
    {
        [Description("Kurier DPD")]
        Courier = 0,
        [Description("Poczta")]
        Post = 1,
        [Description("Odbiór osobisty")]
        Takeself = 2,
        [Description("Paczkomat")]
        Paczkomat = 3
    }

    public static class PayTypeEnumConverter
    {
        public static string GetNameById(int id) => ((PayType)id).AsString(EnumFormat.Description);
    }

    public static class DeliveryTypeEnumConverter
    {
        public static string GetNameById(int id) => ((DeliveryType)id).AsString(EnumFormat.Description);
    }

}
