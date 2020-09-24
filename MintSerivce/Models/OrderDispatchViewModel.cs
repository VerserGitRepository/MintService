using System;

namespace MintSerivce.Models
{
    public class OrderDispatchViewModel
    {
        public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
        public string VerserReturnOrderID { get; set; }
        public string SSN { get; set; }
        public int ReturnSSN { get; set; }
        public int ReplacementSSN { get; set; }
        public string ReturnedIMEI { get; set; }
        public string ReplacementIMEI { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Salutation { get; set; }
        public string State { get; set; }
        public string AddressLine1 { get; set; }
        public string Locality { get; set; }
        public string Postcode { get; set; }
        public string SKU { get; set; }
        public int? ContactNumber { get; set; }
        public string JMS_JobNo { get; set; }
        public int? JMS_JOBID { get; set; }
        public string OrderType { get; set; }
        public Nullable<DateTime> OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string Ordershipment_Id { get; set; }
        public string Ordershipment_Consignment_Id { get; set; }
        public string OrderDispatchno { get; set; }
        public string OrderShipmentStatus { get; set; }
        public string OrderShipmentURL { get; set; }
        public string CustomerID { get; set; }
        public DateTime? DateOfPurchase { get; set; }
        public DateTime? DateOfReturnOrganised { get; set; }
        public DateTime? DateOfExpectedReturn { get; set; } //Date of return organised +7 days     
        public string ReturnType { get; set; }
        public string CoolingOffPeriod { get; set; }
        public string TrackingNumberToCustomer { get; set; }
        public string TrackingNumberFromCustomerToVerser { get; set; }
        public int? NumberofdaysReturnedReceive { get; set; } //-	Number of days taken to receive returned device  ( The date it was received by Verser minus the date of returned organise)
        public string IsReplaced { get; set; }
        public string SMSReminder { get; set; }
        public string CustomerServiceComments { get; set; }
        public bool IsReplacementRequired { get; set; }
        public bool ReplacementTONewAddress { get; set; }
        public bool ReplacementToNewSKU { get; set; }
        public bool ReplacementTONewOrderType { get; set; }
        public Nullable<DateTime> OrderShippedDate { get; set; }

    }
}