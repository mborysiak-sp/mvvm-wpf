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
    
    public partial class document_spindle
    {
        public int id { get; set; }
        public Nullable<System.DateTime> issue_date { get; set; }
        public int day_count { get; set; }
        public string comment { get; set; }
        public Nullable<int> id_spindle { get; set; }
    
        public virtual spindle spindle { get; set; }
    }
}
