﻿using System;

namespace MintSerivce.Models
{
    public class DispatchedOrderDto
    {
        public int ID { get; set; }
        public string VerserOrderID { get; set; }
        public string TIABOrderID { get; set; }
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
        public string OrderDispatchno { get; set; }
        public string OrderShipmentStatus { get; set; }
        public Nullable<DateTime> OrderShippedDate { get; set; }
    }
}