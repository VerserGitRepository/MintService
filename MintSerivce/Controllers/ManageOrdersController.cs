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
            UpdateOrderAddressValidatorDto obj = new UpdateOrderAddressValidatorDto();

            UpdateOrderAddressDto obj1 = new UpdateOrderAddressDto();
            //obj1.FirstName = theModel.FirstName;

            //obj1.FirstName = theModel.FirstName;
            //obj1.Surname = theModel.Surname;
            //obj1.ContactNumber = theModel.ContactNumber;
            //obj1.Salutation = theModel.Salutation;
            obj1.AddressLine1 = theModel.AddressLine1;
            obj1.Locality = theModel.Locality;
            obj1.Postcode = theModel.Postcode;
            obj1.VerserOrderID = theModel.VerserOrderID;
            obj1.State = theModel.State;
            ValidationResult result = obj.Validate(theModel);
            if (!result.IsValid)
            {
                foreach (ValidationFailure failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
            else
            {
                var ReturnFileName = Helper.Helper.UpdateOrderAddress(obj1);
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