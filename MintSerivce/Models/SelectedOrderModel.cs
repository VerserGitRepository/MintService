using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class SelectedOrderModel
    {
        public bool PhoneAndSim { get; set; }
        [Required]
        public string VerserOrderID { get; set; }
        [Required(ErrorMessage = "SSN Is Mandatory To Add Phone!")]
        [MaxLength(10, ErrorMessage = "SSN Can Be Maximum 10 Digits Length")]
        [MinLength(7, ErrorMessage = "SSN Must Be Minimum 7 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "SSN Must Be Numeric")]
        public string SSN { get; set; }

        [RequiredIf("PhoneAndSim", true, "SIM Is Mandatory To Add Phone!")]
        [MaxLength(16, ErrorMessage = "SIM Can Be Maximum 10 Digits Length")]
        [MinLength(8, ErrorMessage = "SIM Must Be Minimum 7 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "SIM Must Be Numeric")]
        public string SIM { get; set; }

        public string DispatchNo { get; set; }
        public string ResultMessage { get; set; }
        public string ShipLabelURL { get; set; }
        public string UserName { get; set; }
        public string OrderType { get; set; }
    }
}