//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderTrackingSystem.Logic.DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.ComplaintStates = new HashSet<ComplaintStates>();
            this.OrderCarts = new HashSet<OrderCarts>();
            this.OrderStates = new HashSet<OrderStates>();
            this.Mails = new HashSet<Mails>();
        }
    
        public int Id { get; set; }
        public string Number { get; set; }
        public int CustomerId { get; set; }
        public byte PayType { get; set; }
        public byte DeliveryType { get; set; }
        public int PickupId { get; set; }
        public int SellerId { get; set; }
        public Nullable<int> ComplaintDefinitionId { get; set; }
    
        public virtual ComplaintDefinitions ComplaintDefinitions { get; set; }
        public virtual Customers Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintStates> ComplaintStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderCarts> OrderCarts { get; set; }
        public virtual Pickups Pickups { get; set; }
        public virtual Sellers Sellers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderStates> OrderStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mails> Mails { get; set; }
    }
}
