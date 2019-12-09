using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class UpdateOrderAddressDto
    {
        public string VerserOrderID { get; set; }
        //public string FirstName { get; set; }
        //public string Surname { get; set; }
        //public string Salutation { get; set; }
        //public int? ContactNumber { get; set; }
        public string State { get; set; }
        public string AddressLine1 { get; set; }
        public string Locality { get; set; }
        public string Postcode { get; set; }
    }
}

