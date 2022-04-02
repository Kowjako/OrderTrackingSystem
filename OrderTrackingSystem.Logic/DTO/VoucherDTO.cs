﻿using System;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class VoucherDTO
    { 
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Value { get; set; }
        public decimal RemainValue { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ExpireDateString => ExpireDate.ToShortDateString();
        public string ValueString => Value + "/" + RemainValue + " zł";
    }
}