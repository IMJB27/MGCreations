//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MGCreations.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class customer_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public customer_details()
        {
            this.orders = new HashSet<order>();
        }
    
        public int Customer_ID { get; set; }
        public string Customer_Username { get; set; }
        public string Customer_Password { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Surname { get; set; }
        public System.DateTime Customer_DOB { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_ContactNo { get; set; }
        public string Customer_Address { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
    }
}
