using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class AccessoriesStockCountDto
    {
        public int Id { get; set; }
        public string AccessoryType { get; set; }
        public int? AvailableStockCount { get; set; }
    }
}