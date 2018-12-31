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
using TourismCMS.Helper;

namespace TourismCMS.Controllers
{
    public class RouteOrderController : Controller
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd";

        // GET: RouteOrder
        public ActionResult Index()
        {
            User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
            if (currentUser == null)
                return View();

            //if (string.Equals(currentUser.AccountId, SuperAdminAccount.SUPER_ADMIN_ACCOUNT))
            //{
            //    ViewData["RoleName"] = "超级管理员";
            //}
            //else
            //{
            //    UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
            //    Role role = RoleService.Instance.GetById(userRole.RoleId);
            //    ViewData["RoleName"] = role.Name;
            //}
            UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
            Role role = RoleService.Instance.GetById(userRole.RoleId);
            ViewData["RoleName"] = role.Name;
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult GetTableRecords(int start, int limit,
            string routeId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string flag, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            var results = new Object();
            var routeOrders = new List<RouteOrder>();
            try
            {
                int total = 0;
                int pageNum = 0;

                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                routeOrders = RouteOrderService.Instance.GetBySomeWhere(currentUser, routeId, channelId, supplierId, balacnestatusId, collectstatusId, flag, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                //统计
                /*int? totalAdults = 0;//成人总数
                int? totalChild = 0;//小孩总数
                decimal? totalCost = 0;//成本总额
                decimal? totalSell = 0;//销售总额
                decimal? totalProxy = 0;//代收总额
                decimal? totalPay = 0;//应付总额
                decimal? totalProfit = 0;//总利润
                decimal? totalCollect = 0;//应收总额*/

                var newRouteOrders = new List<Object>();
                foreach (RouteOrder routeOrder in routeOrders)
                {
                    /*totalAdults += routeOrder.AdultNum;
                    totalChild += routeOrder.ChildNum;
                    totalCost += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse(routeOrder.ProxyIncome.ToString()),3);
                    totalPay += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((routeOrder.AdultNum * (routeOrder.AdultSellPrice - routeOrder.AdultOriginalPrice) + routeOrder.ChildNum * (routeOrder.ChildSellPrice - routeOrder.ChildOriginalPrice) + (routeOrder.ExtrasSellPrice - routeOrder.ExtrasOriginPrice)).ToString()), 3);

                    totalCollect += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice - routeOrder.ProxyIncome).ToString()), 3);*/

                    string seller = string.Empty;
                    string route = string.Empty;
                    string supplier = string.Empty;
                    string channel = string.Empty;

                    if (routeOrder.HandlerId != null)
                        seller = UserService.Instance.GetById(routeOrder.HandlerId).Name;

                    if (routeOrder.RouteId != null)
                    {
                        Route routeItem = RouteService.Instance.GetById(routeOrder.RouteId);
                        if (routeItem != null)
                            route = routeItem.Name;
                    }
                        

                    if (routeOrder.SupplierId != null)
                        supplier = SupplierService.Instance.GetById(routeOrder.SupplierId).Name;

                    if (routeOrder.ChannelId != null)
                        channel = ChannelService.Instance.GetById(routeOrder.ChannelId).Name;

                    newRouteOrders.Add(new
                    {
                        Id = routeOrder.Id,
                        UserId = routeOrder.UserId,
                        //HotelId = hotelOrder.HotelId,
                        RouteId = routeOrder.RouteId,
                        GuestName = routeOrder.GuestName,
                        GuestPhone = routeOrder.GuestPhone,
                        AdultNum = routeOrder.AdultNum,
                        AdultOriginalPrice = routeOrder.AdultOriginalPrice,
                        AdultSellPrice = routeOrder.AdultSellPrice,
                        ChildNum = routeOrder.ChildNum,
                        ChildOriginalPrice = routeOrder.ChildOriginalPrice,
                        ChildSellPrice = routeOrder.ChildSellPrice,
                        ExtrasOriginPrice = routeOrder.ExtrasOriginPrice,
                        ExtrasSellPrice = routeOrder.ExtrasSellPrice,
                        BookTime = null == routeOrder.BookTime ? string.Empty : DateTime.Parse(routeOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                        StartDate = null == routeOrder.StartDate ? string.Empty : DateTime.Parse(routeOrder.StartDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        EndDate = null == routeOrder.EndDate ? string.Empty : DateTime.Parse(routeOrder.EndDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ProxyIncome = routeOrder.ProxyIncome,
                        ChannelId = routeOrder.ChannelId,
                        SupplierId = routeOrder.SupplierId,
                        BalanceStatus = BalanceStatusManager.BalanceStatus[routeOrder.BalanceStatus],
                        BalanceStatusId = routeOrder.BalanceStatus,
                        BalanceDate = null == routeOrder.BalanceDate ? string.Empty : DateTime.Parse(routeOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        BalanceRemark = routeOrder.BalanceRemark,
                        CollectMoneyStatus = CollectMoneyStatusManager.CollectMoneyStatus[routeOrder.CollectMoneyStatus],
                        CollectMoneyStatusId = routeOrder.CollectMoneyStatus,
                        CollectMoneyDate = null == routeOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(routeOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ReviewStatus = ReviewStatusManager.ReviewStatus[routeOrder.ReviewStatus],
                        ReviewStatusId = routeOrder.ReviewStatus,
                        Description = routeOrder.Description,
                        Created = DateTime.Parse(routeOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(routeOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                        Seller = seller,
                        Route = route,
                        Supplier = supplier,
                        Channel = channel,
                        CollectMoneyRemark = routeOrder.CollectMoneyRemark,
                        Settlement = routeOrder.Settlement,
                        ModifierId = routeOrder.ModifierId,
                        CostTotal = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice).ToString()), 3),//成本总额
                        SalesTotal = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice).ToString()), 3),//销售总额
                        ShouldPay = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice - routeOrder.ProxyIncome).ToString()), 3),//应付
                        ShouldCollect = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice - routeOrder.ProxyIncome).ToString()),3),//应收
                        Profit = Math.Round(Decimal.Parse((routeOrder.AdultNum * (routeOrder.AdultSellPrice - routeOrder.AdultOriginalPrice) + routeOrder.ChildNum * (routeOrder.ChildSellPrice - routeOrder.ChildOriginalPrice) + (routeOrder.ExtrasSellPrice - routeOrder.ExtrasOriginPrice)).ToString()), 3)  //利润
                    });
                }

                //RouteOrderStatics statics = new RouteOrderStatics
                //{
                //    TotalAdults = totalAdults,
                //    TotalChild = totalChild,
                //    TotalCost = totalCost,
                //    TotalSell = totalSell,
                //    TotalProxy = totalProxy,
                //    TotalPay = totalPay,
                //    TotalProfit = totalProfit,
                //    TotalCollect = totalCollect
                //};

                results = new
                {
                    pageNum = pageNum,
                    total = total,
                    routeOrders = newRouteOrders
                    //Statics = statics
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetById(int id)
        {
            RouteOrder routeOrder = new RouteOrder();
            var newRouteOrder = new Object();
            try
            {
                routeOrder = RouteOrderService.Instance.GetById(id);

                if (routeOrder == null)
                    return null;

                string seller = string.Empty;
                string route = string.Empty;
                string supplier = string.Empty;
                string channel = string.Empty;

                if (routeOrder.HandlerId != null)
                    seller = UserService.Instance.GetById(routeOrder.HandlerId).Name;

                if (routeOrder.RouteId != null)
                    route = RouteService.Instance.GetById(routeOrder.RouteId).Name;

                if (routeOrder.SupplierId != null)
                    supplier = SupplierService.Instance.GetById(routeOrder.SupplierId).Name;

                if (routeOrder.ChannelId != null)
                    channel = ChannelService.Instance.GetById(routeOrder.ChannelId).Name;

                newRouteOrder = new
                {
                    Id = routeOrder.Id,
                    UserId = routeOrder.UserId,
                    //HotelId = hotelOrder.HotelId,
                    RouteId = routeOrder.RouteId,
                    GuestName = routeOrder.GuestName,
                    GuestPhone = routeOrder.GuestPhone,
                    AdultNum = routeOrder.AdultNum,
                    AdultOriginalPrice = routeOrder.AdultOriginalPrice,
                    AdultSellPrice = routeOrder.AdultSellPrice,
                    ChildNum = routeOrder.ChildNum,
                    ChildOriginalPrice = routeOrder.ChildOriginalPrice,
                    ChildSellPrice = routeOrder.ChildSellPrice,
                    ExtrasOriginPrice = routeOrder.ExtrasOriginPrice,
                    ExtrasSellPrice = routeOrder.ExtrasSellPrice,
                    BookTime = null == routeOrder.BookTime ? string.Empty : DateTime.Parse(routeOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                    StartDate = null == routeOrder.StartDate ? string.Empty : DateTime.Parse(routeOrder.StartDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    EndDate = null == routeOrder.EndDate ? string.Empty : DateTime.Parse(routeOrder.EndDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    ProxyIncome = routeOrder.ProxyIncome,
                    ChannelId = routeOrder.ChannelId,
                    SupplierId = routeOrder.SupplierId,
                    BalanceStatus = BalanceStatusManager.BalanceStatus[routeOrder.BalanceStatus],
                    BalanceStatusId = routeOrder.BalanceStatus,
                    BalanceDate = null == routeOrder.BalanceDate ? string.Empty : DateTime.Parse(routeOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    BalanceRemark = routeOrder.BalanceRemark,
                    CollectMoneyStatus = CollectMoneyStatusManager.CollectMoneyStatus[routeOrder.CollectMoneyStatus],
                    CollectMoneyStatusId = routeOrder.CollectMoneyStatus,
                    CollectMoneyDate = null == routeOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(routeOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    ReviewStatus = ReviewStatusManager.ReviewStatus[routeOrder.ReviewStatus],
                    ReviewStatusId = routeOrder.ReviewStatus,
                    Description = routeOrder.Description,
                    Created = DateTime.Parse(routeOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                    Modified = DateTime.Parse(routeOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                    Seller = seller,
                    Route = route,
                    Supplier = supplier,
                    Channel = channel,
                    CollectMoneyRemark = routeOrder.CollectMoneyRemark,
                    Settlement = routeOrder.Settlement,
                    ModifierId = routeOrder.ModifierId,
                    CostTotal = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.AdultOriginalPrice).ToString())),//成本总额
                    SalesTotal = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice).ToString())),//销售总额
                    ShouldPay = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.AdultOriginalPrice - routeOrder.ProxyIncome).ToString())),//应付
                    ShouldCollect = Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice - routeOrder.ProxyIncome).ToString())),//应收
                    Profit = Math.Round(Decimal.Parse((routeOrder.AdultNum * (routeOrder.AdultSellPrice - routeOrder.AdultOriginalPrice) + routeOrder.ChildNum * (routeOrder.ChildSellPrice - routeOrder.ChildOriginalPrice)).ToString()))  //利润
                };
            }
            catch
            {

            }
            return Json(newRouteOrder, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatics(string routeId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string flag, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            //统计
            int? totalAdults = 0;//成人总数
            int? totalChild = 0;//小孩总数
            decimal? totalCost = 0;//成本总额
            decimal? totalSell = 0;//销售总额
            decimal? totalProxy = 0;//代收总额
            decimal? totalPay = 0;//应付总额
            decimal? totalProfit = 0;//总利润
            decimal? totalCollect = 0;//应收总额
            try
            {
                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                List<RouteOrder> routeOrders = RouteOrderService.Instance.GetBySomeWhere(currentUser, routeId, channelId, supplierId, balacnestatusId, collectstatusId, flag, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword);
                foreach (RouteOrder routeOrder in routeOrders)
                {
                    totalAdults += routeOrder.AdultNum;
                    totalChild += routeOrder.ChildNum;
                    totalCost += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse(routeOrder.ProxyIncome.ToString()), 3);
                    totalPay += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultOriginalPrice + routeOrder.ChildNum * routeOrder.ChildOriginalPrice + routeOrder.ExtrasOriginPrice - routeOrder.ProxyIncome).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((routeOrder.AdultNum * (routeOrder.AdultSellPrice - routeOrder.AdultOriginalPrice) + routeOrder.ChildNum * (routeOrder.ChildSellPrice - routeOrder.ChildOriginalPrice) + (routeOrder.ExtrasSellPrice - routeOrder.ExtrasOriginPrice)).ToString()), 3);

                    totalCollect += Math.Round(Decimal.Parse((routeOrder.AdultNum * routeOrder.AdultSellPrice + routeOrder.ChildNum * routeOrder.ChildSellPrice + routeOrder.ExtrasSellPrice - routeOrder.ProxyIncome).ToString()), 3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            RouteOrderStatics statics = new RouteOrderStatics
            {
                TotalAdults = totalAdults,
                TotalChild = totalChild,
                TotalCost = totalCost,
                TotalSell = totalSell,
                TotalProxy = totalProxy,
                TotalPay = totalPay,
                TotalProfit = totalProfit,
                TotalCollect = totalCollect
            };

            return Json(statics, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert(
                int routeId,
                int channelId,
                int supplierId,
                string bookTime,
                string startDate,
                string endDate,
                string guestName,
                string guestPhone,
                int adultNum,
                double adultOriginalPrice,
                double adultSellPrice,
                int childNum,
                double childOriginalPrice,
                double childSellPrice,
                double extrasOriginPrice,
                double extrasSellPrice,
                string proxyIncome,
                string description
            )
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                RouteOrder routeOrder = new RouteOrder
                {
                    UserId = currentAdminUser.Id,
                    RouteId = routeId,
                    GuestName = guestName,
                    GuestPhone = guestPhone,
                    //BookTime=DateTime.Parse(bo),
                    AdultNum = adultNum,
                    AdultOriginalPrice = adultOriginalPrice,
                    AdultSellPrice = adultSellPrice,
                    ChildNum = childNum,
                    ChildOriginalPrice = childOriginalPrice,
                    ChildSellPrice = childSellPrice,
                    ExtrasOriginPrice = extrasOriginPrice,
                    ExtrasSellPrice = extrasSellPrice,
                    BookTime = DateTime.Parse(bookTime),
                    StartDate = DateTime.Parse(startDate),
                    EndDate = DateTime.Parse(endDate),
                    ProxyIncome = string.IsNullOrEmpty(proxyIncome) ? 0 : double.Parse(proxyIncome),
                    ChannelId = channelId,
                    SupplierId = supplierId,
                    ReviewStatus = 0,//创建时默认设置为待评审
                    CollectMoneyStatus = 0,
                    BalanceStatus = 0,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Description=description,
                    HandlerId = currentAdminUser.Id,
                    ModifierId = currentAdminUser.Id,
                    Settlement = (adultNum * adultOriginalPrice + childNum * adultOriginalPrice - double.Parse(proxyIncome)) > 0 ? "挂账" : "返佣"
                };

                bRet = RouteOrderService.Instance.Create(routeOrder);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(
            int id,
            int routeId,
                int channelId,
                int supplierId,
                string bookTime,
                string startDate,
                string endDate,
                string guestName,
                string guestPhone,
                int adultNum,
                double adultOriginalPrice,
                double adultSellPrice,
                int childNum,
                double childOriginalPrice,
                double childSellPrice,
                double extrasOriginPrice,
                double extrasSellPrice,
                string proxyIncome,
                string description)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                UserRole userRole = UserRoleService.Instance.GetByUserId(currentAdminUser.Id);
                Role role = RoleService.Instance.GetById(userRole.RoleId);

                RouteOrder routeOrder = RouteOrderService.Instance.GetById(id);

                if (routeOrder.ReviewStatus == 1 && !role.Name.Equals("超级管理员"))
                {
                    oNotice = new CNotice
                    {
                        Bresult = false,
                        Notice = "已审订单不能修改!",
                        Param1 = string.Empty,
                        Param2 = string.Empty,
                        Param3 = string.Empty
                    };
                    return Json(oNotice, JsonRequestBehavior.AllowGet);
                }

                routeOrder.RouteId = routeId;
                routeOrder.ChannelId = channelId;
                routeOrder.SupplierId = supplierId;
                routeOrder.BookTime = DateTime.Parse(bookTime);
                routeOrder.StartDate = DateTime.Parse(startDate);
                routeOrder.EndDate = DateTime.Parse(endDate);
                routeOrder.GuestName = guestName;
                routeOrder.GuestPhone = guestPhone;
                routeOrder.AdultNum = adultNum;
                routeOrder.AdultOriginalPrice = adultOriginalPrice;
                routeOrder.AdultSellPrice = adultSellPrice;
                routeOrder.ChildNum = childNum;
                routeOrder.ChildOriginalPrice = childOriginalPrice;
                routeOrder.ChildSellPrice = childSellPrice;
                routeOrder.ExtrasOriginPrice = extrasOriginPrice;
                routeOrder.ExtrasSellPrice = extrasSellPrice;
                routeOrder.ProxyIncome = string.IsNullOrEmpty(proxyIncome) ? 0 : double.Parse(proxyIncome);
                routeOrder.Description = description;
                routeOrder.Modified = DateTime.Now;
                //routeOrder.HandlerId = currentAdminUser.Id;
                routeOrder.ModifierId = currentAdminUser.Id;
                routeOrder.ReviewStatus = 0;
                routeOrder.BalanceStatus = 0;
                routeOrder.CollectMoneyStatus = 0;

                bRet = RouteOrderService.Instance.Update(routeOrder);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string strIds)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                strIds = strIds.TrimEnd(',');
                List<string> ids = strIds.Split(',').ToList();

                for (int i = 0; i < ids.Count; i++)
                {
                    int id = int.Parse(ids[i]);
                    
                    //判断是否审核,已审 则不能删除
                    RouteOrder routeOrder = RouteOrderService.Instance.GetById(id);
                    if (routeOrder.ReviewStatus == 1)
                        continue;

                    bRet = RouteOrderService.Instance.Delete(id);
                }

                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Review(int id, int reviewStatusId)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                RouteOrder routeOrder = RouteOrderService.Instance.GetById(id);

                routeOrder.ReviewStatus = reviewStatusId;
                routeOrder.Modified = DateTime.Now;
                routeOrder.ModifierId = currentAdminUser.Id;

                bRet = RouteOrderService.Instance.Update(routeOrder);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Balance(int id, int balanceStatusId, string balanceRemark)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                RouteOrder routeOrder = RouteOrderService.Instance.GetById(id);

                //增加条件限制
                if (routeOrder.ReviewStatus == 0)
                {
                    oNotice = new CNotice
                    {
                        Bresult = bRet,
                        Notice = NoticeResource.UN_REVIEWED,
                        Param1 = string.Empty,
                        Param2 = string.Empty,
                        Param3 = string.Empty
                    };
                    return Json(oNotice, JsonRequestBehavior.AllowGet);
                }

                routeOrder.BalanceStatus = balanceStatusId;
                routeOrder.BalanceRemark = balanceRemark;
                routeOrder.BalanceDate = DateTime.Now;
                routeOrder.Modified = DateTime.Now;
                routeOrder.ModifierId = currentAdminUser.Id;

                bRet = RouteOrderService.Instance.Update(routeOrder);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Collect(int id, int collectStatusId, string collectmoneyRemark)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                RouteOrder routeOrder = RouteOrderService.Instance.GetById(id);

                //增加条件限制
                if (routeOrder.ReviewStatus == 0)
                {
                    oNotice = new CNotice
                    {
                        Bresult = bRet,
                        Notice = NoticeResource.UN_REVIEWED,
                        Param1 = string.Empty,
                        Param2 = string.Empty,
                        Param3 = string.Empty
                    };
                    return Json(oNotice, JsonRequestBehavior.AllowGet);
                }

                routeOrder.CollectMoneyStatus = collectStatusId;
                routeOrder.CollectMoneyRemark = collectmoneyRemark;
                routeOrder.CollectMoneyDate = DateTime.Now;
                routeOrder.Modified = DateTime.Now;
                routeOrder.ModifierId = currentAdminUser.Id;

                bRet = RouteOrderService.Instance.Update(routeOrder);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

    }

    public class RouteOrderStatics
    {
        public int? TotalAdults { get; set; }
        public int? TotalChild { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalSell { get; set; }
        public decimal? TotalProxy { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalProfit { get; set; }
        public decimal? TotalCollect { get; set; }
    }

}