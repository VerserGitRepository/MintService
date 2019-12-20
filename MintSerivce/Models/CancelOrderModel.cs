using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MintSerivce.Models
{
    public class CancelOrderModel
    {
        //  public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string OrderStatus { get; set; }
        public string ErrorMessage { get; set; }


    }
}