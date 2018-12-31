using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TourismCMS.EntityDataModel;
using TourismCMS.Services;
using TourismCMS.Models;
using TourismCMS.TranslateResources;

namespace TourismCMS.Services
{
    public class RouteOrderService
    {
        public static BaseRepository<RouteOrder> oBll = new BaseRepository<RouteOrder>();
        private static RouteOrderService _instance = null;

        public static RouteOrderService Instance
        {
            get
            {
                return _instance ?? new RouteOrderService(); ;
            }
        }

        private RouteOrderService()
        {

        }

        #region Public

        #region Query

        public List<RouteOrder> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                var query = context.RouteOrder.ToList();
                return query;
            }
        }

        public RouteOrder GetById(int id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.RouteOrder.Find(id);
                return query;
            }
        }
        public List<RouteOrder> GetBySomeWhere(User currentUser,string routeId,
            string channelId, string supplierId, string balacnestatusId, string collectstatusId,
            string flag, string sellerId, string settlement, string startdate, string enddate, string guestname, string guestphone, string keyword, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(currentUser,routeId, channelId, supplierId, balacnestatusId, collectstatusId, flag,sellerId,settlement,startdate,enddate,guestname, guestphone, keyword);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<RouteOrder>();
        }

        public List<RouteOrder> GetBySomeWhere(User currentUser, string routeId,
            string channelId, string supplierId, string balacnestatusId, string collectstatusId,
            string flag, string sellerId, string settlement, string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            var filtered = GetSearchResult(currentUser, routeId, channelId, supplierId, balacnestatusId, collectstatusId, flag, sellerId, settlement, startdate, enddate, guestname, guestphone, keyword);
            return filtered;
        }

        #endregion

        #region Create

        public bool Create(RouteOrder oItem)
        {
            var oRet = new RouteOrder();

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

        public bool Update(RouteOrder oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.RouteOrder.Attach(oIem);
                context.Entry<RouteOrder>(oIem).State = EntityState.Modified;
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
                    RouteOrder oItem = GetById(id);
                    db.RouteOrder.Attach(oItem);
                    db.Entry<RouteOrder>(oItem).State = EntityState.Deleted;
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

        private List<RouteOrder> GetSearchResult(User currentUser,string routeId,
            string channelId, string supplierId, string balacnestatusId, string collectstatusId, string flag, string sellerId, string settlement,
            string startdate, string enddate, string guestname, string guestphone, string keyword)
        {
            UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
            Role role = RoleService.Instance.GetById(userRole.RoleId);

            List<RouteOrder> list = new List<RouteOrder>();
            using (var context = new TourismDbContext())
            {
                string sql = @"select * from RouteOrder where 1=1";
                if (!string.IsNullOrEmpty(routeId) && routeId != "-1")
                    sql += " and RouteId=" + int.Parse(routeId);

                if (!string.IsNullOrEmpty(channelId) && channelId != "-1")
                    sql += " and ChannelId=" + int.Parse(channelId);

                if (!string.IsNullOrEmpty(supplierId) && supplierId != "-1")
                    sql += " and SupplierId=" + int.Parse(supplierId);

                if (!string.IsNullOrEmpty(balacnestatusId) && balacnestatusId != "-1")
                    sql += " and BalanceStatus=" + int.Parse(balacnestatusId);

                if (!string.IsNullOrEmpty(collectstatusId) && collectstatusId != "-1")
                    sql += " and CollectMoneyStatus=" + int.Parse(collectstatusId);

                //if (!string.IsNullOrEmpty(startdate))
                //    sql += " and StartDate=" + DateTime.Parse(startdate);

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

                list = context.Database.SqlQuery<RouteOrder>(sql).ToList();
            }
            return list;
        }

        #endregion
    }
}