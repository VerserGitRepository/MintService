using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MintSerivce.Models
{
    public class ViewModel
    {
        public ViewModel()
        {
            AccessoriesStock = new List<AccessoriesStockCountDto>();
            SKUViewModel = new SKUModel();
            NewUserModel = new NewUserRegisterModel();           
        }
        public List<SelectListItem> ListItemModel { get; set; }
        public List<SelectListItem> OrdersListItemModel { get; set; }
        public string SKU { get; set; }
        public string VerserOrderID { get; set; }
        public string State { get; set; }
        public string AddressLine1 { get; set; }
        public string Locality { get; set; }
        public string Postcode { get; set; }
        public List<AccessoriesStockCountDto> AccessoriesStock { get; set; }
        public SKUModel SKUViewModel { get; set; }
        public NewUserRegisterModel NewUserModel { get; set; }
        public string UNLockUserAccount { get; set; }

    }
}