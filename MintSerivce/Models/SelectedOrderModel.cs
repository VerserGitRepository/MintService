using System.ComponentModel.DataAnnotations;

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
        [MaxLength(13, ErrorMessage = "SIM Can Be Maximum 13 Digits Length")]
        [MinLength(9, ErrorMessage = "SIM Must Be Minimum 9 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "SIM Must Be Numeric")]
        public string SIM { get; set; }
        [MaxLength(18, ErrorMessage = "ConsignmentNumber Can Be Maximum 18 Digits Length")]
        [MinLength(5, ErrorMessage = "ConsignmentNumber Must Be Minimum 5 Digits Length")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Consignment Number is mandatory.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "ConsignmentNumber Must Be Alpha Numeric - Special character Not Allowed")]
        public string ConsignmentNumber { get; set; }
        public string DispatchNo { get; set; }
        public string ResultMessage { get; set; }
        public string ShipLabelURL { get; set; }
        public string UserName { get; set; }
        public string OrderType { get; set; }
    }
}