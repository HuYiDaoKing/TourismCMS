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
using TourismCMS.Filters;

namespace TourismCMS.Controllers
{
    public class HotelOrderController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd";

        // GET: HotelOrder
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

        public ActionResult GetTableRecords(int start, int limit, string flag, string hotelId,
            string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            var results = new Object();
            var hotelOrders = new List<HotelOrder>();
            try
            {
                int total = 0;
                int pageNum = 0;

                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                hotelOrders = HotelOrderService.Instance.GetBySomeWhere(currentUser, flag, hotelId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                //统计
                /*int? totalDays = 0;//总夜间数
                decimal? totalCost = 0;//总成本
                decimal? totalSell = 0;//销售总额
                decimal? totalProxy = 0;//代收总额
                decimal? totalPay = 0;//应付总额
                decimal? totalProfit = 0;//总利润
                decimal? totalCollect = 0;//应收*/

                var newHotelOrders = new List<Object>();
                foreach (HotelOrder hotelOrder in hotelOrders)
                {
                    //求日期差
                    int days = GetDays(DateTime.Parse(hotelOrder.StartDate.ToString()).Date, DateTime.Parse(hotelOrder.EndDate.ToString()).Date);
                    /*totalDays += days * hotelOrder.RoomNum;
                    totalCost += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse((hotelOrder.ProxyIncome).ToString()), 3);
                    totalPay += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice - hotelOrder.ProxyIncome).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * (hotelOrder.SellPrice - hotelOrder.OriginalPrice)).ToString()), 3);
                    totalCollect += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice - hotelOrder.ProxyIncome).ToString()), 3);*/

                    string seller = string.Empty;
                    string hotel = string.Empty;
                    string supplier = string.Empty;
                    string channel = string.Empty;

                    if (hotelOrder.HandlerId != null)
                        seller = UserService.Instance.GetById(hotelOrder.HandlerId).Name;

                    if (hotelOrder.HotelId != null)
                        hotel = HotelService.Instance.GetById(hotelOrder.HotelId).Name;

                    if (hotelOrder.SupplierId != null)
                        supplier = SupplierService.Instance.GetById(hotelOrder.SupplierId).Name;

                    if (hotelOrder.ChannelId != null)
                        channel = ChannelService.Instance.GetById(hotelOrder.ChannelId).Name;

                    newHotelOrders.Add(new
                    {
                        Id = hotelOrder.Id,
                        UserId = hotelOrder.UserId,
                        HotelId = hotelOrder.HotelId,
                        GuestName = hotelOrder.GuestName,
                        GuestPhone = hotelOrder.GuestPhone,
                        RoomType = hotelOrder.RoomType,
                        RoomNum = hotelOrder.RoomNum,
                        OriginalPrice = hotelOrder.OriginalPrice,
                        SellPrice = hotelOrder.SellPrice,
                        BookTime = null == hotelOrder.BookTime ? string.Empty : DateTime.Parse(hotelOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                        StartDate = null == hotelOrder.StartDate ? string.Empty : DateTime.Parse(hotelOrder.StartDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        EndDate = null == hotelOrder.EndDate ? string.Empty : DateTime.Parse(hotelOrder.EndDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ProxyIncome = hotelOrder.ProxyIncome,
                        ChannelId = hotelOrder.ChannelId,
                        SupplierId = hotelOrder.SupplierId,
                        BalanceStatus = BalanceStatusManager.BalanceStatus[hotelOrder.BalanceStatus],
                        BalanceDate = null == hotelOrder.BalanceDate ? string.Empty : DateTime.Parse(hotelOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        BalanceRemark = hotelOrder.BalanceRemark,
                        CollectMoneyStatus = CollectMoneyStatusManager.CollectMoneyStatus[hotelOrder.CollectMoneyStatus],
                        CollectMoneyDate = null == hotelOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(hotelOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ReviewStatus = ReviewStatusManager.ReviewStatus[hotelOrder.ReviewStatus],
                        Description = hotelOrder.Description,
                        Created = DateTime.Parse(hotelOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(hotelOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                        Seller = seller,
                        Hotel = hotel,
                        Supplier = supplier,
                        Channel = channel,
                        CollectMoneyRemark = hotelOrder.CollectMoneyRemark,
                        Settlement = hotelOrder.Settlement,
                        ModifierId = hotelOrder.ModifierId,
                        ShouldPay = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice - hotelOrder.ProxyIncome).ToString()), 3),//应付
                        ShouldCollect = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice - hotelOrder.ProxyIncome).ToString()), 3),//应收
                        Profit = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * (hotelOrder.SellPrice - hotelOrder.OriginalPrice)).ToString()), 3)//利润
                    });
                }

                //HotelOrderStatics statics = new HotelOrderStatics
                //{
                //    TotalDays = totalDays,
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
                    hotelOrders = newHotelOrders
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
            HotelOrder hotelOrder = new HotelOrder();
            var newHotelOrder = new Object();
            try
            {
                hotelOrder = HotelOrderService.Instance.GetById(id);

                if (hotelOrder == null)
                    return null;

                string seller = string.Empty;
                string hotel = string.Empty;
                string supplier = string.Empty;
                string channel = string.Empty;

                if (hotelOrder.HandlerId != null)
                    seller = UserService.Instance.GetById(hotelOrder.HandlerId).Name;

                if (hotelOrder.HotelId != null)
                    hotel = HotelService.Instance.GetById(hotelOrder.HotelId).Name;

                if (hotelOrder.SupplierId != null)
                    supplier = SupplierService.Instance.GetById(hotelOrder.SupplierId).Name;

                if (hotelOrder.ChannelId != null)
                    channel = ChannelService.Instance.GetById(hotelOrder.ChannelId).Name;

                int days = DateTime.Parse(hotelOrder.EndDate.ToString()).Subtract(DateTime.Parse(hotelOrder.StartDate.ToString())).Days;

                newHotelOrder = new
                    {
                        Id = hotelOrder.Id,
                        UserId = hotelOrder.UserId,
                        HotelId = hotelOrder.HotelId,
                        GuestName = hotelOrder.GuestName,
                        GuestPhone = hotelOrder.GuestPhone,
                        RoomType = hotelOrder.RoomType,
                        RoomNum = hotelOrder.RoomNum,
                        OriginalPrice = hotelOrder.OriginalPrice,
                        SellPrice = hotelOrder.SellPrice,
                        BookTime = null == hotelOrder.BookTime ? string.Empty : DateTime.Parse(hotelOrder.BookTime.ToString()).Date.ToString(DATETIME_FORMAT),
                        StartDate = null == hotelOrder.StartDate ? string.Empty : DateTime.Parse(hotelOrder.StartDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        EndDate = null == hotelOrder.EndDate ? string.Empty : DateTime.Parse(hotelOrder.EndDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ProxyIncome = Math.Round(Decimal.Parse((hotelOrder.ProxyIncome).ToString()), 3),
                        ChannelId = hotelOrder.ChannelId,
                        SupplierId = hotelOrder.SupplierId,
                        BalanceStatus = hotelOrder.BalanceStatus,
                        BalanceDate = null == hotelOrder.BalanceDate ? string.Empty : DateTime.Parse(hotelOrder.BalanceDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        BalanceRemark = hotelOrder.BalanceRemark,
                        CollectMoneyStatus = hotelOrder.CollectMoneyStatus,
                        CollectMoneyDate = null == hotelOrder.CollectMoneyDate ? string.Empty : DateTime.Parse(hotelOrder.CollectMoneyDate.ToString()).Date.ToString(DATETIME_FORMAT),
                        ReviewStatus = hotelOrder.ReviewStatus,
                        Description = hotelOrder.Description,
                        Created = DateTime.Parse(hotelOrder.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(hotelOrder.Modified.ToString()).ToString(DATETIME_FORMAT),
                        Seller = seller,
                        Hotel = hotel,
                        Supplier = supplier,
                        Channel = channel,
                        Settlement = hotelOrder.Settlement,//结算类型
                        ModifierId = hotelOrder.ModifierId,
                        ShouldPay = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice - hotelOrder.ProxyIncome).ToString()), 3),//应付
                        ShouldCollect = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice - hotelOrder.ProxyIncome).ToString()), 3),//应收
                        Profit = Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * (hotelOrder.SellPrice - hotelOrder.OriginalPrice)).ToString()), 3)//利润
                    };
            }
            catch
            {

            }
            return Json(newHotelOrder, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatics(string flag, string hotelId,
            string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            //统计
            int? totalDays = 0;//总夜间数
            decimal? totalCost = 0;//总成本
            decimal? totalSell = 0;//销售总额
            decimal? totalProxy = 0;//代收总额
            decimal? totalPay = 0;//应付总额
            decimal? totalProfit = 0;//总利润
            decimal? totalCollect = 0;//应收
            try
            {
                User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                //List<HotelOrder> hotelOrders = HotelOrderService.Instance.GetBySomeWhere(currentUser, flag, hotelId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword, start, limit, out total);
                List<HotelOrder> hotelOrders = HotelOrderService.Instance.GetBySomeWhere(currentUser, flag, hotelId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword);

                foreach (HotelOrder hotelOrder in hotelOrders)
                {
                    //求日期差
                    int days = GetDays(DateTime.Parse(hotelOrder.StartDate.ToString()).Date, DateTime.Parse(hotelOrder.EndDate.ToString()).Date);
                    totalDays += days * hotelOrder.RoomNum;
                    totalCost += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice).ToString()), 3);
                    totalSell += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice).ToString()), 3);
                    totalProxy += Math.Round(Decimal.Parse((hotelOrder.ProxyIncome).ToString()), 3);
                    totalPay += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.OriginalPrice - hotelOrder.ProxyIncome).ToString()), 3);
                    totalProfit += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * (hotelOrder.SellPrice - hotelOrder.OriginalPrice)).ToString()), 3);
                    totalCollect += Math.Round(Decimal.Parse((days * hotelOrder.RoomNum * hotelOrder.SellPrice - hotelOrder.ProxyIncome).ToString()), 3);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            HotelOrderStatics statics = new HotelOrderStatics
            {
                TotalDays = totalDays,
                TotalCost = totalCost,
                TotalSell = totalSell,
                TotalProxy = totalProxy,
                TotalPay = totalPay,
                TotalProfit = totalProfit,
                TotalCollect = totalCollect
            };

            return Json(statics,JsonRequestBehavior.AllowGet);
        }

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Insert(
                int hotelId,
                int channelId,
                int supplierId,
                string bookTime,
                string startDate,
                string endDate,
                string guestName,
                string guestPhone,
                string roomType,
                int roomNum,
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

                int days = DateTime.Parse(endDate.ToString()).Subtract(DateTime.Parse(startDate.ToString())).Days;//间夜数

                HotelOrder hotelOrder = new HotelOrder
                {
                    UserId = currentAdminUser.Id,
                    HotelId = hotelId,
                    GuestName = guestName,
                    GuestPhone = guestPhone,
                    RoomType = roomType,
                    RoomNum = roomNum,
                    OriginalPrice = originalPrice,
                    SellPrice = sellPrice,
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
                    Description = description,
                    HandlerId = currentAdminUser.Id,
                    ModifierId = currentAdminUser.Id,
                    Settlement = (days * roomNum * originalPrice - double.Parse(proxyIncome)) > 0 ? "挂账" : "返佣"
                };

                bRet = HotelOrderService.Instance.Create(hotelOrder);
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

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Update(
            int id,
            int hotelId,
                int channelId,
                int supplierId,
                string bookTime,
                string startDate,
                string endDate,
                string guestName,
                string guestPhone,
                string roomType,
                int roomNum,
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

                int days = DateTime.Parse(endDate.ToString()).Subtract(DateTime.Parse(startDate.ToString())).Days;//间夜数

                HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);

                if (hotelOrder.ReviewStatus == 1 && !role.Name.Equals("超级管理员"))
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

                hotelOrder.HotelId = hotelId;
                hotelOrder.ChannelId = channelId;
                hotelOrder.SupplierId = supplierId;
                hotelOrder.BookTime = DateTime.Parse(bookTime);
                hotelOrder.StartDate = DateTime.Parse(startDate);
                hotelOrder.EndDate = DateTime.Parse(endDate);
                hotelOrder.GuestName = guestName;
                hotelOrder.GuestPhone = guestPhone;
                hotelOrder.RoomType = roomType;
                hotelOrder.RoomNum = roomNum;
                hotelOrder.OriginalPrice = originalPrice;
                hotelOrder.SellPrice = sellPrice;
                hotelOrder.ProxyIncome = double.Parse(proxyIncome);
                hotelOrder.Description = description;
                hotelOrder.Modified = DateTime.Now;
                //hotelOrder.HandlerId = currentAdminUser.Id;
                hotelOrder.ModifierId = currentAdminUser.Id;
                hotelOrder.ReviewStatus = 0;
                hotelOrder.BalanceStatus = 0;
                hotelOrder.CollectMoneyStatus = 0;
                hotelOrder.Settlement = (days * roomNum * originalPrice - double.Parse(proxyIncome)) > 0 ? "挂账" : "返佣";

                bRet = HotelOrderService.Instance.Update(hotelOrder);
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

        [AdminUserAuthentication]
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
                    HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);
                    if (hotelOrder.ReviewStatus == 1)
                        continue;
                    bRet = HotelOrderService.Instance.Delete(id);
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

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Review(int id, int reviewStatusId)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);

                hotelOrder.ReviewStatus = reviewStatusId;
                hotelOrder.Modified = DateTime.Now;
                hotelOrder.CheckerId = currentAdminUser.Id;

                bRet = HotelOrderService.Instance.Update(hotelOrder);
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

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult BatchReview(string strIds, int reviewStatusId)
        {
            CNotice oNotice = new CNotice();
            string strNotice = string.Empty;
            var bRet = false;
            try
            {
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;
                strIds = strIds.TrimEnd(',');
                List<string> ids = strIds.Split(',').ToList();

                for (int i = 0; i < ids.Count; i++)
                {
                    int id = int.Parse(ids[i]);
                    HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);

                    hotelOrder.ReviewStatus = reviewStatusId;
                    hotelOrder.Modified = DateTime.Now;
                    hotelOrder.CheckerId = currentAdminUser.Id;
                    HotelOrderService.Instance.Update(hotelOrder);
                }
                bRet = true;
            }
            catch (Exception)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
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

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Balance(int id, int balanceStatusId, string balanceRemark)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);

                //增加条件限制
                if (hotelOrder.ReviewStatus == 0)
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

                hotelOrder.BalanceStatus = balanceStatusId;
                hotelOrder.BalanceRemark = balanceRemark;
                hotelOrder.BalanceDate = DateTime.Now;
                hotelOrder.Modified = DateTime.Now;
                hotelOrder.CheckerId = currentAdminUser.Id;

                bRet = HotelOrderService.Instance.Update(hotelOrder);
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

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Collect(int id, int collectStatusId, string collectmoneyRemark)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;
                var currentAdminUser = Session[AccountHashKeys.CurrentAdminUser] as User;

                HotelOrder hotelOrder = HotelOrderService.Instance.GetById(id);

                //增加条件限制
                if (hotelOrder.ReviewStatus == 0)
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

                hotelOrder.CollectMoneyStatus = collectStatusId;
                hotelOrder.CollectMoneyRemark = collectmoneyRemark;
                hotelOrder.CollectMoneyDate = DateTime.Now;
                hotelOrder.Modified = DateTime.Now;
                hotelOrder.CheckerId = currentAdminUser.Id;

                bRet = HotelOrderService.Instance.Update(hotelOrder);
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


        //Private
        //private bool isExistRoute(string strName)
        //{
        //    bool bRet = false;
        //    try
        //    {
        //        var item = RouteService.Instance.GetByName(strName);
        //        if (item != null)
        //            bRet = true;
        //    }
        //    catch (Exception)
        //    {
        //        bRet = false;
        //    }
        //    return bRet;
        //}

        //获取日期天数
        public int GetDays(DateTime dtBegin, DateTime dtEnd)
        {
            TimeSpan ts = dtEnd.Subtract(dtBegin);//TimeSpan得到dt1和dt2的时间间隔
            int countday = ts.Days;//获取两个日期间的总天数
            return countday;
        }
    }

    public class HotelOrderStatics
    {
        public int? TotalDays { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalSell { get; set; }
        public decimal? TotalProxy { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalProfit { get; set; }
        public decimal? TotalCollect { get; set; }
    }

}