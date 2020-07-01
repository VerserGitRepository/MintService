using MintSerivce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MintSerivce.Controllers
{
    public class CancelledOrdersController : Controller
    {
        public ActionResult CancelledOrders()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var dispatchedorders = CancelledOrderList();
            return View(dispatchedorders);
            
        }

        public static List<OrderDispatchViewModel> CancelledOrderList()
        {

            List<OrderDispatchViewModel> ordermodel = new List<OrderDispatchViewModel>();
            string response = string.Empty;
            string OnOrderlist = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["CancelledOrders"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(OnOrderlist);

                    resp.Wait(TimeSpan.FromSeconds(10));

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("Authorization failed. Token expired or invalid.");
                        }
                        else
                        {
                            response = resp.Result.Content.ReadAsStringAsync().Result;

                            ordermodel = JsonConvert.DeserializeObject<List<OrderDispatchViewModel>>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ordermodel;
        }


        [HttpPost]
        public ActionResult ExportCancelledOrders()
        {
            var cancelledOrdersList = new List<OrderDispatchViewModel>();
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                cancelledOrdersList = CancelledOrderList();
                GridView gv = new GridView();
                gv.DataSource = cancelledOrdersList;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CancelledOrders.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("StockAvailable", "Home");
        }
    }
}