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
    
    public partial class Metro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Metro()
        {
            this.PropertyMetroes = new HashSet<PropertyMetro>();
        }
    
        public int MetroID { get; set; }
        public string MetroName { get; set; }
        public int AreaID { get; set; }
        public int Line { get; set; }
        public Nullable<long> MapID { get; set; }
    
        public virtual Area Area { get; set; }
        public virtual Map Map { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PropertyMetro> PropertyMetroes { get; set; }
    }
}
