using MintSerivce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MintSerivce.Helper
{
    public class RegisterNewUserService
    {
        public static bool RegisterNewUser(NewUserRegisterModel NewUserRequest)
        {
          bool returnresponse = false;
            string NewuserAddUri = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + "api/account/register";
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.PostAsJsonAsync(NewuserAddUri, NewUserRequest);
                    resp.Wait(TimeSpan.FromSeconds(10));
                    if (resp.IsCompleted)
                    {
                        if (resp.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("Authorization failed. Token expired or invalid.");
                        }
                        else
                        {
                           var response = resp.Result.Content.ReadAsStringAsync().Result;
                            returnresponse = JsonConvert.DeserializeObject<bool>(response);                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnresponse = false;
            }
            return returnresponse;
        }

        public static bool UpdateUserPassword(NewUserRegisterModel changeUserpasswordRequest)
        {
            bool returnresponse = false;
            string NewuserAddUri = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + "api/account/UpdatePassword";
            string token = System.Web.HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.PostAsJsonAsync(NewuserAddUri, changeUserpasswordRequest);
                    resp.Wait(TimeSpan.FromSeconds(10));
                    if (resp.IsCompleted)
                    {
                        if (resp.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("Authorization failed. Token expired or invalid.");
                        }
                        else
                        {
                            var response = resp.Result.Content.ReadAsStringAsync().Result;
                            returnresponse = JsonConvert.DeserializeObject<bool>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnresponse = false;
            }
            return returnresponse;
        }

        public static bool UnLockUserAccount(string username)
        {
            bool returnresponse = false;
            string NewuserAddUri = System.Configuration.ConfigurationManager.AppSettings["rooturi"] + $"api/account/{username}/UnLockUserAccount";
            string token = HttpContext.Current.Session["BearerToken"].ToString();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var resp = client.GetAsync(NewuserAddUri);
                    resp.Wait(TimeSpan.FromSeconds(10));
                    if (resp.IsCompleted)
                    {
                        if (resp.Result.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            Console.WriteLine("Authorization failed. Token expired or invalid.");
                        }
                        else
                        {
                            var response = resp.Result.Content.ReadAsStringAsync().Result;
                            returnresponse = JsonConvert.DeserializeObject<bool>(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnresponse = false;
            }
            return returnresponse;
        }
    }
}