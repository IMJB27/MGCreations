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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class billing_address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public billing_address()
        {
            this.orders = new HashSet<order>();
        }

        public int Billing_Address_ID { get; set; }
        public int User_ID { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Address Line 1")]
        public string Billing_Address_Line1 { get; set; }

        [DisplayName("Address Line 2")]
        public string Billing_Address_Line2 { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("City")]
        public string Billing_Address_City { get; set; }

        [DisplayName("County")]
        public string Billing_Address_County { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Country")]
        public string Billing_Address_Country { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DisplayName("Postcode")]
        public string Billing_Address_Postcode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }
        public virtual user user { get; set; }
    }
}