using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class SIMViewModel
    {
        [Required]
        public string VerserOrderID { get; set; }

        [Required(ErrorMessage = "SSN Is Mandatory To Add Phone!")]
        [MaxLength(10, ErrorMessage = "SSN Can Be Maximum 10 Digits Length")]
        [MinLength(7, ErrorMessage = "SSN Must Be Minimum 7 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "SSN Must Be Numeric")]
        public string SSN { get; set; }    
        public string ResultMessage { get; set; }
        public string ShipLabelURL { get; set; }
    }
}