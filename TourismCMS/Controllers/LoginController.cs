using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TourismCMS.Helper;
using TourismCMS.Services;
using TourismCMS.TranslateResources;


namespace TourismCMS.Controllers
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
            var browserCookie = Request.Cookies[AccountHashKeys.AdminUserBrowserCookie];

            if (browserCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(browserCookie.Value);
                var email = ticket.Name;
                var password = ticket.UserData;
                var currentAdminUser = UserService.Instance.GetByAccountId(email);
                if (currentAdminUser == null)
                {
                    ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);
                    return View();
                }
                Session.Add(AccountHashKeys.CurrentAdminUser, currentAdminUser);
                return RedirectToAction("index", "default");
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session[AccountHashKeys.CurrentAdminUser] = null;
            Session.Remove(AccountHashKeys.CurrentAdminUser);
            ClearClientCookie(AccountHashKeys.AdminUserBrowserCookie);

            return RedirectToAction("login", "login");
        }

        [HttpPost]
        public ActionResult Login(string strUserName, string strPassword)
        {
            var oResult = new Object();
            if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
            {
                oResult = new
                {
                    Bresult = true,
                    Notice = "用户名,密码不能为空!"
                };
                return Json(oResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var currentUser = UserService.Instance.GetByAccountId(strUserName.Trim());                
                bool isAdminUserValid = false;
                if (currentUser != null)
                    isAdminUserValid = currentUser != null && BCrypt.Net.BCrypt.Verify(strPassword, currentUser.PasswordSalt);

                //if (currentUser != null && !string.Equals(strPassword, currentUser.PasswordSalt))
                if (!isAdminUserValid)
                {
                    oResult = new
                    {
                        Bresult = false,
                        Notice = "密码错误!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }
                //if (currentUser != null && string.Equals(strPassword, currentUser.PasswordSalt))
                if (isAdminUserValid)
                {
                    //Session.Add("strDomainID", currentUser);
                    Session.Add(AccountHashKeys.CurrentAdminUser, currentUser);
                    oResult = new
                    {
                        Bresult = true,
                        Url = "/default/index",
                        Notice = "成功!"
                    };
                    return Json(oResult, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("index", "default");
        }

        private void ClearClientCookie(string cookieKey)
        {
            if (Request.Cookies[cookieKey] != null)
            {
                Response.Cookies.Add(new HttpCookie(cookieKey)
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
        }
    }
}