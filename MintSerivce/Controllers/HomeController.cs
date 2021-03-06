﻿using MintSerivce.Models;
//using BarcodeLib;
using MintSerivce.ServiceAgents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MintSerivce.Controllers
{
    public class HomeController : Controller
    {
        public static readonly string Filepath = ConfigurationManager.AppSettings["Filepath"];

        Helper.Helper APIHelper = new Helper.Helper();
        public ActionResult Index()
        {
            if (Session["User"] == null || Session["BearerToken"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var orderslist = GetOrderList();
            return View(orderslist);
        }
        public ActionResult DispatchedOrders()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var dispatchedorders = DispachedOrderList();
            return View(dispatchedorders);
        }
        public ActionResult SimOrders()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var simorderslist = Helper.Helper.SIMOrdersList();
            return View(simorderslist);
        }
        public ActionResult StockAvailable()
        {
            if (Session["User"] == null || Session["BearerToken"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var skustock = AvailableSKU();
            return View(skustock);
        }
        public ActionResult URLPartialView(string VerserOrderID, string ResultMessage, string URL)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            SelectedOrderModel SimModel = new SelectedOrderModel();
            SimModel.VerserOrderID = VerserOrderID;
            SimModel.ResultMessage = ResultMessage;
            SimModel.ShipLabelURL = URL;
            return View(SimModel);
        }
        public ActionResult ProcessOrder(string VerserOrderID, string ResultMessage, string URL, string OrderType)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            SelectedOrderModel selectedorder = new SelectedOrderModel();
            selectedorder.VerserOrderID = VerserOrderID;
            selectedorder.ResultMessage = ResultMessage;
            selectedorder.UserName = Session["User"].ToString();
            selectedorder.PhoneAndSim = true;
            selectedorder.OrderType = OrderType;
            if (URL != null)
            {
                URL = URL.Trim('"');
                // URL= URL.Substring(0, URL.Length - 14);
                selectedorder.ShipLabelURL = URL;
            }
            return View(selectedorder);
        }
        public ActionResult PhoneOnlyOrder(string VerserOrderID, string ResultMessage, string URL)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var selectedorder = new PhoneOnlyModel();
            selectedorder.VerserOrderID = VerserOrderID;
            selectedorder.ResultMessage = ResultMessage;
            selectedorder.UserName = Session["User"].ToString();
            if (URL != null)
            {
                URL = URL.Trim('"');
                selectedorder.ShipLabelURL = URL;
            }
            return View(selectedorder);
        }
        [HttpPost]
        public ActionResult PhoneOnlyOrder(SelectedOrderModel selectedOrder)
        {
            ModelState.Remove("ConsignmentNumber");
            ModelState.Remove("SIM");
            if (!ModelState.IsValid)
            {
                return View("ProcessOrder", selectedOrder);
            }
            else
            {
                selectedOrder.UserName = Session["User"].ToString();
                string result = ProcessOrderService(selectedOrder).Result;
                System.Threading.Thread.Sleep(5000);
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Replace('"', ' ').Trim();
                    selectedOrder.ResultMessage = result;
                }
                return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrder.VerserOrderID, ResultMessage = selectedOrder.ResultMessage });
            }
        }
        public ActionResult AddMobileToOrder(SelectedOrderModel selectedOrderDetails)
        {
            ModelState.Remove("ConsignmentNumber");
            if (!ModelState.IsValid)
            {
                return View("ProcessOrder", selectedOrderDetails);
            }
            else
            {
                selectedOrderDetails.UserName = Session["User"].ToString();
                if (selectedOrderDetails.PhoneAndSim == true && selectedOrderDetails.SIM == null)
                {
                    return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrderDetails.VerserOrderID, ResultMessage = selectedOrderDetails.ResultMessage = "SIM Is Required!" });
                }
                string Results = ProcessOrderService(selectedOrderDetails).Result;
                System.Threading.Thread.Sleep(5000);
                if (!string.IsNullOrEmpty(Results))
                {
                    Results = Results.Replace('"', ' ').Trim();
                    selectedOrderDetails.ResultMessage = Results;
                }
                return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrderDetails.VerserOrderID, ResultMessage = selectedOrderDetails.ResultMessage });
            }
        }
        public ActionResult ProcessSimOrder(SelectedOrderModel selectedOrder)
        {
            if (selectedOrder.SIM == null)
            {
                return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrder.VerserOrderID, ResultMessage = selectedOrder.ResultMessage = "SIM Is Required!", OrderType = "SimOnly" });
            }
            else if (selectedOrder.ConsignmentNumber == null)
            {

                return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrder.VerserOrderID, ResultMessage = selectedOrder.ResultMessage = "Consignment Number Is Required!", OrderType = "SimOnly" });
            }
            selectedOrder.UserName = Session["User"].ToString();

            string Strconcatinate = selectedOrder.ConsignmentNumber.Substring(0, selectedOrder.ConsignmentNumber.Length - 16);
            selectedOrder.ConsignmentNumber = Strconcatinate.Remove(0, 18);

            //  selectedOrder.ConsignmentNumber = selectedOrder.ConsignmentNumber;

            var result = ProcessSimOrderService(selectedOrder).Result;
            if (result != null)
            {
                selectedOrder.ResultMessage = result.ResultMessage;
                if (result.IsValidateState)
                {
                    PrintOrderLabel(selectedOrder.VerserOrderID);
                }
            }
            return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = selectedOrder.VerserOrderID, ResultMessage = selectedOrder.ResultMessage });
        }

        public ActionResult GetOrderShipmentLabel(string VerserOrderID)
        {
            var URL = OrderShipmentLabel(VerserOrderID).Result;
            if (URL != null)
            {

                return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = VerserOrderID, ResultMessage = "Please Go TO Print Shipment Label Tab and click On Print Shipping Label", URL = URL.URL });
            }
            return RedirectToAction("ProcessOrder", "Home", new { VerserOrderID = VerserOrderID, ResultMessage = "Order Shipment Label Doesn't Exist For This Order", URL = "" });
        }

        public ActionResult Assets(string SKU)
        {
            SKUStock AssetDetails = new SKUStock();
            var skustock = AssetLookUp(SKU).Result;
            if (skustock != null)
            {
                AssetDetails.Make = skustock.Make;
                AssetDetails.Model = skustock.Model;
                AssetDetails.Colour = skustock.Colour;
                AssetDetails.Capacity = skustock.Capacity;
                AssetDetails.SKU = skustock.SKU;
            }
            return PartialView("Assets", AssetDetails);
        }
        public static List<OrderViewModel> GetOrderList()
        {
            List<OrderViewModel> ordermodel = new List<OrderViewModel>();
            string response = string.Empty;
            string OnOrderlist = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["OnOrderList"];
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
                            ordermodel = JsonConvert.DeserializeObject<List<OrderViewModel>>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ordermodel;
        }
        public static List<OrderDispatchViewModel> DispachedOrderList()
        {

            List<OrderDispatchViewModel> ordermodel = new List<OrderDispatchViewModel>();
            string response = string.Empty;
            string OnOrderlist = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["DispachedOrderList"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(OnOrderlist);

                    resp.Wait(TimeSpan.FromSeconds(100));

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

            }
            return ordermodel;
        }
        public static List<SKUStock> AvailableSKU()
        {
            List<SKUStock> ordermodel = new List<SKUStock>();
            string response = string.Empty;
            string OnOrderlist = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["SKUAvailable"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    //   FormUrlEncodedContent cont = new FormUrlEncodedContent(lsPostContent);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(OnOrderlist);

                    resp.Wait(TimeSpan.FromSeconds(100));

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("Authorization failed. Token expired or invalid.");
                        }
                        else
                        {
                            response = resp.Result.Content.ReadAsStringAsync().Result;
                            ordermodel = JsonConvert.DeserializeObject<List<SKUStock>>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return ordermodel;
        }
        public async Task<string> ProcessOrderService(SelectedOrderModel selectedorder)
        {
            string returnmessage = string.Empty;
            string BaseUri = ConfigurationManager.AppSettings["baseUri"] + ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/MintServiceOrder/ProcessOrder", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async Task<ReturnValidationMessageDTO> ProcessSimOrderService(SelectedOrderModel selectedorder)
        {
            var returnmessage = new ReturnValidationMessageDTO();
            string BaseUri = ConfigurationManager.AppSettings["baseUri"] + ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/MintServiceOrder/ProcessSimOrder", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    returnmessage = JsonConvert.DeserializeObject<ReturnValidationMessageDTO>(result);

                }
            }
            return returnmessage;
        }
        public async static Task<ShipLabelDTO> OrderShipmentLabel(string VerserOrderID)
        {
            ShipLabelDTO ShipLabelReturn = new ShipLabelDTO();
            string BaseUri = ConfigurationManager.AppSettings["baseUri"] + ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/dispatch/{0}/OrderShipmentLabel", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<ShipLabelDTO>();

                    ShipLabelReturn = result; //Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return ShipLabelReturn;
        }
        public async static Task<OrderDto> GetOrdergreeting(string VerserOrderID)
        {
            OrderDto Order = new OrderDto();
            string BaseUri = ConfigurationManager.AppSettings["baseUri"] + ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/order/{0}/OrderDetails", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<OrderDto>();
                    Order = result;
                }
            }
            return Order;
        }
        public async static Task<SKUStock> AssetLookUp(string SKU)
        {
            SKUStock returnmessage = new SKUStock();
            string BaseUri = ConfigurationManager.AppSettings["baseUri"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("AssetManagementService/inventorycontrol/assets/{0}/AssetLookUp", SKU)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<SKUStock>();
                    returnmessage = result;// Mapper.Map<SKUStock>(result);
                }
            }
            return returnmessage;
        }

        //private void PrintLabel(string data)
        //{
        //    string labelPath = ConfigurationManager.AppSettings["LabelPath"];
        //    string printerName = ConfigurationManager.AppSettings["PrinterName"];
        //    if (labelPath == null || labelPath == string.Empty)
        //    {
        //        labelPath = Server.MapPath("\\") + "\\Images\\DymoLabel\\SKUAddressLabel.label";
        //    }
        //    var PrintContent = "";
        //    if (data != null)
        //    {
        //        PrintContent = data;

        //        var label = DYMO.Label.Framework.Label.Open(labelPath);
        //        label.SetObjectText("data", PrintContent);
        //        label.Print(printerName);
        //    }

        //}
        public ActionResult PrintOrderLabel(string VerserOrderID)
        {
            string SIMLOrderLabelURL = ConfigurationManager.AppSettings["SIMLOrderLabelURL"];
            var PrintData = PDFileBuilderService.CustomerAddressLabelRequest(VerserOrderID).Result;
            if (PrintData != null && PrintData.IsSuccess)
            {
                TempData["LabelURL"] = SIMLOrderLabelURL + VerserOrderID + ".pdf";
            }
            return Json(PrintData.FileContent, JsonRequestBehavior.AllowGet);

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
                DispatchOrdersList = DispachedOrderList();
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
            return RedirectToAction("StockAvailable", "Home");
        }
        [HttpPost]
        public ActionResult ExportSimToExcel()
        {
            List<SIMOrderModel> SimOrdersList = new List<SIMOrderModel>();
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                SimOrdersList = Helper.Helper.SIMOrdersList();
                GridView gv = new GridView();
                gv.DataSource = Helper.Helper.SIMOrdersList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=SIMOrdersList.xls");
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
        public ActionResult ExportStockToExcel()
        {
            List<SKUStock> SKUStockModel = new List<SKUStock>();
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                SKUStockModel = AvailableSKU();
                GridView gv = new GridView();
                gv.DataSource = SKUStockModel;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=AvailableStocks.xls");
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
        [HttpGet]
        public ActionResult GetOrderDetails(string orderId)
        {
            OrderDispatchViewModel model = Helper.Helper.GetOrderDetails(orderId);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportOrdersToExcel()
        {

            List<OrderViewModel> orderModel = new List<OrderViewModel>();
            if (System.Web.HttpContext.Current.Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                GridView gv = new GridView();
                gv.DataSource = GetOrderList();
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=OrdersList.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult Reports()
        {
            if (Session["User"] == null || Session["BearerToken"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var files = GetDirectoryFiles(Filepath);
            return PartialView(files);
        }
        public List<FilesModel> GetDirectoryFiles(string dir)
        {
            var d = new DirectoryInfo(dir);
            var files = new List<FilesModel>();

            foreach (FileInfo info in d.GetFiles())
            {
                var fileModel = new FilesModel
                {
                    FileName = info.Name,
                    FileDate = info.LastWriteTime,
                    FileSize = sizes(Convert.ToInt32(info.Length)),
                    FileType = info.Extension,
                    FileFullPath = info.DirectoryName
                };
                files.Add(fileModel);
            }
            return files;
        }

        public int sizes(int size)
        {
            var sizes = (size /= 1024);
            return sizes;
        }
        public FileResult DownloadFile(string path, string fileName)
        {

            var filepath = System.IO.Path.Combine(path, fileName);

            return File(filepath, MimeMapping.GetMimeMapping(filepath), fileName);
        }
    }
}