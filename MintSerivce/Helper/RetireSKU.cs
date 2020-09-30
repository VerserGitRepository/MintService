using MintSerivce.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace MintSerivce.Helper
{
    public class RetireSKU
    {
        public static async Task<string> RetireExistingSKU(SKUModel selectedorder)
        {
            string returnmessage = string.Empty;
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("inventorycontrol/MintServiceOrder/RemoveSKU", selectedorder).Result;
                if (response.IsSuccessStatusCode)
                {
                    returnmessage = Convert.ToString(await response.Content.ReadAsStringAsync());
                }
            }
            return returnmessage;
        }
    }
}