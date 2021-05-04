using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace MintSerivce.Helper
{
    public class TokenInitiator
    {
        public static Dictionary<string, string> GetTokenDetails(string UserName, string Password)
        {
            string TokenBaseUri = ConfigurationManager.AppSettings["TokenApiBaseUri"];

            Dictionary<string, string> tokenDetails = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"Content-Type", "application/x-www-form-urlencoded"},
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
    }
}