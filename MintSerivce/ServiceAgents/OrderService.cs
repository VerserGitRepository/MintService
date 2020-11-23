using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MintSerivce.Models;

namespace MintSerivce.ServiceAgents
{
    public class OrderService
    {
        public static string APIEndPointUri = ConfigurationManager.AppSettings["rooturi"] + ConfigurationManager.AppSettings["ShopifyOrdersList"];
        public static List<OrderViewModel> ShopifyOnHoldOrders()
        {
            var ordermodel = new List<OrderViewModel>();
            string response = string.Empty;
         
            string token = HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(APIEndPointUri);
                    resp.Wait(TimeSpan.FromSeconds(200));

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
    }
}