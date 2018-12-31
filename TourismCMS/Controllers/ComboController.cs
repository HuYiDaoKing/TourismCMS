using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TourismCMS.EntityDataModel;
using TourismCMS.Services;
using TourismCMS.Models;
using TourismCMS.TranslateResources;

namespace TourismCMS.Controllers
{
    public class ComboController : Controller
    {
        // GET: Combo
        public ActionResult Index()
        {
            return View();
        }

        //线路下拉列
        public JsonResult GetRouteComboData()
        {
            List<object> _reuslts = new List<object>();
            List<Route> routes = RouteService.Instance.GetAll();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach (Route route in routes)
            {
                _reuslts.Add(new { Id = route.Id, Text = route.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //门票下拉列
        public JsonResult GetTicketComboData()
        {
            List<object> _reuslts = new List<object>();
            List<Ticket> tickets = TicketService.Instance.GetAll();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach (Ticket ticket in tickets)
            {
                _reuslts.Add(new { Id = ticket.Id, Text = ticket.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //酒店下拉列
        public JsonResult GetHotelComboData()
        {
            List<object> _reuslts = new List<object>();
            List<Hotel> hotels = HotelService.Instance.GetAll();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach (Hotel hotel in hotels)
            {
                _reuslts.Add(new { Id = hotel.Id, Text = hotel.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //供应商下拉列
        public JsonResult GetSupplierComboData(string type)
        {
            List<object> _reuslts = new List<object>();
            List<Supplier> suppliers = SupplierService.Instance.GetAll(type);
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach (Supplier supplier in suppliers)
            {
                //_reuslts.Add(new { Id = supplier.Id, Text = supplier.Manager });
                _reuslts.Add(new { Id = supplier.Id, Text = supplier.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //渠道下拉列
        public JsonResult GetChannelComboData(string type)
        {
            List<object> _reuslts = new List<object>();
            List<Channel> channels = ChannelService.Instance.GetAll(type);
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach (Channel channel in channels)
            {
                _reuslts.Add(new { Id = channel.Id, Text = channel.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //评审状态下拉
        public JsonResult GetReviewStatusComboData()
        {
            List<object> _reuslts = new List<object>();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach(var kv in ReviewStatusManager.ReviewStatus)
            {
                _reuslts.Add(new { Id = kv.Key, Text =kv.Value });
            }

            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //结算状态下拉
        public JsonResult GetBalanceStatusComboData()
        {
            List<object> _reuslts = new List<object>();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach(var kv in BalanceStatusManager.BalanceStatus)
            {
                _reuslts.Add(new { Id = kv.Key, Text = kv.Value });
            }

            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //收款状态下拉
        public JsonResult GetCollectStatusComboData()
        {
            List<object> _reuslts = new List<object>();
            _reuslts.Add(new { Id = -1, Text = "全部" });
            foreach(var kv in CollectStatusManager.CollectStatus)
            {
                _reuslts.Add(new { Id = kv.Key, Text = kv.Value });
            }

            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //销售员
        public JsonResult GetSellerComboData()
        {
            List<object> _reuslts = new List<object>();
            _reuslts.Add(new { Id = -1, Text = "全部" });

            List<User> users = UserService.Instance.GetSellers();

            foreach(User user in users)
            {
                _reuslts.Add(new { Id = user.Id, Text = user.Name });
            }

            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

    }
}