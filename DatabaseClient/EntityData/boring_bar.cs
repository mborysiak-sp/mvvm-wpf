//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseClient.EntityData
{
    using System;
    using System.Collections.Generic;
    
    public partial class boring_bar : IEntityWithId
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public boring_bar()
        {
            this.boring_bar_bearing = new HashSet<boring_bar_bearing>();
            this.document_boring_bar = new HashSet<document_boring_bar>();
        }
    
        public int id { get; set; }
        public string model { get; set; }
        public Nullable<int> ordinal_number { get; set; }
        public Nullable<System.DateTime> scrapping_date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<boring_bar_bearing> boring_bar_bearing { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<document_boring_bar> document_boring_bar { get; set; }
    }
}
