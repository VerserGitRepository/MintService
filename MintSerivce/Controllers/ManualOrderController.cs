using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MintSerivce.Models;
using MintSerivce.Helper;
namespace MintSerivce.Controllers
{
    public class ManualOrderController : Controller
    {   // GET: ManualOrder
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                string username = Session["User"].ToString();
                if (username == "VerserMintAdmin@verser.com.au")
                {
                    ManualOrderModel model = new ManualOrderModel();
                    List<ListItemModel> ordersList =  Helper.Helper.CancelOrdersList(); ;
                    model.OrdersListItemModel = new List<SelectListItem>();
                    foreach (ListItemModel item in ordersList)
                    {
                        model.OrdersListItemModel.Add(new SelectListItem { Text = item.Value });
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
              string returnmessage=Helper.Helper.CreateOrder(manualOrder);
                TempData["ManualOrder"] = returnmessage.ToString();
                ModelState.Clear();
                return RedirectToAction("index", "ManualOrder");
            }
            return RedirectToAction("index", "ManualOrder");
        }

        [HttpPost]
        public ActionResult ReturnedOrder(ManualOrderModel manualOrder)
        {
            if (ModelState.IsValid)
            {
                if (manualOrder.VerserOrderID !=null)
                {
                  var returnmessage = Helper.Helper.CreateReturnOrder(manualOrder);
                    if (returnmessage != null && returnmessage.ErrorMessage !=null && returnmessage.OrderStatus == "ERROR")
                    {
                        TempData["OrderError"] = $"{returnmessage.VerserOrderID} {returnmessage.ErrorMessage}";
                    }
                    else{
                        TempData["ManualOrder"]= $" Order {returnmessage.VerserOrderID} Return Order {returnmessage.VerserReturnOrderID} is been created {returnmessage.OrderStatus} and ready to Process. Go To Orders for Processing";
                    }
              
                    ModelState.Clear();
                    return RedirectToAction("index", "ManualOrder");
                }
                else
                {
                    TempData["ManualOrder"] = "Verser Order Is Mandatory";
                    return RedirectToAction("index", "ManualOrder");
                }
               
            }
            return RedirectToAction("index", "ManualOrder");
        }
        [HttpPost]
        public ActionResult CancelOrder(ManualOrderModel manualorder)
        {
            try
            {
                CancelOrderModel model = new CancelOrderModel { ErrorMessage = string.Empty, OrderStatus =string.Empty, TIABOrderID = string.Empty, VerserOrderID = manualorder.VerserOrderID };
                var returnModel = Helper.Helper.CancelOrder(model);
                if (returnModel != null && returnModel.First().ErrorMessage != null)
                {
                    TempData["ManualOrder"] = $"{manualorder.VerserOrderID} Order Successfully Cancelled !";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
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
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
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
                    TempData["ManualOrder"]   = $"{manualorder.VerserOrderID} Order Status Successfully Updated To On Order"; 
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["OrderError"] = "Error has occurred while processing the request.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("index", "ManualOrder");
            }
        }

        
    }   
}