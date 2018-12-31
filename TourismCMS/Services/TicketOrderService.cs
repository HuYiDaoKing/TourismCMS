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
    public class TicketOrderService
    {
        public static BaseRepository<TicketOrder> oBll = new BaseRepository<TicketOrder>();
        private static TicketOrderService _instance = null;

        public static TicketOrderService Instance
        {
            get
            {
                return _instance ?? new TicketOrderService(); ;
            }
        }

        private TicketOrderService()
        {

        }

        #region Public

        #region Query

        public List<TicketOrder> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                return context.TicketOrder.ToList();
            }
        }

        public TicketOrder GetById(int id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.TicketOrder.Find(id);
                return query;
            }
        }

        public List<TicketOrder> GetBySomeWhere(User currentUser, string ticketId, string channelId, string supplierId, string balacnestatusId, string collectstatusId,string sellerId ,string settlement,
            string usedate, string guestname, string guestphone, string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(currentUser,ticketId,channelId, supplierId, balacnestatusId, collectstatusId,sellerId,settlement, usedate, guestname, guestphone, strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<TicketOrder>();
        }

        public List<TicketOrder> GetBySomeWhere(User currentUser, string ticketId, string channelId, string supplierId, string balacnestatusId, string collectstatusId, string sellerId, string settlement,
            string usedate, string guestname, string guestphone, string strKeyWord)
        {
            var filtered = GetSearchResult(currentUser, ticketId, channelId, supplierId, balacnestatusId, collectstatusId, sellerId, settlement, usedate, guestname, guestphone, strKeyWord);

            return filtered;
        }

        #endregion

        #region Create

        public bool Create(TicketOrder oItem)
        {
            var oRet = new TicketOrder();

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

        public bool Update(TicketOrder oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.TicketOrder.Attach(oIem);
                context.Entry<TicketOrder>(oIem).State = EntityState.Modified;
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
                    TicketOrder oItem = GetById(id);
                    db.TicketOrder.Attach(oItem);
                    db.Entry<TicketOrder>(oItem).State = EntityState.Deleted;
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
        private List<TicketOrder> GetSearchResult(User currentUser, string ticketId, string channelId, string supplierId, string balacnestatusId, string collectstatusId,string sellerId,string settlement,
            string usedate, string guestname, string guestphone, string keyword)
        {
            UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
            Role role = RoleService.Instance.GetById(userRole.RoleId);
            List<TicketOrder> list = new List<TicketOrder>();
            using (var context = new TourismDbContext())
            {
                string sql = @"select * from TicketOrder where 1=1";

                if (!string.IsNullOrEmpty(ticketId) && ticketId != "-1")
                    sql += " and TicketId=" + int.Parse(ticketId);

                if (!string.IsNullOrEmpty(channelId) && channelId != "-1")
                    sql += " and ChannelId=" + int.Parse(channelId);

                if (!string.IsNullOrEmpty(supplierId) && supplierId != "-1")
                    sql += " and SupplierId=" + int.Parse(supplierId);

                if (!string.IsNullOrEmpty(balacnestatusId) && balacnestatusId != "-1")
                    sql += " and BalanceStatus=" + int.Parse(balacnestatusId);

                if (!string.IsNullOrEmpty(collectstatusId) && collectstatusId != "-1")
                    sql += " and CollectMoneyStatus=" + int.Parse(collectstatusId);

                if (!string.IsNullOrEmpty(usedate))
                    sql += " and UseDate=" + DateTime.Parse(usedate);

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

                sql += " order by Created desc";

                list = context.Database.SqlQuery<TicketOrder>(sql).ToList();
            }
            return list;
        }

        #endregion
    }
}