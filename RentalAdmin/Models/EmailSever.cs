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
    
    public partial class EmailSever
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailSever()
        {
            this.SendInEmails = new HashSet<SendInEmail>();
        }
    
        public long EmailSeverID { get; set; }
        public string EmailSeverPassword { get; set; }
        public string EmailSeverMail { get; set; }
        public string EmailSeverDomain { get; set; }
        public int EmailSeverSentTodey { get; set; }
        public int EmailSeverMaxSend { get; set; }
        public System.DateTime EmailSeverLastUpdate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SendInEmail> SendInEmails { get; set; }
    }
}
