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
    public class UserRoleService
    {
        public static BaseRepository<UserRole> oBll = new BaseRepository<UserRole>();
        private static UserRoleService _instance = null;

        public static UserRoleService Instance
        {
            get
            {
                return _instance ?? new UserRoleService(); ;
            }
        }

        private UserRoleService()
        {

        }

        #region Public

        #region Query

        public UserRole GetByUserId(int userId)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.UserRole.FirstOrDefault(c => c.UserId == userId);
                return query;
            }
        }

        public UserRole GetByUserIdAndRoleId(int userId,int roleId)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.UserRole.FirstOrDefault(c => (c.UserId == userId && c.RoleId==roleId));
                return query;
            }
        }

        public UserRole GetById(int id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.UserRole.Find(id);
                return query;
            }
        }

        public List<UserRole> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<UserRole>();
        }

        #endregion

        #region Create

        public bool Create(UserRole oItem)
        {
            var oRet = new UserRole();

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

        public bool Update(UserRole oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.UserRole.Attach(oIem);
                context.Entry<UserRole>(oIem).State = EntityState.Modified;
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
                    UserRole oItem = GetById(id);
                    db.UserRole.Attach(oItem);
                    db.Entry<UserRole>(oItem).State = EntityState.Deleted;
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

        private List<UserRole> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.UserRole.ToList()
                            select c;

                return query
               //.Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Modified)
               //.ThenByDescending(m => m.Id)
               .ToList<UserRole>();
            }
        }

        #endregion
    }
}