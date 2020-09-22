using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MintSerivce.Models
{
    public class ReturnReplacementViewModel
    {
        public string CustomerID { get; set; }
        [Required(ErrorMessage = "Day Is Mandatory")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfPurchase { get; set; }
        [Required(ErrorMessage = "Day Is Mandatory")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfReturnOrganised { get; set; }
        [Required(ErrorMessage = "Day Is Mandatory")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfExpectedReturn { get; set; } //Date of return organised +7 days
        public string ReturnType { get; set; } //DropdownList
        public string CoolingOffPeriod { get; set; } //Yes, No
        public string TrackingNumberToCustomer { get; set; }
        public string TrackingNumberFromCustomerToVerser { get; set; }
        public int? NumberofdaysReturnedReceive   { get; set; } //-	Number of days taken to receive returned device  ( The date it was received by Verser minus the date of returned organise)
        public string IsReplaced { get; set; }
        public string SMSReminder { get; set; } //Yes/No
        public string CustomerServiceComments { get; set; }
        //public SelectListItem CoolingOffPeriodlist { get; set; }
        //public SelectListItem ReturnTypes { get; set; }
        //public SelectListItem IsReplacedlist { get; set; }
        //public SelectListItem SMSReminderList { get; set; }
    }
}