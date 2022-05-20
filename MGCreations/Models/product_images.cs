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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class product_images
    {
        public int Product_Images_ID { get; set; }

        [Required(ErrorMessage = "This Field is Important")]
        [DisplayName("Product")]
        public int Product_ID { get; set; }

        [DisplayName("Select Pictures")]
        [DataType(DataType.ImageUrl)]
        public string Product_Image_URL { get; set; }


        [DisplayName("Image Name")]
        public string Product_Image_Name { get; set; }


        [DisplayName("Personalisable")]
        public Nullable<sbyte> isPersonalisable { get; set; }

        [NotMapped]
        public bool isPersonalisableBool
        {
            get { return isPersonalisable > 0; }
            set { isPersonalisable = (sbyte)(value ? 1 : 0); }
        }

        public virtual product product { get; set; }
    }
}