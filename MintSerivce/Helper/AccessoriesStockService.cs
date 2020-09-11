using MintSerivce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MintSerivce.Helper
{
    public class AccessoriesStockService
    {
        public static async Task<List<AccessoriesStockCountDto>> AvailableAccessoriesStock()
        {
            var _AccessoriesStockCounts = new List<AccessoriesStockCountDto>();
            
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/MintServiceOrder/AvailableAccessoriesStock")).Result;
                if (response.IsSuccessStatusCode)
                {                   
                    _AccessoriesStockCounts = await response.Content.ReadAsAsync<List<AccessoriesStockCountDto>>();
                }
            }
            return _AccessoriesStockCounts;
        }


        public static async Task<ReturnValidationMessageDTO> UpdateAccessoriesCount(int AccessoryId, int AddAccessoriesCount, int RemoveAccessoriesCount)
        {
            var Returnresponse = new ReturnValidationMessageDTO();
            string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/MintServiceOrder/UpdateAccessoriesCount/{0}/{1}/{2}",AccessoryId, AddAccessoriesCount, RemoveAccessoriesCount)).Result;
                if (response.IsSuccessStatusCode)
                {
                    Returnresponse = await response.Content.ReadAsAsync<ReturnValidationMessageDTO>();
                }
            }
            return Returnresponse;
        }
     }
}