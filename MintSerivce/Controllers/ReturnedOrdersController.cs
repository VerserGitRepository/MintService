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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MintSerivce.Controllers
{
    public class ReturnedOrdersController : Controller
    {
        // GET: ReturnedOrders
        public ActionResult ReturnedOrders()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var dispatchedorders = DispatchedThnReturnedOrderList();
            return View(dispatchedorders);
            
        }

        public static List<OrderDispatchViewModel> DispatchedThnReturnedOrderList()
        {

            List<OrderDispatchViewModel> ordermodel = new List<OrderDispatchViewModel>();
            string response = string.Empty;
            string OnOrderlist = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["DispatchedThnReturnedOrders"];
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
        public ActionResult ExportDispatchToExcel()
        {
            List<OrderDispatchViewModel> DispatchOrdersList = new List<OrderDispatchViewModel>();
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                DispatchOrdersList = DispatchedThnReturnedOrderList();
                GridView gv = new GridView();
                gv.DataSource = DispatchOrdersList;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=DispatchedOrdersList.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("ReturnedOrders", "ReturnedOrders");
        }
    }
}