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
    
    public partial class admin
    {
        
        public int Admin_Id { get; set; }
        [DisplayName("Username")]

        [Required(ErrorMessage = "This Field is Required")]
        public string Admin_Username { get; set; }

        [DisplayName("Password")]
        [Required (ErrorMessage = "This Field is Required")]
        [DataType(DataType.Password)]
        public string Admin_Password { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Admin_Name { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "This Field is Required")]
        public string Admin_Surname { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.EmailAddress)]
        public string Admin_Email { get; set; }

        [DisplayName("Contact No")]
        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.PhoneNumber)]
        public string Admin_ContactNo { get; set; }
    }
}
