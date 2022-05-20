﻿using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class OrderDTO
    {
        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Numer { get; set; }
        [Display(Name = "PayType", ResourceType = typeof(Properties.Resources))]
        public string Oplata { get; set; }
        [Display(Name = "DeliveryType", ResourceType = typeof(Properties.Resources))]
        public string Dostawa { get; set; }
        [Display(Name = "Complaint", ResourceType = typeof(Properties.Resources))]
        public string Rezygnacja { get; set; }

        [MoneyField(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Kwota { get; set; }
        [Display(Name = "Shop", ResourceType = typeof(Properties.Resources))]
        public string Sklep { get; set; }

        [Browsable(false)]
        public int CurrentOrderState { get; set; }
        [Browsable(false)]
        public int Id { get; set; }
        [Browsable(false)]
        public int PickupId { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int CustomerId { get; set; }

        [Browsable(false)]
        public PickupDTO PickupDTO { get; set; }
        [Browsable(false)]
        public BindingList<CartProductDTO> CartProducts { get; set; }
    }
}
