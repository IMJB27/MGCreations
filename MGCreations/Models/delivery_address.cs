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
    
    public partial class delivery_address
    {
        public int Delivery_Address_ID { get; set; }
        public int User_ID { get; set; }
        public string Delivery_Address_Line1 { get; set; }
        public string Delivery_Address_Line2 { get; set; }
        public string Delivery_Address_City { get; set; }
        public string Delivery_Address_County { get; set; }
        public string Delivery_Address_Country { get; set; }
        public string Delivery_Address_Postcode { get; set; }
    
        public virtual user user { get; set; }
    }
}