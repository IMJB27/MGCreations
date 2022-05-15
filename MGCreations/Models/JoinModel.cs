using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGCreations.Models
{
    public class JoinModel
    {
        public order Order { get; set; }
        public user User { get; set; }
        public delivery_address delivery_Address { get; set; }
        public billing_address billing_Address { get; set; }
        public product Product { get; set; }

        public cart Cart{ get; set; }

    }
}