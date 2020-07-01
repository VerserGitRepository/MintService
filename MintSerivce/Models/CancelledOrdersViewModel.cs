using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class CancelledOrdersViewModel
    {
        public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Salutation { get; set; }
        public string OrderStatus { get; set; }
        public string State { get; set; }
        public string AddressLine1 { get; set; }
        public string Locality { get; set; }
        public string Postcode { get; set; }
        public string SKU { get; set; }
        public string OrderType { get; set; }
    }
}