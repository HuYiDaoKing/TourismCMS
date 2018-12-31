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
    public class TicketService
    {
        public static BaseRepository<Ticket> oBll = new BaseRepository<Ticket>();
        private static TicketService _instance = null;

        public static TicketService Instance
        {
            get
            {
                return _instance ?? new TicketService(); ;
            }
        }

        private TicketService()
        {

        }

        #region Public

        #region Query

        public Ticket GetByName(string strName)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Ticket.FirstOrDefault(c=>c.Name==strName);
                return query;
            }
        }

        public Ticket GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Ticket.Find(id);
                return query;
            }
        }

        public List<Ticket> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Ticket>();
        }

        public List<Ticket> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Ticket.ToList()
                            select c;

                return query
               .OrderByDescending(m => m.Modified)
               .ThenByDescending(m => m.Id)
               .ToList<Ticket>();
            }
        }

        #endregion

        #region Create

        public bool Create(Ticket oItem)
        {
            var oRet = new Ticket();

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

        public bool Update(Ticket oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Ticket.Attach(oIem);
                context.Entry<Ticket>(oIem).State = EntityState.Modified;
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
                    Ticket oItem = GetById(id);
                    db.Ticket.Attach(oItem);
                    db.Entry<Ticket>(oItem).State = EntityState.Deleted;
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

        private List<Ticket> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Ticket.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Ticket>();
            }
        }

        #endregion
    }
}