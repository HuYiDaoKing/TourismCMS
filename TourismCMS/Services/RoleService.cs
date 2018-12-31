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
    public class RoleService
    {
        public static BaseRepository<Role> oBll = new BaseRepository<Role>();
        private static RoleService _instance = null;

        public static RoleService Instance
        {
            get
            {
                return _instance ?? new RoleService(); ;
            }
        }

        private RoleService()
        {

        }

        #region Public

        #region Query

        public Role GetById(int ? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Role.Find(id);
                return query;
            }
        }
        public Role GetByName(string strName)
        {
            using (var context = new TourismDbContext())
            {
                var query = (from c in context.Role where c.Name == strName select c).FirstOrDefault();
                return query;
            }
            return null;
        }

        public List<Role> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Role>();
        }

        public List<Role> GetAll()
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Role.ToList()
                            select c;

                return query
               .OrderByDescending(m => m.Modified)
               .ThenByDescending(m => m.Id)
               .ToList<Role>();
            } 
        }

        #endregion

        #region Create

        public bool Create(Role oItem)
        {
            var oRet = new Role();

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

        public bool Update(Role oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Role.Attach(oIem);
                context.Entry<Role>(oIem).State = EntityState.Modified;
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
                    Role oItem = GetById(id);
                    db.Role.Attach(oItem);
                    db.Entry<Role>(oItem).State = EntityState.Deleted;
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

        private List<Role> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Role.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Role>();
            }
        }

        #endregion
    }
}