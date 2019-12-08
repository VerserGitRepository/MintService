using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class ReturnDto
    {
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string OrderStatus { get; set; }
        public string ErrorMessage { get; set; }
    }
}