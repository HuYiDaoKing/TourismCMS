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
    public class TicketOrderController : Controller
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd";

        // GET: TicketOrder
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
            string ticketId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string usedate, string guestname, string guestphone, string keyword)
        {
            var results = new Object();
            var ticketOrders = new List<TicketOrder>();
            try
            {
                int total = 0;
                int pageNum = 0;

                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                ticketOrders = TicketOrderService.Instance.GetBySomeWhere(currentUser, ticketId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, usedate, guestname, guestphone, keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                //统计
                /*int? totalTickets = 0;//总门票数
                decimal? totalCost = 0;//总成本
                decimal? totalSell = 0;//销售总额
                decimal? totalProxy = 0;//代收总额
                decimal? totalPay = 0;//应付总额
                decimal? totalProfit = 0;//总利润
                decimal? totalCollect = 0;//应收*/

                var newTicketOrders = new List<Object>();
                foreach (TicketOrder ticketOrder in ticketOrders)
                {
                    /*totalTickets += ticketOrder.TicketNum;
                    totalCost += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.OriginalPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse((ticketOrder.ProxyIncome).ToString()), 3);
                    totalPay += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.OriginalPrice - ticketOrder.ProxyIncome).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((ticketOrder.TicketNum * (ticketOrder.SellPrice - ticketOrder.OriginalPrice)).ToString()), 3);
                    totalCollect += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice - ticketOrder.ProxyIncome).ToString()), 3);*/

                    string seller = string.Empty;
                    string ticket = string.Empty;
                    string supplier = string.Empty;
                    string channel = string.Empty;

                    if (ticketOrder.HandlerId != null)
                        seller = UserService.Instance.GetById(ticketOrder.HandlerId).Name;

                    if (ticketOrder.TicketId != null)
                        ticket = TicketService.Instance.GetById(ticketOrder.TicketId).Name;

                    if (ticketOrder.SupplierId != null)
                        supplier = SupplierService.Instance.GetById(ticketOrder.SupplierId).Name;

                    if (ticketOrder.ChannelId != null)
                        channel = ChannelService.Instance.GetById(ticketOrder.ChannelId).Name;

                    newTicketOrders.Add(new
                    {
                        Id = ticketOrder.Id,
                        //UserId = ticketOrder.UserId,
                        //HotelId = hotelOrder.HotelId,
                        TicketId = ticketOrder.TicketId,
                        GuestName = ticketOrder.GuestName,
                        GuestPhone = ticketOrder.GuestPhone,
                        //RoomType = ticketOrder.RoomType,
                        //RoomNum = ticketOrder.RoomNum,
                        TicketType = ticketOrder.TicketType,
                        TicketNum = ticketOrder.TicketNum,
                        OriginalPrice = ticketOrder.OriginalPrice,
                        SellPrice = ticketOrder.SellPrice,
                        BookTime = null == ticketOrder.BookTime ? string.Empty : DateTime.Parse(ticketOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                        UseDate = null == ticketOrder.UseDate ? string.Empty : DateTime.Parse(ticketOrder.UseDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        //EndDate = null == hotelOrder.EndDate ? string.Empty : DateTime.Parse(hotelOrder.EndDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ProxyIncome = ticketOrder.ProxyIncome,
                        ChannelId = ticketOrder.ChannelId,
                        SupplierId = ticketOrder.SupplierId,
                        BalanceStatus = BalanceStatusManager.BalanceStatus[ticketOrder.BalanceStatus],
                        BalanceDate = null == ticketOrder.BalanceDate ? string.Empty : DateTime.Parse(ticketOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        BalanceRemark = ticketOrder.BalanceRemark,
                        CollectMoneyStatus = CollectMoneyStatusManager.CollectMoneyStatus[ticketOrder.CollectMoneyStatus],
                        CollectMoneyDate = null == ticketOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(ticketOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ReviewStatus = ReviewStatusManager.ReviewStatus[ticketOrder.ReviewStatus],
                        Description = ticketOrder.Description,
                        Created = DateTime.Parse(ticketOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(ticketOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                        Seller = seller,
                        Ticket = ticket,
                        Supplier = supplier,
                        Channel = channel,
                        CollectMoneyRemark = ticketOrder.CollectMoneyRemark,
                        Settlement = ticketOrder.Settlement,
                        ShouldPay = Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.OriginalPrice - ticketOrder.ProxyIncome).ToString()), 3),//应付
                        ShouldCollect = Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice - ticketOrder.ProxyIncome).ToString()), 3),//应收
                        Profit = Math.Round(Decimal.Parse((ticketOrder.TicketNum * (ticketOrder.SellPrice - ticketOrder.OriginalPrice)).ToString()), 3)//利润
                    });
                }

                //TicketOrderStatics statics = new TicketOrderStatics
                //{
                //    TotalTickets = totalTickets,
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
                    ticketOrders = newTicketOrders
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
            TicketOrder ticketOrder = new TicketOrder();
            var newHotelOrder = new Object();
            try
            {
                ticketOrder = TicketOrderService.Instance.GetById(id);

                if (ticketOrder == null)
                    return null;

                string seller = string.Empty;
                string ticket = string.Empty;
                string supplier = string.Empty;
                string channel = string.Empty;

                if (ticketOrder.HandlerId != null)
                    seller = UserService.Instance.GetById(ticketOrder.HandlerId).Name;

                if (ticketOrder.TicketId != null)
                    ticket = TicketService.Instance.GetById(ticketOrder.TicketId).Name;

                if (ticketOrder.SupplierId != null)
                    supplier = SupplierService.Instance.GetById(ticketOrder.SupplierId).Name;

                if (ticketOrder.ChannelId != null)
                    channel = ChannelService.Instance.GetById(ticketOrder.ChannelId).Name;

                newHotelOrder = new
                {
                    Id = ticketOrder.Id,
                    //UserId = ticketOrder.us,
                    //HotelId = hotelOrder.HotelId,
                    TicketId = ticketOrder.TicketId,
                    GuestName = ticketOrder.GuestName,
                    GuestPhone = ticketOrder.GuestPhone,
                    TicketType = ticketOrder.TicketType,
                    TicketNum = ticketOrder.TicketNum,
                    OriginalPrice = ticketOrder.OriginalPrice,
                    SellPrice = ticketOrder.SellPrice,
                    BookTime = null == ticketOrder.BookTime ? string.Empty : DateTime.Parse(ticketOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                    UseDate = null == ticketOrder.UseDate ? string.Empty : DateTime.Parse(ticketOrder.UseDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    ProxyIncome = ticketOrder.ProxyIncome,
                    ChannelId = ticketOrder.ChannelId,
                    SupplierId = ticketOrder.SupplierId,
                    BalanceStatus = BalanceStatusManager.BalanceStatus[ticketOrder.BalanceStatus],
                    BalanceStatusId = ticketOrder.BalanceStatus,
                    BalanceDate = null == ticketOrder.BalanceDate ? string.Empty : DateTime.Parse(ticketOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    BalanceRemark = ticketOrder.BalanceRemark,
                    CollectMoneyStatus = CollectMoneyStatusManager.CollectMoneyStatus[ticketOrder.CollectMoneyStatus],
                    CollectMoneyStatusId = ticketOrder.CollectMoneyStatus,
                    CollectMoneyDate = null == ticketOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(ticketOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                    ReviewStatus = ReviewStatusManager.ReviewStatus[ticketOrder.ReviewStatus],
                    ReviewStatusId = ticketOrder.ReviewStatus,
                    Description = ticketOrder.Description,
                    Created = DateTime.Parse(ticketOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                    Modified = DateTime.Parse(ticketOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                    Seller = seller,
                    Ticket = ticket,
                    Supplier = supplier,
                    Channel = channel,
                    CollectMoneyRemark = ticketOrder.CollectMoneyRemark,
                    Settlement = ticketOrder.Settlement,
                    ShouldPay = Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice).ToString()), 3),//应付
                    ShouldCollect = Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice - ticketOrder.ProxyIncome).ToString()), 3),//应收
                    Profit = Math.Round(Decimal.Parse((ticketOrder.TicketNum * (ticketOrder.SellPrice - ticketOrder.OriginalPrice)).ToString()), 3)//利润
                };
            }
            catch
            {

            }
            return Json(newHotelOrder, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatics(string ticketId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,string usedate, string guestname, string guestphone, string keyword)
        {
            //统计
            int? totalTickets = 0;//总门票数
            decimal? totalCost = 0;//总成本
            decimal? totalSell = 0;//销售总额
            decimal? totalProxy = 0;//代收总额
            decimal? totalPay = 0;//应付总额
            decimal? totalProfit = 0;//总利润
            decimal? totalCollect = 0;//应收
            try
            {
                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                List<TicketOrder> ticketOrders = TicketOrderService.Instance.GetBySomeWhere(currentUser, ticketId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, usedate, guestname, guestphone, keyword);

                foreach (TicketOrder ticketOrder in ticketOrders)
                {
                    totalTickets += ticketOrder.TicketNum;
                    totalCost += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.OriginalPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse((ticketOrder.ProxyIncome).ToString()), 3);
                    totalPay += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.OriginalPrice - ticketOrder.ProxyIncome).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((ticketOrder.TicketNum * (ticketOrder.SellPrice - ticketOrder.OriginalPrice)).ToString()), 3);
                    totalCollect += Math.Round(Decimal.Parse((ticketOrder.TicketNum * ticketOrder.SellPrice - ticketOrder.ProxyIncome).ToString()), 3);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            TicketOrderStatics statics = new TicketOrderStatics
            {
                TotalTickets = totalTickets,
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
                int ticketId,
                int channelId,
                int supplierId,
                string bookTime,
                string useDate,
                string guestName,
                string guestPhone,
                string ticketType,
                int ticketNum,
                double originalPrice,
                double sellPrice,
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

                if (currentAdminUser == null)
                {
                    oNotice = new CNotice
                    {
                        Bresult = false,
                        Notice = "登录超时!",
                        Param1 = string.Empty,
                        Param2 = string.Empty,
                        Param3 = string.Empty
                    };
                    return Json(oNotice, JsonRequestBehavior.AllowGet);
                }

                TicketOrder ticketOrder = new TicketOrder
                {
                    //UserId = currentAdminUser.Id,
                    //HotelId = hotelId,
                    TicketId = ticketId,
                    GuestName = guestName,
                    GuestPhone = guestPhone,
                    TicketType = ticketType,
                    TicketNum = ticketNum,
                    OriginalPrice = originalPrice,
                    SellPrice = sellPrice,
                    //BookTime=DateTime.Parse(bo),
                    //StartDate = DateTime.Parse(startDate),
                    //EndDate = DateTime.Parse(endDate),
                    BookTime = DateTime.Parse(useDate),
                    UseDate = DateTime.Parse(useDate),
                    ProxyIncome = string.IsNullOrEmpty(proxyIncome) ? 0 : double.Parse(proxyIncome),
                    ChannelId = channelId,
                    SupplierId = supplierId,
                    ReviewStatus = 0,//创建时默认设置为待评审
                    CollectMoneyStatus = 0,
                    BalanceStatus = 0,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Description = description,
                    HandlerId = currentAdminUser.Id,
                    ModifierId = currentAdminUser.Id,
                    CheckerId = currentAdminUser.Id,
                    Settlement = (ticketNum * originalPrice - double.Parse(proxyIncome)) > 0 ? "挂账" : "返佣"
                };

                //bRet = HotelOrderService.Instance.Create(hotelOrder);
                bRet = TicketOrderService.Instance.Create(ticketOrder);
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
            //int hotelId,
            int ticketId,
                int channelId,
                int supplierId,
                string bookTime,
                string useDate,
            //string endDate,
                string guestName,
                string guestPhone,
                string ticketType,
                int ticketNum,
                double originalPrice,
                double sellPrice,
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

                TicketOrder ticketOrder = TicketOrderService.Instance.GetById(id);
                if (ticketOrder.ReviewStatus == 1 && !role.Name.Equals("超级管理员"))
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

                ticketOrder.TicketId = ticketId;
                ticketOrder.ChannelId = channelId;
                ticketOrder.SupplierId = supplierId;
                ticketOrder.BookTime = DateTime.Parse(bookTime);
                ticketOrder.UseDate = DateTime.Parse(useDate);
                ticketOrder.GuestName = guestName;
                ticketOrder.GuestPhone = guestPhone;
                ticketOrder.TicketType = ticketType;
                ticketOrder.TicketNum = ticketNum;
                ticketOrder.OriginalPrice = originalPrice;
                ticketOrder.SellPrice = sellPrice;
                ticketOrder.ProxyIncome = double.Parse(proxyIncome);
                ticketOrder.Description = description;
                ticketOrder.Modified = DateTime.Now;
                //ticketOrder.HandlerId = currentAdminUser.Id;
                ticketOrder.ModifierId = currentAdminUser.Id;
                ticketOrder.ReviewStatus = 0;
                ticketOrder.BalanceStatus = 0;
                ticketOrder.CollectMoneyStatus = 0;
                ticketOrder.Settlement = (ticketNum * originalPrice - double.Parse(proxyIncome)) > 0 ? "挂账" : "返佣";

                bRet = TicketOrderService.Instance.Update(ticketOrder);
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
                    int id = int.Parse(ids[0]);

                    //判断是否审核,已审 则不能删除
                    TicketOrder ticketOrder = TicketOrderService.Instance.GetById(id);
                    if (ticketOrder.ReviewStatus == 1)
                        continue;

                    bRet = TicketOrderService.Instance.Delete(id);
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

                TicketOrder ticketOrder = TicketOrderService.Instance.GetById(id);

                ticketOrder.ReviewStatus = reviewStatusId;
                ticketOrder.Modified = DateTime.Now;
                ticketOrder.ModifierId = currentAdminUser.Id;

                bRet = TicketOrderService.Instance.Update(ticketOrder);
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

                TicketOrder ticketOrder = TicketOrderService.Instance.GetById(id);

                //增加条件限制
                if (ticketOrder.ReviewStatus == 0)
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

                ticketOrder.BalanceStatus = balanceStatusId;
                ticketOrder.BalanceRemark = balanceRemark;
                ticketOrder.BalanceDate = DateTime.Now;
                ticketOrder.Modified = DateTime.Now;
                ticketOrder.ModifierId = currentAdminUser.Id;

                bRet = TicketOrderService.Instance.Update(ticketOrder);
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

                TicketOrder ticketOrder = TicketOrderService.Instance.GetById(id);

                //增加条件限制
                if (ticketOrder.ReviewStatus == 0)
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

                ticketOrder.CollectMoneyStatus = collectStatusId;
                ticketOrder.CollectMoneyRemark = collectmoneyRemark;
                ticketOrder.CollectMoneyDate = DateTime.Now;
                ticketOrder.Modified = DateTime.Now;
                ticketOrder.ModifierId = currentAdminUser.Id;

                bRet = TicketOrderService.Instance.Update(ticketOrder);
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

    public class TicketOrderStatics
    {
        public int? TotalTickets { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalSell { get; set; }
        public decimal? TotalProxy { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalProfit { get; set; }
        public decimal? TotalCollect { get; set; }
    }

}