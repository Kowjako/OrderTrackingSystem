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
    
    public partial class ComplaintStates
    {
        public int OrderId { get; set; }
        public byte State { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int Id { get; set; }
    
        public virtual Orders Orders { get; set; }
    }
}
