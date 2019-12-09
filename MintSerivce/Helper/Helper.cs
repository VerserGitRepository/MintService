using MintSerivce.Models;
using Newtonsoft.Json;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
namespace MintSerivce.Helper
{
    public class Helper
    {
        public Dictionary<string, string> GetTokenDetails()
        {
            string TokenBaseUri = System.Configuration.ConfigurationManager.AppSettings["TokenApiBaseUri"];
            string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            string Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            Dictionary<string, string> tokenDetails = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", UserName},
                       {"password", Password},
                   };

                    var resp = client.PostAsync(TokenBaseUri, new FormUrlEncodedContent(login));
                    resp.Wait(TimeSpan.FromSeconds(10));

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                        {
                            tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return tokenDetails;
        }
        public static List<OrderViewModel> GetOrderList()
        {
            List<OrderViewModel> ordermodel = new List<OrderViewModel>();
            string response = string.Empty;
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["OnOrderList"];
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
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["DispachedOrderList"];
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

            }
            return ordermodel;
        }
        public static List<SKUStock> AvailableSKU()
        {
            List<SKUStock> ordermodel = new List<SKUStock>();
            string response = string.Empty;
            string SKUAvailablelist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["SKUAvailable"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    //   FormUrlEncodedContent cont = new FormUrlEncodedContent(lsPostContent);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(SKUAvailablelist);

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
        public async Task<string> AddDeviceToOrder(SelectedOrderModel selectedorder)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/assets/AddDeviceToOrder", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async static Task<string> CreateDispatchOrder(string VerserOrderID)
        {

            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/dispatch/{0}/MintDispatchOrder", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async Task<string> AddMobileToMintDispatch(SelectedOrderModel selectedorder)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/dispatch/AddMobileToMintDispatch", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async static Task<string> CompleteDispatch(string VerserOrderID)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/dispatch/{0}/MintCompleteDispatch", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async static Task<string> OrderShipmentLabel(string VerserOrderID)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/dispatch/{0}/OrderShipmentLabel", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        public async static Task<string> OrderSummary()
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/dispatch/OrderSummary")).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
        //public async static Task<OrderDto> ReturnOrders()
        //{
        //    OrderDto returnmessage = new OrderDto();
        //    string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BaseUri);
        //        HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/Orders/ReturnOrders")).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var result = await response.Content.ReadAsAsync<OrderDto>();
        //            returnmessage = result;
        //        }
        //    }
        //    return returnmessage;
        //}
        public static List<OrderViewModel> ReturnOrders()
        {
            List<OrderViewModel> ordermodel = new List<OrderViewModel>();
            string response = string.Empty;
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["ReturnOrder"];
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
        public static string CreateOrder(ManualOrderModel NewManualOrder)
        {
            List<OrderViewModel> ordermodel = new List<OrderViewModel>();
            string response = string.Empty;
            string CreateOrderURi = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["CreateOrder"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.PostAsJsonAsync(CreateOrderURi, NewManualOrder);
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
                            if(ordermodel.First().VerserOrderID != null) { 
                            response = ordermodel.First().VerserOrderID + " Order is been created and ready to Process. Go To Orders for Processing";
                            }
                            else
                            {
                                response = "Error Occured Please Verify Your Order Request Information.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        public static string UpdateSKUBufferValue(string SKU, string SKUBuffer)
        {           
            string response = string.Empty;
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["ListItems"]+SKU+"/"+SKUBuffer+"/UpdateSKUBuffer";
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
                            response = JsonConvert.DeserializeObject<string>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message.ToString();
            }
            return response;
        }

        public static List<ListItemModel> SKUList()
        {
            var response =  new List<ListItemModel>();
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["SKUsList"];
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
                           // response = resp.Result.Content.ReadAsStringAsync().Result;

                            response = JsonConvert.DeserializeObject<List<ListItemModel>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                //response = ex.Message.ToString();
            }
            return response;
        }
        public static string UpdateOrderAddress(UpdateOrderAddressDto theModel)
        {
            string response = string.Empty;
            var _ReturnDto = new ReturnDto();
            string uri = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["UpdateOrderAddress"];
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.PostAsJsonAsync(uri, theModel);
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
                            _ReturnDto = JsonConvert.DeserializeObject<ReturnDto>(response);
                            response = _ReturnDto.ErrorMessage;
                            if (string.IsNullOrEmpty(response))
                            {
                                response = _ReturnDto.OrderStatus;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = ex.Message.ToString();
            }
            return response;
        }
        public static List<ListItemModel> OrdersList()
        {
            var response = new List<ListItemModel>();
            
            string OnOrderlist = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + System.Configuration.ConfigurationManager.AppSettings["Orderslist"];
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
                            response = JsonConvert.DeserializeObject<List<ListItemModel>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return response;
        }
    }
}