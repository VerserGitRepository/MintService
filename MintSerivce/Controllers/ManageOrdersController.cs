using FluentValidation.Results;
using MintSerivce.Helper;
using MintSerivce.Models;
using MintSerivce.ValidationHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;

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
                var _requestModel = new SimActivationModel
                {
                    VerserOrderID = theModel.VerserOrderID,
                    SKU = theModel.SKU,
                };
                var _a = Helper.Helper.ChangeVerserOrderSKU(_requestModel);
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
                TempData["ValidationErrors"] = "Unable To Create New SKU Please validate your inputs ";
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

        [HttpPost]
        public ActionResult ImportAssets()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["file"];
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/ExcelFile"), _FileName);
                    file.SaveAs(_path);
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(_path, false))
                    {
                        WorkbookPart wbPart = doc.WorkbookPart;
                        Sheet mysheet = (Sheet)doc.WorkbookPart.Workbook.Sheets.ChildElements.GetItem(0);
                        Worksheet Worksheet = ((WorksheetPart)wbPart.GetPartById(mysheet.Id)).Worksheet;

                        //Note: worksheet has 8 children and the first child[1] = sheetviewdimension,....child[4]=sheetdata  
                        int wkschildno = 4;
                        SheetData Rows = (SheetData)Worksheet.ChildElements.GetItem(wkschildno);
                        var assetList = new List<ImportAssetViewModel>();
                        for (int row = 1; row < Rows.Count(); row++)
                        {
                            Row currentrow = (Row)Rows.ChildElements.GetItem(row);
                            if (!string.IsNullOrEmpty(GetCellValue((Cell)currentrow.ChildElements.GetItem(0), wbPart)))
                            {
                                var asset = new ImportAssetViewModel();
                                asset.AlchemyID = GetCellValue((Cell)currentrow.ChildElements.GetItem(0), wbPart);
                                asset.SSN = GetCellValue((Cell)currentrow.ChildElements.GetItem(1), wbPart);
                                asset.Make = GetCellValue((Cell)currentrow.ChildElements.GetItem(2), wbPart);
                                asset.Model = GetCellValue((Cell)currentrow.ChildElements.GetItem(3), wbPart);
                                asset.Serialno = GetCellValue((Cell)currentrow.ChildElements.GetItem(4), wbPart);
                                asset.Capacity = GetCellValue((Cell)currentrow.ChildElements.GetItem(5), wbPart);
                                asset.Colour = GetCellValue((Cell)currentrow.ChildElements.GetItem(6), wbPart);
                                asset.Grade = GetCellValue((Cell)currentrow.ChildElements.GetItem(7), wbPart);
                                asset.SubGrade = GetCellValue((Cell)currentrow.ChildElements.GetItem(8), wbPart);
                                asset.DatePurchased = GetCellValue((Cell)currentrow.ChildElements.GetItem(9), wbPart);
                                asset.BatteryTest = GetCellValue((Cell)currentrow.ChildElements.GetItem(10), wbPart);
                                asset.IMEI = GetCellValue((Cell)currentrow.ChildElements.GetItem(11), wbPart);
                                assetList.Add(asset);
                            }
                            else
                            {
                                break;
                            }
                        }
                        bool _IsSuccess = ImportAssetService.ImportAssets(assetList);

                        if (_IsSuccess)
                        {
                            TempData["ManualOrder"] = $"Asset Import Processed Successfully!";
                         }
                        else
                        {
                            TempData["OrderError"] = $"Asset Import Processed Failed!";
                        }
                    }
                    //Delete the file after reading
                    // System.IO.File.Delete(_path);
                }
                else
                {
                    System.Web.HttpContext.Current.Session["ErrorMessage"] = "File Not choosen Please a file";
                }
            }
            catch (System.Exception Ex)
            {
                System.Web.HttpContext.Current.Session["ErrorMessage"] = Ex.Message;
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        
       public ActionResult CreateNewUser()
        {
            var newUserAddRequest = new NewUserRegisterModel(); 
            return View(newUserAddRequest);
        }
        [HttpPost]
        public ActionResult CreateNewUser(ViewModel newUserAddRequest)
        {
            if (ModelState.IsValid)
            {
                var result= RegisterNewUserService.RegisterNewUser(newUserAddRequest.NewUserModel);
                if (result == true)
                {
                    TempData["ValidationErrors"] = "New User Successfully Registered !";
                }
                else
                {
                    TempData["ValidationErrors"] = "Request Fail To Register New User Please Check Existing User List!";
                }                
            }
            return RedirectToAction("Index", "ManageOrders");
        }


        public ActionResult UpdateUserPassword()
        {
            var newUserAddRequest = new NewUserRegisterModel();
            return View(newUserAddRequest);
        }
        [HttpPost]
        public ActionResult UpdateUserPassword(ViewModel newUserAddRequest)
        {
           if (ModelState.IsValid)
            {
                var result = RegisterNewUserService.UpdateUserPassword(newUserAddRequest.NewUserModel);
                if (result == true)
                {
                    TempData["ValidationErrors"] = "User Password Updated Successfully!";
                }
                else
                {
                    TempData["ValidationErrors"] = "Request Fail To Update User Password!";
                }
            }
            return RedirectToAction("Index", "ManageOrders");
        }


        public ActionResult UnLockUserAccount(ViewModel UserName)
        {
            var result = RegisterNewUserService.UnLockUserAccount(UserName.UNLockUserAccount);
            if (result == true)
            {
                TempData["ValidationErrors"] = "User Account UnLocked Successfully!";
            }
            else
            {
                TempData["ValidationErrors"] = "Request Fail To Unlock User Account!";
            }
            return RedirectToAction("Index", "ManageOrders");
        }
        private static string GetCellValue(Cell theCell, WorkbookPart wbPart)
        {
            string value = "";
            if (theCell != null)
            {
                value = theCell.InnerText;

                // If the cell represents an integer number, you are done. 
                // For dates, this code returns the serialized value that 
                // represents the date. The code handles strings and 
                // Booleans individually. For shared strings, the code 
                // looks up the corresponding value in the shared string 
                // table. For Booleans, the code converts the value into 
                // the words TRUE or FALSE.
                if (theCell.DataType != null)
                {
                    switch (theCell.DataType.Value)
                    {
                        case CellValues.SharedString:

                            // For shared strings, look up the value in the
                            // shared strings table.
                            var stringTable =
                                wbPart.GetPartsOfType<SharedStringTablePart>()
                                .FirstOrDefault();

                            // If the shared string table is missing, something 
                            // is wrong. Return the index that is in
                            // the cell. Otherwise, look up the correct text in 
                            // the table.
                            if (stringTable != null)
                            {
                                value =
                                    stringTable.SharedStringTable
                                    .ElementAt(int.Parse(value)).InnerText;
                            }
                            break;

                        case CellValues.Boolean:
                            switch (value)
                            {
                                case "0":
                                    value = "FALSE";
                                    break;
                                default:
                                    value = "TRUE";
                                    break;
                            }
                            break;
                    }
                }
            }
            return value;
        }
    }
}