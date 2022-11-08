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
    
    public partial class PropertyType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PropertyType()
        {
            this.ArticleRelations = new HashSet<ArticleRelation>();
            this.Properties = new HashSet<Property>();
        }
    
        public int PropertyTypeID { get; set; }
        public string PropertyTypeName { get; set; }
        public int PropertyTypeHomeNumber { get; set; }
        public string PropertyTypeSlug { get; set; }
        public string PropertyTypeDescription { get; set; }
        public Nullable<int> SpecialArea1 { get; set; }
        public Nullable<int> SpecialArea2 { get; set; }
        public string PropertyTypeTitle { get; set; }
        public bool PropertyIsApartment { get; set; }
        public bool PropertyIsHouse { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticleRelation> ArticleRelations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Property> Properties { get; set; }
    }
}