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
    
    public partial class photographer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public photographer()
        {
            this.comments = new HashSet<comment>();
            this.commentsHomes = new HashSet<commentsHome>();
            this.photo_sessions = new HashSet<photo_sessions>();
            this.photographer_cities = new HashSet<photographer_cities>();
            this.PhotographerPricings = new HashSet<PhotographerPricing>();
            this.PhotographerTypes = new HashSet<PhotographerType>();
            this.photos = new HashSet<photo>();
            this.Subscriptions = new HashSet<Subscription>();
            this.photosAdmins = new HashSet<photosAdmin>();
        }
    
        public int id { get; set; }
        public string user_id { get; set; }
        public string FullName { get; set; }
        public string subscription_type { get; set; }
        public string profilePhoto { get; set; }
        public string coverPhoto { get; set; }
        public string bio { get; set; }
        public bool accept { get; set; }
        public bool is_hidden { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<int> age { get; set; }
        public string instagram { get; set; }
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string linkedin { get; set; }
        public string PhoneNumber { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<comment> comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<commentsHome> commentsHomes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<photo_sessions> photo_sessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<photographer_cities> photographer_cities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhotographerPricing> PhotographerPricings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhotographerType> PhotographerTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<photo> photos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<photosAdmin> photosAdmins { get; set; }
    }
}
