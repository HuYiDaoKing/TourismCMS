using System.Web.Mvc;
using TourismCMS.TranslateResources;
using TourismCMS.EntityDataModel;

namespace TourismCMS.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: Default
        public ActionResult Index()
        {
            //var currentAdminUser = HttpContext.Session[SessionResource.UserSessionKey] as AdminUser;
            var currentAdminUser = HttpContext.Session[SessionResource.UserSessionKey] as User;
            ViewData["UserId"] = null == currentAdminUser ? string.Empty : currentAdminUser.Email;
            return View();
        }
    }
}