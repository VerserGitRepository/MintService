using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MintSerivce.Models
{
    public class SKUStock
    {
        public SKUStock()
        {
            Files = new List<FilesModel>();
        }
        public string SKU { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Capacity { get; set; }
        public string Colour { get; set; }
        public int? Count { get; set; }
        public string SKUBuffer { get; set; }
        public List<FilesModel>  Files { get; set; }
    }
}