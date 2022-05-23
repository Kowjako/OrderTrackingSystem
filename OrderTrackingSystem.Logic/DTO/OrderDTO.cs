using OrderTrackingSystem.Logic.DTO.Pagination;
using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    #pragma warning disable CS1591
    public sealed class OrderDTO : IPagedEntity
    {
        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Number { get; set; }

        [Display(Name = "PayType", ResourceType = typeof(Properties.Resources))]
        public string PayType { get; set; }

        [Display(Name = "DeliveryType", ResourceType = typeof(Properties.Resources))]
        public string DeliveryType { get; set; }

        [Display(Name = "Complaint", ResourceType = typeof(Properties.Resources))]
        public bool IsComplaint { get; set; }

        [MoneyField(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Value { get; set; }

        [Display(Name = "Shop", ResourceType = typeof(Properties.Resources))]
        public string Seller { get; set; }

        #region Non-browsable

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

        #endregion

        #region IPagedEntity implementation

        [Browsable(false)]
        public int RowNumber { get; set; }

        #endregion
    }
}
