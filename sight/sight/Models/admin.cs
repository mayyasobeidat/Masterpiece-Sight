//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sight.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class admin
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public string profilePhoto { get; set; }
        public string user_id { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
