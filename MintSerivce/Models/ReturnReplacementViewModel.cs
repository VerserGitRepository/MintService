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
        public ReturnReplacementViewModel()
        {
        }
        [Required(ErrorMessage = "VerserOrderID Order Is Mandatory")]
        public string VerserOrderID { get; set; }
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
        [Required(ErrorMessage = "ReturnType Is Mandatory")]
        public string ReturnType { get; set; }
        [Required(ErrorMessage = "CoolingOffPeriod Is Mandatory")]
        public string CoolingOffPeriod { get; set; } 
        public string TrackingNumberToCustomer { get; set; }
        public string TrackingNumberFromCustomerToVerser { get; set; }
        public int? NumberofdaysReturnedReceive   { get; set; } //-	Number of days taken to receive returned device  ( The date it was received by Verser minus the date of returned organise)
        public string IsReplaced { get; set; }
        [Required(ErrorMessage = "SMSReminder Is Mandatory")]
        public string SMSReminder { get; set; } 
        public string CustomerServiceComments { get; set; }
        public bool IsReplacementRequired { get; set; }
        public bool ReplacementTONewAddress { get; set; }
        public bool ReplacementToNewSKU { get; set; }
        public bool ReplacementTONewOrderType { get; set; }
        public string FirstName { get; set; }       
        public string Surname { get; set; }      
        public string Salutation { get; set; }      
        public string State { get; set; }      
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string TIABOrderID { get; set; }        
        public string Locality { get; set; }      
        public string Postcode { get; set; }     
        public string SKU { get; set; }      
        public string ContactNumber { get; set; }    
        public string OrderType { get; set; }
    }
}