using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TourismCMS.Controllers
{
    public class BulletinController : BaseController
    {
        // GET: Bulletin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }


        //public JsonResult Add(string title,string content)
        //{
        //    var result = new object();
        //    return Json(result,JsonRequestBehavior.AllowGet);
        //}


    }
}