using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.EnumMappers
{
    public enum PayType
    {
        Gotowka = 0,
        ApplePay = 1,
        Karta = 2,
        BLIK = 3
    }

    public static class PayTypeEnumConverter
    {
        public static string GetNameById(int id)
        {
            switch(id)
            {
                case 0:
                    return "Gotowka";
                case 1:
                    return "ApplePay";
                case 2:
                    return "Karta";
                case 3:
                    return "BLIK";
                default:
                    return string.Empty;
            }
        }
    }

}
