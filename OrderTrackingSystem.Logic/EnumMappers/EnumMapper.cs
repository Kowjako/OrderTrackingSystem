using EnumsNET;
using System;
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

    public enum ProductType
    {
        [Description("Tabletki")]
        Pill = 0,
        [Description("Krem")]
        Cream = 1,
        [Description("Syrop")]
        Syrup = 2,
        [Description("Aerozol")]
        Spray = 3
    }

    public enum SubProductType
    {
        /* Aerozole */
        [Description("Na gardło")]
        L1 = 1,
        [Description("Na katar")]
        L2 = 2,
        /* Tabletki */
        [Description("Na sen")]
        R1 = 3,
        [Description("Przeciwbólowe")]
        R2 = 4,
        [Description("Na nadciśnienie")]
        R3 = 5,
        /* Kremy */
        [Description("Na skórę")]
        K1 = 6,
        [Description("Na włosy")]
        K2 = 7,
        /* Syropy */
        [Description("Na kaszel")]
        S1 = 8,
        [Description("Na podrażnienie")]
        S2 = 9,
        [Description("Suchość")]
        S3 = 10
    }

    public static class EnumConverter
    {
        /* Generyczna metoda do konwersji z Id na Description */
        public static string GetNameById<T>(int id) where T : struct, Enum
        {
            return ((T)(object)id).AsString(EnumFormat.Description);
        }
    }

}
