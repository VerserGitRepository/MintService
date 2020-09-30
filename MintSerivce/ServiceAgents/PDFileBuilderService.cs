using MintSerivce.Models;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace MintSerivce.ServiceAgents
{
    public class PDFileBuilderService
    {
        public async static Task<PDFFileReturnModel> CustomerAddressLabelRequest(string VerserOrderID)
        {
            var returndata = new PDFFileReturnModel();
            string BaseUri = ConfigurationManager.AppSettings["PDFBaseUri"] + ConfigurationManager.AppSettings["PDFRootSite"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("Api/PDFBuilder/GenerateFile/{0}", VerserOrderID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<PDFFileReturnModel>();
                    returndata = result;
                }
            }
            return returndata;
        }
    }
}