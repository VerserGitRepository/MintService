﻿using MintSerivce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MintSerivce.ServiceAgents;
using Newtonsoft.Json;
using FluentValidation.Results;

namespace MintSerivce.Controllers
{
    public class ManualOrderController : Controller
    {   // GET: ManualOrder
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                string username = Session["User"].ToString();
                if (username != null) //"VerserMintAdmin@verser.com.au")
                {
                    ManualOrderModel model = new ManualOrderModel();
                    List<ListItemModel> ordersList = Helper.Helper.CancelOrdersList();
                    List<ListItemModel> DispatchedOrdersList = Helper.Helper.DispatchedOrderNumbers();

                    model.OrdersList = OrderService.ShopifyOnHoldOrders().ToList();

                    model.OrdersListItemModel = new List<SelectListItem>();
                    model.DispatchedOrderListItems = new List<SelectListItem>();
                    model.SKUList = new SelectList(Helper.Helper.SKUList(), "Value", "Value");

                    //model.ReturnReplacementModel.CoolingOffPeriodlist = new SelectList(Helper.DropDownHelper.CoolingoffPeriods());
                    //model.ReturnReplacementModel.ReturnTypes = new SelectList(Helper.DropDownHelper.ReturnTypes());
                    //model.ReturnReplacementModel.SMSReminder = new SelectList(Helper.DropDownHelper.SMSReminder());
                    //model.ReturnReplacementModel.CoolingOffPeriodlist = new SelectList(Helper.DropDownHelper.CoolingoffPeriods());

                    foreach (ListItemModel item in ordersList)
                    {
                        model.OrdersListItemModel.Add(new SelectListItem { Text = item.Value });
                    }

                    foreach (ListItemModel item in DispatchedOrdersList)
                    {
                        model.DispatchedOrderListItems.Add(new SelectListItem { Text = item.Value });
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Index(ManualOrderModel manualOrder)
        {
            if (ModelState.IsValid)
            {
                string returnmessage = Helper.Helper.CreateOrder(manualOrder);

                TempData["ManualOrder"] = returnmessage.ToString();
                ModelState.Clear();
                return RedirectToAction("index", "ManualOrder");
            }
            //if (TempData["TabOrder"] == null)
            //{
            TempData["TabOrder"] = "MORDER";
            //}
            return RedirectToAction("index", "ManualOrder");
        }
        [HttpPost]
        public ActionResult ReturnedOrder(ManualOrderModel manualOrder)
        {

            string json = JsonConvert.SerializeObject(manualOrder.ReturnReplacementModel);

            if (manualOrder.ReturnReplacementModel != null)
                {
                 var returnmessage = Helper.Helper.CreateReturnOrder(manualOrder.ReturnReplacementModel);
                    if (returnmessage != null && returnmessage.ErrorMessage !=null && returnmessage.OrderStatus == "ERROR")
                    {
                        TempData["OrderError"] = $"{returnmessage.VerserOrderID} {returnmessage.ErrorMessage}";
                    }
                    else{
                        TempData["ManualOrder"]= $" Order {returnmessage.VerserOrderID} Return Order {returnmessage.VerserReturnOrderID} is been created {returnmessage.OrderStatus} and ready to Process. Go To Orders for Processing";
                    }
              
                    ModelState.Clear();
                    TempData["TabOrder"] = "RORDER";
                    return RedirectToAction("index", "ManualOrder");
                }
                else
                {
                    TempData["ManualOrder"] = "Verser Order Is Mandatory";
                    TempData["TabOrder"] = "RORDER";
                    return RedirectToAction("index", "ManualOrder");
                }
               
            
            //TempData["TabOrder"] = "MORDER";

            //return RedirectToAction("index", "ManualOrder");
        }
        [HttpPost]
        public ActionResult CancelOrder(ManualOrderModel manualorder)
        {
            try
            {
                CancelOrderModel model = new CancelOrderModel { ErrorMessage = string.Empty, OrderStatus = string.Empty, TIABOrderID = manualorder.TIABOrderID, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.CancelOrder(model);
                if (returnModel != null && returnModel.First().ErrorMessage != null)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} Order Successfully Cancelled !";
                    TempData["TabOrder"] = "CORDER";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "CORDER";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "CORDER";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        [HttpPost]
        public ActionResult OrderPutOnHold(ManualOrderModel manualorder)
        {
            try
            {
                CancelOrderModel model = new CancelOrderModel { ErrorMessage = string.Empty, OrderStatus = string.Empty, TIABOrderID = string.Empty, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.OrderOnHold(model);
                if (returnModel != null && returnModel.First().ErrorMessage != null)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} Order Status Successfully Updated To On Hold";
                    TempData["TabOrder"] = "OHORDER";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "OHORDER";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "OHORDER";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        [HttpPost]
        public ActionResult UpdateToOnOrder(ManualOrderModel manualorder)
        {
            try
            {
                CancelOrderModel model = new CancelOrderModel { ErrorMessage = string.Empty, OrderStatus = string.Empty, TIABOrderID = string.Empty, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.UpdateOnOrder(model);
                if (returnModel != null && returnModel.First().ErrorMessage != null)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} Order Status Successfully Updated To On Order";
                    TempData["TabOrder"] = "ONORDER";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "ONORDER";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "ONORDER";
                return RedirectToAction("index", "ManualOrder");
            }
        }

        public ActionResult ApproveOnOrder(string VerserOrderID)
        {
            try
            {
                CancelOrderModel model = new CancelOrderModel { ErrorMessage = string.Empty, OrderStatus = string.Empty, TIABOrderID = string.Empty, VerserOrderID = VerserOrderID };
                var returnModel = Helper.Helper.UpdateOnOrder(model);
                if (returnModel != null && returnModel.First().ErrorMessage != null)
                {
                    TempData["ManualOrder"] = $"{VerserOrderID} Order Status Successfully Approved";
                    TempData["TabOrder"] = "ONORDER";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "ONORDER";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "ONORDER";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        [HttpPost]
        public ActionResult ReactivateOrderSIM(ManualOrderModel manualorder)
        {
            try
            {
                var SimActivateReuestModel = new SimActivationModel { IsActivation = true, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.SimReActivateHelper(SimActivateReuestModel);
                if (returnModel != null && returnModel.IsActivated == true)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} {returnModel.Message}";
                    TempData["TabOrder"] = "SIMACTIVATE";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "SIMACTIVATE";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "SIMACTIVATE";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        public ActionResult OrderReturn(ManualOrderModel manualorder)
        {
            try
            {
                var SimActivateReuestModel = new SimActivationModel { IsActivation = true, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.ReturnOnlyOrderHelper(SimActivateReuestModel);
                if (returnModel != null)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} {returnModel}";
                    TempData["TabOrder"] = "ROONLY";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    TempData["TabOrder"] = "ROONLY";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "ROONLY";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        [HttpGet]
        public ActionResult ShowModal()
        {
            try
            {
                return PartialView("ReturnModalView", new ManualOrderModel());
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "SIMACTIVATE";
                return RedirectToAction("index", "ManualOrder");
            }
        }
        [HttpGet]
        public ActionResult UpdateAddress(string VerserOrderID)
        {
            try
            {

                return PartialView("UpdateAddress", new ViewModel { VerserOrderID = VerserOrderID});
            }
            catch (Exception)
            {
                TempData["TabOrder"] = "ONORDER";
                return RedirectToAction("UpdateAddress", "ManualOrder");
            }
        }
        [HttpPost]
        public ActionResult UpdateOrderAddress(ViewModel theModel)
        {
            var _Validator = new UpdateOrderAddressValidatorDto();

            var ReturnFileName = "";
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
                ReturnFileName =  Helper.Helper.UpdateOrderAddress(_UpdateOrderAddressData);
                if (ReturnFileName != null)
                {
                    ReturnFileName = ReturnFileName.Trim('"');
                    TempData["ValidationErrors"] = ReturnFileName;
                }
            }
            return Json(ReturnFileName, JsonRequestBehavior.AllowGet);
        }
    }   
}