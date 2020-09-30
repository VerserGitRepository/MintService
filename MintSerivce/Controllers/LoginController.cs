using MintSerivce.Helper;
using MintSerivce.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MintSerivce.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {

            Session.Clear();
            Session.Abandon();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            Session["User"] = null;
            Session["ErrorMessage"] = null;
            return View();
        }
        public ActionResult submit(string username, string password)
        {
            Session["User"] = null;
            Session["ErrorMessage"] = null;

            if (string.IsNullOrEmpty(password))
            {
                Session["ErrorMessage"] = "Please Enter Password !";
                return RedirectToAction("Login", "Login");
            }
            if (string.IsNullOrEmpty(username))
            {
                Session["ErrorMessage"] = "Please Enter User Name !";
                return RedirectToAction("Login", "Login");
            }
            if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
            {
                UserModel login = new UserModel();
                login.UserName = username;
                login.Password = password;
                var token = TokenInitiator.GetTokenDetails(username, password);
                if (token != null)
                {
                    System.Web.HttpContext.Current.Session["BearerToken"] = token.FirstOrDefault().Value.ToString();
                    Session["LoginCount"] = null;
                }
                else
                {
                    login.IsAccountLocked = true;
                    login.AccessFailedCount = 1;
                    var lockReturn = LockAccount(login).Result;
                    TempData["ErrorMessage"] = lockReturn.ErrorMessage;
                    return RedirectToAction("Login", "Login");
                }
                var LoginResult = Login(login).Result;
                if (LoginResult.IsLogged == true && LoginResult.IsUiEligible == "Yes")
                {
                    Session["User"] = LoginResult.UserName;
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorMessage"] = LoginResult.ErrorMessage;
            }
            return RedirectToAction("Login", "Login");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            Session["User"] = null;
            Session["ErrorMessage"] = null;
            return RedirectToAction("Login", "Login");
        }
        public async static Task<UserModel> Login(UserModel login)
        {
            UserModel returnmessage = new UserModel();
            string BaseUri = ConfigurationManager.AppSettings["BaseGatewayAPILogin"] + ConfigurationManager.AppSettings["RootGatewayAPILogin"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("Login", login).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<UserModel>();
                    returnmessage = result;
                }
            }
            return returnmessage;
        }
        public async static Task<UserModel> LockAccount(UserModel login)
        {
            UserModel returnmessage = new UserModel();
            string BaseUri = ConfigurationManager.AppSettings["BaseGatewayAPILogin"] + ConfigurationManager.AppSettings["RootGatewayAPILogin"];

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("LockAccount", login).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<UserModel>();
                    returnmessage = result;
                }
            }
            return returnmessage;
        }
    }
}