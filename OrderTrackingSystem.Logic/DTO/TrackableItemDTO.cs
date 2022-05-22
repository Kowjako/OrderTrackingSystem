using OrderTrackingSystem.Logic.DTO.Pagination;
using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class TrackableItemDTO : IPagedEntity
    { 
        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Number { get; set; }

        [ShortDateField]
        [Display(Name = "CreateDate", ResourceType = typeof(Properties.Resources))]
        public DateTime Date { get; set; }

        [Display(Name = "Buyer", ResourceType = typeof(Properties.Resources))]
        public string Customer { get; set; }

        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Seller { get; set; }

        [MoneyField(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Value { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [Browsable(false)]
        public bool IsOrder { get; set; }

        [Browsable(false)]
        public int SellerId { get; set; }

        [Browsable(false)]
        public int CustomerId { get; set; }

        #endregion

        #region IPagedEntity implementation

        [Browsable(false)]
        public int RowNumber { get; set; }

        #endregion
    }
}
