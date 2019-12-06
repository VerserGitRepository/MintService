using MintSerivce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MintSerivce.Helper
{
    public class CreateNewSKU
    {
        public static async Task<string> AddNewSKU(SKUModel selectedorder)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/MintServiceOrder/CreateMobileSKU", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
    }
}