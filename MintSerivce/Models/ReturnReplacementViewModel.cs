using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MintSerivce.Models
{
    public class ReturnReplacementViewModel
    {
        public string CustomerID { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public DateTime? DateOfReturnOrganised { get; set; }
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