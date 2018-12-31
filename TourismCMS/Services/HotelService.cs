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
    public class HotelService
    {
        public static BaseRepository<Hotel> oBll = new BaseRepository<Hotel>();
        private static HotelService _instance = null;

        public static HotelService Instance
        {
            get
            {
                return _instance ?? new HotelService(); ;
            }
        }

        private HotelService()
        {

        }

        #region Public

        #region Query

        public Hotel GetByName(string strName)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Hotel.FirstOrDefault(c=>c.Name==strName);
                return query;
            }
        }

        public Hotel GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Hotel.Find(id);
                return query;
            }
        }

        public List<Hotel> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Hotel>();
        }

        public List<Hotel> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Hotel.ToList()
                            select c;

                return query
               .OrderByDescending(m => m.Modified)
               //.ThenByDescending(m => m.Id)
               .ToList<Hotel>();
            }
        }

        #endregion

        #region Create

        public bool Create(Hotel oItem)
        {
            var oRet = new Hotel();

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

        public bool Update(Hotel oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Hotel.Attach(oIem);
                context.Entry<Hotel>(oIem).State = EntityState.Modified;
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
                    Hotel oItem = GetById(id);
                    db.Hotel.Attach(oItem);
                    db.Entry<Hotel>(oItem).State = EntityState.Deleted;
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

        private List<Hotel> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Hotel.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Hotel>();
            }
        }

        #endregion
    }
}