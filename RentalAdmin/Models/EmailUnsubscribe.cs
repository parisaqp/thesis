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
    
    public partial class EmailUnsubscribe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailUnsubscribe()
        {
            this.EmailBlocks = new HashSet<EmailBlock>();
        }
    
        public long EmailUnsubscribeID { get; set; }
        public System.DateTime EmailUnsubscribeDate { get; set; }
        public string UserID { get; set; }
        public System.Guid SentEmailID { get; set; }
        public string Comment { get; set; }
        public string EmailFrom { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailBlock> EmailBlocks { get; set; }
    }
}
