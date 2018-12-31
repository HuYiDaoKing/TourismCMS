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
    public class SupplierProductService
    {
        public static BaseRepository<SupplierProduct> oBll = new BaseRepository<SupplierProduct>();
        private static SupplierProductService _instance = null;

        public static SupplierProductService Instance
        {
            get
            {
                return _instance ?? new SupplierProductService(); ;
            }
        }

        private SupplierProductService()
        {

        }

        #region Public

        #region Query

        public SupplierProduct GetById(int id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.SupplierProduct.Find(id);
                return query;
            }
        }

        public List<SupplierProduct> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<SupplierProduct>();
        }

        #endregion

        #region Create

        public bool Create(SupplierProduct oItem)
        {
            var oRet = new SupplierProduct();

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

        public bool Update(SupplierProduct oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.SupplierProduct.Attach(oIem);
                context.Entry<SupplierProduct>(oIem).State = EntityState.Modified;
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
                    SupplierProduct oItem = GetById(id);
                    db.SupplierProduct.Attach(oItem);
                    db.Entry<SupplierProduct>(oItem).State = EntityState.Deleted;
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

        private List<SupplierProduct> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.SupplierProduct.ToList()
                            select c;

                return query
               //.Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               //.OrderByDescending(m => m.Modified)
               //.ThenByDescending(m => m.Id)
               .ToList<SupplierProduct>();
            }
        }

        #endregion
    }
}