using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string IsUiEligible { get; set; }
        public bool? IsLogged { get; set; }
        public bool? IsAccountLocked { get; set; }
        public int AccessFailedCount { get; set; } = 0;
    }
}