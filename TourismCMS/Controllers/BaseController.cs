using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TourismCMS.Filters;

namespace TourismCMS.Controllers
{
    [AdminUserAuthentication]
    public class BaseController : Controller
    {

    }
}