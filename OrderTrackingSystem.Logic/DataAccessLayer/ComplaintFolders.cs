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
    
    public partial class ComplaintFolders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ComplaintFolders()
        {
            this.ComplaintFolders1 = new HashSet<ComplaintFolders>();
            this.ComplaintRelations = new HashSet<ComplaintRelations>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentComplaintFolderId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintFolders> ComplaintFolders1 { get; set; }
        public virtual ComplaintFolders ComplaintFolders2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintRelations> ComplaintRelations { get; set; }
    }
}
