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
                    return View();
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
             return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ReturnedOrder(ManualOrderModel manualOrder)
        {
            if (ModelState.IsValid)
            {
                if (manualOrder.VerserOrderID !=null)
                {
                  var returnmessage = Helper.Helper.CreateReturnOrder(manualOrder);
                    if (returnmessage != null && returnmessage.ErrorMessage !=null)
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
            return View();
        }

    }   
}