using EnumsNET;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace OrderTrackingSystem.Logic.EnumMappers
{
    public enum PayType
    {
        [Display(Name = "Gotówka")]
        Cash = 0,
        [Display(Name = "Apple Pay")]
        ApplePay = 1,
        [Display(Name = "Karta")]
        Card = 2,
        [Display(Name = "BLIK")]
        BLIK = 3
    }

    public enum DeliveryType
    {
        [Display(Name = "Kurier DPD")]
        Courier = 0,
        [Display(Name = "Poczta")]
        Post = 1,
        [Display(Name = "Odbiór osobisty")]
        Takeself = 2,
        [Display(Name = "Paczkomat")]
        Paczkomat = 3
    }

    public enum MailDirectionType
    {
        [Description("Customer-Customer")]
        CustomerToCustomer = 1,
        [Description("Customer-Seller")]
        CustomerToSeller = 2,
        [Description("Seller-Customer")]
        SellerToCustomer = 3
    }

    public enum ComplaintState
    {
        [Display(Name = "Anulowana")]
        Cancelled = 0,
        [Display(Name = "Założenie reklamacji")]
        ComplaintCreate = 1,
        [Display(Name = "Decyzja sprzedawcy")]
        SellerDecision = 2,
        [Display(Name = "Rozwiązanie reklamacji")]
        ComplaintSolved = 3
    }

    public static class EnumConverter
    {
        /* Generyczna metoda do konwersji z Id na Description */
        public static string GetNameById<T>(int id) where T : struct, Enum
        {
            return ((T)(object)id).AsString(EnumFormat.DisplayName);
        }

        public static string GetNameByIdLocalized<T>(int id) where T : struct, Enum
        {
            return Properties.Resources.ResourceManager.GetString(((T)(object)id).AsString(EnumFormat.Name), CultureInfo.CurrentCulture);
        }
    }

}
