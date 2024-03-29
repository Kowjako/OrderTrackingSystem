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
    
    public partial class Sells
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sells()
        {
            this.SellCarts = new HashSet<SellCarts>();
        }
    
        public int Id { get; set; }
        public string Number { get; set; }
        public System.DateTime SellingDate { get; set; }
        public int CustomerId { get; set; }
        public int SellerId { get; set; }
        public Nullable<int> PickupDays { get; set; }
    
        public virtual Customers Customers { get; set; }
        public virtual Customers Customers1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellCarts> SellCarts { get; set; }
    }
}
