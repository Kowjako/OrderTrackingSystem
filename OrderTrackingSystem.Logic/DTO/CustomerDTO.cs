﻿using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CustomerDTO
    {
        public string Nazwa { get; set; }
        public string Email { get; set; }
        public string Numer { get; set; }
        public string Adres { get; set; }
        public string MiastoKod { get; set; }
    }
}