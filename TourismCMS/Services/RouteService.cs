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
    public class RouteService
    {
        public static BaseRepository<Route> oBll = new BaseRepository<Route>();
        private static RouteService _instance = null;

        public static RouteService Instance
        {
            get
            {
                return _instance ?? new RouteService(); ;
            }
        }

        private RouteService()
        {

        }

        #region Public

        #region Query

        public Route GetByName(string strName)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Route.FirstOrDefault(c => c.Name == strName);
                return query;
            }
        }

        public Route GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Route.Find(id);
                return query;
            }
        }

        public List<Route> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Route.ToList()
                            select c;

                return query
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Route>();
            }
        }

        public List<Route> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Route>();
        }

        #endregion

        #region Create

        public bool Create(Route oItem)
        {
            var oRet = new Route();

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

        public bool Update(Route oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Route.Attach(oIem);
                context.Entry<Route>(oIem).State = EntityState.Modified;
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
                    Route oItem = GetById(id);
                    db.Route.Attach(oItem);
                    db.Entry<Route>(oItem).State = EntityState.Deleted;
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

        private List<Route> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Route.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Modified)
               //.ThenByDescending(m => m.Id)
               .ToList<Route>();
            }
        }

        #endregion
    }
}