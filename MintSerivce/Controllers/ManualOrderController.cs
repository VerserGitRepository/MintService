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
       
    }   
}