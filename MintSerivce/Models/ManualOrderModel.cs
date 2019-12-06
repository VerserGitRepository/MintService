using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace MintSerivce.Models
{
    public class ManualOrderModel
    {
      //  public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }   
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Surname Is Mandatory")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Salutation Is Mandatory")]
        public string Salutation { get; set; }
        [Required(ErrorMessage = "State Is Mandatory!")]
        public string State { get; set; }
        [Required(ErrorMessage = "AddressLine1 Is Mandatory")]
        public string AddressLine1 { get; set; }
        [Required(ErrorMessage = "Suburb Is Mandatory")]
        public string Locality { get; set; }
        [Required(ErrorMessage = "Postcode Is Mandatory")]
        [MaxLength(4, ErrorMessage = "Postcode Can Be Maximum 4 Digits Length")]
        [MinLength(4, ErrorMessage = "Postcode Must Be Minimum 4 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postcode Must Be Numeric")]
        public string Postcode { get; set; }
        [Required(ErrorMessage = "SKU Is Mandatory")]
        public string SKU { get; set; }
        [Required(ErrorMessage = "ContactNumber Is Mandatory")]
        [MaxLength(10, ErrorMessage = "ContactNumber Can Be Maximum 10 Digits Length")]
        [MinLength(8, ErrorMessage = "ContactNumber Must Be Minimum 8 Digits Length")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "ContactNumber Must Be Numeric")]
        public string ContactNumber { get; set; }      
        [Required(ErrorMessage = "OrderType Is Mandatory")]
        public string OrderType { get; set; }
        //public Nullable<DateTime> OrderDate { get; set; }
        //public string OrderStatus { get; set; }
        //public string Ordershipment_Id { get; set; }
        //public string OrderDispatchno { get; set; }
        //public string OrderShipmentStatus { get; set; }
        //public string OrderShipmentURL { get; set; }
        //public Nullable<DateTime> OrderShippedDate { get; set; }
        //public string JMS_JobNo { get; set; }
        //public int? JMS_JOBID { get; set; }
    }
}