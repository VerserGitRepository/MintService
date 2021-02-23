using FluentValidation.Results;
using MintSerivce.Helper;
using MintSerivce.Models;
using MintSerivce.ValidationHelper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MintSerivce.Controllers
{
    public class ManageOrdersController : Controller
    {
      
        public async Task<ActionResult> Index()
        {
            if (Session["User"] != null)
            {
                string username = Session["User"].ToString();
                if (username == "VerserMintAdmin@verser.com.au")
                {
                    ViewModel mymodel = new ViewModel();
                    List<ListItemModel> ordersList = Helper.Helper.OrdersList();
                    List<ListItemModel> skuList = Helper.Helper.SKUList();
                    mymodel.AccessoriesStock = await AccessoriesStockService.AvailableAccessoriesStock();
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
            var _buffermodel = new UpdateSKUBufferModel();

            if (!string.IsNullOrEmpty(ListItemModel) && !string.IsNullOrEmpty(SKUBuffer))
            {
                _buffermodel.BufferCount = SKUBuffer;
                _buffermodel.SKU = ListItemModel;
                var _validate = new UpdateSKUBufferValidator();
                var returnvalidation = _validate.Validate(_buffermodel);
                if (returnvalidation.IsValid == true)
                {
                    TempData["ValidationErrors"] = Helper.Helper.UpdateSKUBufferValue(ListItemModel, SKUBuffer);
                }
                else
                {
                    TempData["ValidationErrors"] = returnvalidation.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            else
            {
                TempData["ValidationErrors"] = "SKU And Buffer Value Should Not Be Empty!";
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        public ActionResult CreateSKU()
        {
            var _skunew = new ViewModel();          
            return View(_skunew);
        }
        public ActionResult UpdateSKU()
        {
            var _skunew = new ViewModel();
            return View(_skunew);
        }
        public ActionResult RemoveSKU()
        {
            return View();
        }
        public ActionResult ChangeOrderSKU()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangeOrderSKU(ViewModel theModel)
        {
            if (theModel != null)
            {
                var _requestModel = new SimActivationModel { 
                    VerserOrderID= theModel.VerserOrderID,
                    SKU= theModel.SKU,                    
                };  
             var _a=   Helper.Helper.ChangeVerserOrderSKU(_requestModel);
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        public ActionResult ManageAccessories()
        {
            return View();
        }
       [HttpPost]
        public ActionResult CreateSKU(ViewModel _SKURequest)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                result = Helper.Helper.CreateNewSKU(_SKURequest.SKUViewModel);
            }
            if (result == true)
            {
                TempData["ValidationErrors"] = $"New {_SKURequest.SKUViewModel.SKU} New SKU Created Successfully !";
            }
            else
            {
                TempData["ValidationErrors"] ="Unable To Create New SKU Please validate your inputs ";
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        [HttpPost]
        public ActionResult UpdateSKU(ViewModel _SKURequest)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                result = Helper.Helper.UpdateSKU(_SKURequest.SKUViewModel);
            }
            if (result == true)
            {
                TempData["ValidationErrors"] = $"New {_SKURequest.SKUViewModel.SKU} SKU Updated Successfully !";
            }
            else
            {
                TempData["ValidationErrors"] = "Unable To Update SKU Please validate your Enteries ";
            }
            return RedirectToAction("Index", "ManageOrders");
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

            return View("Index", theModel);
        }
        [HttpPost]
        public ActionResult ManageAccessories(int AccessoryId, int AddAccessoriesCount, int RemoveAccessoriesCount)
        {
            var result = AccessoriesStockService.UpdateAccessoriesCount(AccessoryId, AddAccessoriesCount, RemoveAccessoriesCount).Result;
            return View("Index", "ManageOrders");
        }
    }
}