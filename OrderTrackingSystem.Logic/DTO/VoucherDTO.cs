using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class VoucherDTO
    { 
        public int Id { get; set; }

        public string Number { get; set; }

        public decimal Value { get; set; }

        public decimal RemainValue { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
