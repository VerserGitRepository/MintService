using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class ImportAssetViewModel
    {
        public string AlchemyID { get; set; }
        public int ReportID { get; set; }
        public string SSN { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Serialno { get; set; }
        public string Capacity { get; set; }
        public string Colour { get; set; }
        public string Grade { get; set; }
        public string SubGrade { get; set; }
        public string DatePurchased { get; set; }
        public string BatteryTest { get; set; }
        public string IMEI { get; set; }
    }
}