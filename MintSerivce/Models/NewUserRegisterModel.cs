using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace MintSerivce.Models
{
    public class NewUserRegisterModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=^.{8,18}$)(?=(?:.*?\d){2})(?=.*[a-z])(?=(?:.*?[A-Z]){1})(?=(?:.*?[!@#$%*()_+^&}{:;?.]){1})(?!.*\s)[0-9a-zA-Z!@#$%*()_+^&]*$", ErrorMessage = "Password Must Be Minumum Eight [8] Character in Total, One Special Character, Upper And Lower Case Character And Minimum One Number Required!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public bool IsUiAccessRequired { get; set; }
    }
}