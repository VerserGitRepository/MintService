using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MintSerivce.Models;
using System.Net.Http;
using System.Threading.Tasks;
namespace MintSerivce.Helper
{
    public class ImportAssetService
    {
      static  string BaseUri = System.Configuration.ConfigurationManager.AppSettings["baseUri"] + System.Configuration.ConfigurationManager.AppSettings["rootSite"];
        public static bool ImportAssets(List<ImportAssetViewModel> assetList)
        {           
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                //HTTP POST
                var postTask = client.PostAsJsonAsync(string.Format("NumobileFeedService/ImportAssets"), assetList);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;  
                }
                else
                {
                    return false;
                }
            }
        }
    }
}