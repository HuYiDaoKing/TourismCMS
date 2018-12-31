using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TourismCMS.EntityDataModel;
using TourismCMS.Services;
using TourismCMS.Models;
using TourismCMS.TranslateResources;
using System.Linq.Expressions;

namespace TourismCMS.Services
{
    public class HotelOrderService
    {
        public static BaseRepository<HotelOrder> oBll = new BaseRepository<HotelOrder>();
        private static HotelOrderService _instance = null;

        public static HotelOrderService Instance
        {
            get
            {
                return _instance ?? new HotelOrderService(); ;
            }
        }

        private HotelOrderService()
        {

        }

        #region Public

        #region Query

        public HotelOrder GetById(int id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.HotelOrder.Find(id);
                return query;
            }
        }

        public List<HotelOrder> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                return context.HotelOrder.ToList();
            }
        }

        public List<HotelOrder> GetBySomeWhere(User currentUser,string flag, string hotelId, string channelId, string supplierId, string balacnestatusId, string collectstatusId,string sellerId,string settlement,
            string startdate,string enddate, string guestname, string guestphone, string keyword, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(currentUser, hotelId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId,settlement,startdate,enddate, guestname, guestphone, keyword,flag);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<HotelOrder>();
        }

        public List<HotelOrder> GetBySomeWhere(User currentUser, string flag, string hotelId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            var filtered = GetSearchResult(currentUser, hotelId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword, flag);
            return filtered;
        }

        #endregion

        #region Create

        public bool Create(HotelOrder oItem)
        {
            var oRet = new HotelOrder();

            try
            {
                oRet = oBll.AddEntities(oItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var bRet = oRet == null ? false : true;
            return bRet;
        }

        #endregion

        #region Update

        public bool Update(HotelOrder oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.HotelOrder.Attach(oIem);
                context.Entry<HotelOrder>(oIem).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// 注意在调用的时候还要删除对应的物理文件
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool bRet = false;

            try
            {
                using (var db = new TourismDbContext())
                {
                    HotelOrder oItem = GetById(id);
                    db.HotelOrder.Attach(oItem);
                    db.Entry<HotelOrder>(oItem).State = EntityState.Deleted;
                    bRet = db.SaveChanges() > 0;
                }
            }
            catch
            {
                bRet = false;
            }

            return bRet;
        }

        #endregion

        #endregion

        #region Private

        private List<HotelOrder> GetSearchResult(User currentUser, string hotelId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword, string flag)
        {
            UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
            Role role = RoleService.Instance.GetById(userRole.RoleId);
            List<HotelOrder> list = new List<HotelOrder>();
            using (var context = new TourismDbContext())
            {
                string sql = @"select * from HotelOrder where 1=1";
                if (!string.IsNullOrEmpty(hotelId) && hotelId != "-1")
                    sql += " and HotelId=" + int.Parse(hotelId);

                if (!string.IsNullOrEmpty(channelId) && channelId != "-1")
                    sql += " and ChannelId=" + int.Parse(channelId);

                if (!string.IsNullOrEmpty(supplierId) && supplierId != "-1")
                    sql += " and SupplierId=" + int.Parse(supplierId);

                if (!string.IsNullOrEmpty(balacnestatusId) && balacnestatusId != "-1")
                    sql += " and BalanceStatus=" + int.Parse(balacnestatusId);

                if (!string.IsNullOrEmpty(collectstatusId) && collectstatusId != "-1")
                    sql += " and CollectMoneyStatus=" + int.Parse(collectstatusId);

                if (!string.IsNullOrEmpty(guestname))
                    //sql += " and GuestName='" + guestname + "'";
                    sql += " and GuestName like '%" + guestname + "%'";

                if (!string.IsNullOrEmpty(guestphone))
                    //sql += " and GuestPhone='" + guestphone + "'";
                    sql += " and GuestPhone like '%" + guestphone + "%'";

                if (currentUser != null && !role.Name.Equals("超级管理员"))
                    sql += " and HandlerId='" + currentUser.Id + "'";

                //按照销售员查询
                if (!string.IsNullOrEmpty(sellerId) && sellerId != "-1")
                    sql += " and HandlerId='" + sellerId + "'";

                if (!string.IsNullOrEmpty(settlement) && !string.Equals(settlement, "全部"))
                    sql += " and Settlement='" + settlement + "'";

                if (!string.IsNullOrEmpty(flag))
                {
                    switch (flag)
                    {
                        case "入住时间":
                            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                                sql += " and StartDate>='" + DateTime.Parse(startdate) + "' and StartDate<='" + DateTime.Parse(enddate) + "'";
                            break;
                        case "预定时间":
                            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                                sql += " and BookTime>='" + DateTime.Parse(startdate) + "' and BookTime<='" + DateTime.Parse(enddate) + "'";
                            break;
                        case "结算时间":
                            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                                sql += " and BalanceDate>='" + DateTime.Parse(startdate) + "' and BalanceDate<='" + DateTime.Parse(enddate) + "'";
                            break;
                        case "收款时间":
                            if (!string.IsNullOrEmpty(startdate) && !string.IsNullOrEmpty(enddate))
                                sql += " and CollectMoneyDate>='" + DateTime.Parse(startdate) + "' and CollectMoneyDate<='" + DateTime.Parse(enddate) + "'";
                            break;
                    }
                }

                sql += " order by Created desc";

                list = context.Database.SqlQuery<HotelOrder>(sql).ToList();
            }
            return list;
        }

        #endregion
    }
}