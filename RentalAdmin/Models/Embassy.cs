//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentalAdmin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Embassy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Embassy()
        {
            this.ArticleRelations = new HashSet<ArticleRelation>();
            this.EmbassyProperties = new HashSet<EmbassyProperty>();
        }
    
        public int EmbassyID { get; set; }
        public string EmbassyName { get; set; }
        public string EmbassyOrginalName { get; set; }
        public string EmbassyAddress { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string EmbassyWebsite { get; set; }
        public System.DateTime EmbassyBirthDate { get; set; }
        public string EmbassyEmail { get; set; }
        public string EbmbassyPhone { get; set; }
        public string EmbassySlug { get; set; }
        public Nullable<long> MapID { get; set; }
        public string ImageSmall { get; set; }
        public string ImageBig { get; set; }
    
        public virtual Area Area { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticleRelation> ArticleRelations { get; set; }
        public virtual Map Map { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmbassyProperty> EmbassyProperties { get; set; }
    }
}
