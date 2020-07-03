using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class SIMOnlyViewModel
    {
        [MaxLength(18, ErrorMessage = "ConsignmentNumber Can Be Maximum 18 Digits Length")]
        [MinLength(5, ErrorMessage = "ConsignmentNumber Must Be Minimum 5 Digits Length")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Consignment Number is mandatory.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "ConsignmentNumber Must Be Alpha Numeric - Special character Not Allowed")]
        public string ConsignmentNumber { get; set; } 

        [RequiredIf("PhoneAndSim", true, "SIM Is Mandatory To Add Phone!")]
        [MaxLength(13, ErrorMessage = "SIM Can Be Maximum 13 Digits Length")]
        [MinLength(9, ErrorMessage = "SIM Must Be Minimum 9 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "SIM Must Be Numeric")]
        public string SIM { get; set; }

        public string ResultMessage { get; set; }
    }
}