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
    
    public partial class photosAdmin
    {
        public int id { get; set; }
        public string photographerProfile { get; set; }
        public string photo { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public int photographyType { get; set; }
        public string title { get; set; }
        public string photographerName { get; set; }
        public int photographerID { get; set; }
    
        public virtual PhotographyType PhotographyType1 { get; set; }
        public virtual photographer photographer { get; set; }
    }
}
