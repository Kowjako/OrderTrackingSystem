using OrderTrackingSystem.Logic.DTO.Pagination;
using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class SellDTO : IPagedEntity
    {
        [Display(Name= "Number", ResourceType = typeof(Properties.Resources))]
        public string Number { get; set; }

        [ShortDateField]
        [Display(Name = "SendDate", ResourceType = typeof(Properties.Resources))]
        public DateTime Date { get; set; }

        [Display(Name = "DaysToGet", ResourceType = typeof(Properties.Resources))]
        public int PickupDays { get; set; }

        [Display(Name = "Receiver", ResourceType = typeof(Properties.Resources))]
        public string Receiver { get; set; }

        [MoneyField(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Value { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int CustomerId { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }

        #endregion

        #region IPagedEntity implementation

        [Browsable(false)]
        public int RowNumber { get; set; }

        #endregion
    }
}
