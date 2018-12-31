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
    public class SupplierService
    {
        public static BaseRepository<Supplier> oBll = new BaseRepository<Supplier>();
        private static SupplierService _instance = null;

        public static SupplierService Instance
        {
            get
            {
                return _instance ?? new SupplierService(); ;
            }
        }

        private SupplierService()
        {

        }

        #region Public

        #region Query

        public Supplier GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Supplier.Find(id);
                return query;
            }
        }

        public Supplier GetByName(string strName, string type)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.Supplier.FirstOrDefault(c => c.Type == type && c.Name == strName);
                return query;
            }
        }

        public List<Supplier> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<Supplier>();
        }

        public List<Supplier> GetAll(string type)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Supplier.ToList()
                            select c;
                return query
                .Where(m => string.IsNullOrEmpty(type) || m.Type == type)
               .OrderByDescending(m => m.Modified)
               .ThenByDescending(m => m.Id)
               .ToList<Supplier>();
            }
        }

        #endregion

        #region Create

        public bool Create(Supplier oItem)
        {
            var oRet = new Supplier();

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

        public bool Update(Supplier oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.Supplier.Attach(oIem);
                context.Entry<Supplier>(oIem).State = EntityState.Modified;
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
                    Supplier oItem = GetById(id);
                    db.Supplier.Attach(oItem);
                    db.Entry<Supplier>(oItem).State = EntityState.Deleted;
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

        private List<Supplier> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.Supplier.ToList()
                            select c;

                return query
               //.Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<Supplier>();
            }
        }

        #endregion
    }
}