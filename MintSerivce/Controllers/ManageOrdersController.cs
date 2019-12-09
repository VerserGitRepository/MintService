using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Results;
using MintSerivce.Helper;
using MintSerivce.Models;

namespace MintSerivce.Controllers
{
    public class ManageOrdersController : Controller
    {
        // GET: ReturnedOrders
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                string username = Session["User"].ToString();
                if (username == "VerserMintAdmin@verser.com.au")
                {
                    ViewModel mymodel = new ViewModel();
                    List<ListItemModel> ordersList = Helper.Helper.OrdersList();
                    List<ListItemModel> skuList = Helper.Helper.SKUList();
                    mymodel.ListItemModel = new List<SelectListItem>();
                    foreach (ListItemModel item in skuList)
                    {
                        mymodel.ListItemModel.Add(new SelectListItem { Text = item.Value });
                    }
                    mymodel.OrdersListItemModel = new List<SelectListItem>();
                    foreach (ListItemModel item in ordersList)
                    {
                        mymodel.OrdersListItemModel.Add(new SelectListItem { Text = item.Value });
                    }


                    return View(mymodel);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult OrderSummary()
        {
            var ReturnFileName = Helper.Helper.OrderSummary().Result;
            if (ReturnFileName != null)
            {
                ReturnFileName = ReturnFileName.Trim('"');
                TempData["OrderFilePath"] = ReturnFileName;
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        public ActionResult UpdateSKUBuffer(string ListItemModel, string SKUBuffer)
        {
            if (!string.IsNullOrEmpty(ListItemModel) && !string.IsNullOrEmpty(SKUBuffer))
            {
                string returnMessage = Helper.Helper.UpdateSKUBufferValue(ListItemModel, SKUBuffer);
                TempData["ValidationErrors"] = returnMessage;
            }
            else
            {
                TempData["ValidationErrors"] = "Please Supply SKU Details To Update!";
            }

            return RedirectToAction("Index", "ManageOrders");
        }
        public ActionResult CreateSKU()
        {
            return View();
        }
        public ActionResult RemoveSKU()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateSKU(string SKU, string Make, string Model, string Capacity, string Colour)
        {
            SKUModel skus = new SKUModel() {
            SKU=SKU,
            Make=Make,
            Model=Model,
            Capacity=Capacity,
            Colour=Colour};
            var result= CreateNewSKU.AddNewSKU(skus);
            return View("Index", "ManageOrders");
        }
        [HttpPost]
        public ActionResult RemoveSKU(SKUModel skumodel)
        {
            var result = RetireSKU.RetireExistingSKU(skumodel).Result;       
                 
            return View("Index", "ManageOrders");
        }
        [HttpPost]
        public ActionResult UpdateOrderAddress(ViewModel theModel)
        {
            var _Validator = new UpdateOrderAddressValidatorDto();


            var _UpdateOrderAddressData = new UpdateOrderAddressDto();
    
            _UpdateOrderAddressData.AddressLine1 = theModel.AddressLine1;            
            _UpdateOrderAddressData.Locality = theModel.Locality;
            _UpdateOrderAddressData.Postcode = theModel.Postcode;           
            _UpdateOrderAddressData.VerserOrderID = theModel.VerserOrderID;
            _UpdateOrderAddressData.State = theModel.State;
            ValidationResult result = _Validator.Validate(theModel);
  if (!result.IsValid)
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
            else
            {
                var ReturnFileName = Helper.Helper.UpdateOrderAddress(_UpdateOrderAddressData);
                if (ReturnFileName != null)
                {
                    ReturnFileName = ReturnFileName.Trim('"');
                    TempData["ValidationErrors"] = ReturnFileName;
                }
            }
            List<ListItemModel> ordersList = Helper.Helper.OrdersList();
            List<ListItemModel> skuList = Helper.Helper.SKUList();
            theModel.ListItemModel = new List<SelectListItem>();
            foreach (ListItemModel item in skuList)
            {
                theModel.ListItemModel.Add(new SelectListItem { Text = item.Value });
            }
            theModel.OrdersListItemModel = new List<SelectListItem>();
            foreach (ListItemModel item in ordersList)
            {
                theModel.OrdersListItemModel.Add(new SelectListItem { Text = item.Value });
            }

            return View("Index",theModel);
        }
    }
}