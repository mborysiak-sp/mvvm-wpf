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
    
    public partial class bearing : IEntityWithId
    {
        public bearing()
        {
            this.boring_bar_bearing = new HashSet<boring_bar_bearing>();
        }
    
        public int id { get; set; }
        public string model { get; set; }
    
        public virtual ICollection<boring_bar_bearing> boring_bar_bearing { get; set; }
    }
}
