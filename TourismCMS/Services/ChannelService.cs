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
    public class ChannelService
    {
        public static BaseRepository<Channel> oBll = new BaseRepository<Channel>();
        private static ChannelService _instance = null;

        public static ChannelService Instance
        {
            get
            {
                return _instance ?? new ChannelService(); ;
            }
        }

        private ChannelService()
        {

        }

        #region Public

        #region Query

        public Channel GetByName(string strName,string type)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Channel.FirstOrDefault(c => c.Type == type && c.Name == strName);
                return query;
            }
        }

        public Channel GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Channel.Find(id);
                return query;
            }
        }

        public List<Channel> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Channel>();
        }

        public List<Channel> GetAll(string type)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Channel.ToList()
                            select c;

                return query
               .Where(m=>string.IsNullOrEmpty(type) ||m.Type==type)
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Channel>();
            }
        }

        #endregion

        #region Create

        public bool Create(Channel oItem)
        {
            var oRet = new Channel();

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

        public bool Update(Channel oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Channel.Attach(oIem);
                context.Entry<Channel>(oIem).State = EntityState.Modified;
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
                    Channel oItem = GetById(id);
                    db.Channel.Attach(oItem);
                    db.Entry<Channel>(oItem).State = EntityState.Deleted;
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

        private List<Channel> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Channel.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Modified)
               //.ThenByDescending(m => m.Id)
               .ToList<Channel>();
            }
        }

        #endregion
    }
}