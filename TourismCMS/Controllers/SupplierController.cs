using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using TourismCMS.EntityDataModel;
using TourismCMS.Services;
using TourismCMS.Models;
using TourismCMS.TranslateResources;

namespace TourismCMS.Controllers
{
    public class SupplierController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult GetTableRecords(int start, int limit, string keyword)
        {
            var results = new Object();
            var suppliers = new List<Supplier>();
            try
            {
                int total = 0;
                int pageNum = 0;
                suppliers = SupplierService.Instance.GetBySomeWhere(keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                var newSuppliers = new List<Object>();
                foreach (Supplier supplier in suppliers)
                {
                    newSuppliers.Add(new
                    {
                        Id = supplier.Id,
                        Type=supplier.Type,
                        Name=supplier.Name,
                        Manager = supplier.Manager,
                        Phone=supplier.Phone,
                        Fax=supplier.Fax,
                        QQ=supplier.QQ,
                        WeChat=supplier.WeChat,
                        AccountId=supplier.AccountId,
                        Address=supplier.Address,
                        ProxyType=supplier.ProxyType,
                        Description = supplier.Description,
                        Created = DateTime.Parse(supplier.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(supplier.Modified.ToString()).ToString(DATETIME_FORMAT),
                    });
                }

                results = new { pageNum = pageNum, total = total, suppliers = newSuppliers };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetById(int id)
        {
            Supplier supplier = new Supplier();
            var newSupplier = new Object();
            try
            {
                supplier = SupplierService.Instance.GetById(id);
                newSupplier = new {
                    Id = supplier.Id,
                    Type=supplier.Type,
                    Name=supplier.Name,
                    Manager = supplier.Manager,
                    Phone = supplier.Phone,
                    Fax = supplier.Fax,
                    QQ = supplier.QQ,
                    WeChat = supplier.WeChat,
                    AccountId = supplier.AccountId,
                    Address = supplier.Address,
                    ProxyType = supplier.ProxyType,
                    Description = supplier.Description,
                    Created = DateTime.Parse(supplier.Created.ToString()).ToString(DATETIME_FORMAT),
                    Modified = DateTime.Parse(supplier.Created.ToString()).ToString(DATETIME_FORMAT)
                };

            }
            catch
            {

            }
            return Json(newSupplier, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Insert(
            string strType,
            string strName,
            string strManager,
            string strPhone,
            string strFax,
            string strQQ,
            string strWeChat,
            string strAccountId,
            string strAddress,
            string strProxyType,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Supplier supplier = new Supplier
                {
                    Type=strType,
                    Name=strName,
                    Manager = strManager,
                    Phone = strPhone,
                    Fax = strFax,
                    QQ = strQQ,
                    WeChat = strWeChat,
                    AccountId = strAccountId,
                    Address = strAddress,
                    ProxyType = strProxyType,
                    Description = strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistSupplier(strName,strType);
                if (bExist)
                {
                    oNotice = new CNotice
                    {
                        Bresult = bRet,
                        Notice = NoticeResource.HAS_EXISIT,
                        Param1 = string.Empty,
                        Param2 = string.Empty,
                        Param3 = string.Empty
                    };

                    return Json(oNotice, JsonRequestBehavior.AllowGet);
                }


                bRet = SupplierService.Instance.Create(supplier);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(
            int id,
            string strType,
            string strName,
            string strManager,
            string strPhone,
            string strFax,
            string strQQ,
            string strWeChat,
            string strAccountId,
            string strAddress,
            string strProxyType,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Supplier supplier = SupplierService.Instance.GetById(id);
                supplier.Type = strType;
                supplier.Name = strName;
                supplier.Manager = strManager;
                supplier.Phone = strPhone;
                supplier.Fax = strFax;
                supplier.QQ = strQQ;
                supplier.WeChat = strWeChat;
                supplier.AccountId = strAccountId;
                supplier.Address = strAddress;
                supplier.ProxyType = strProxyType;
                supplier.Description = strDescription;
                supplier.Modified = DateTime.Now;

                bRet = SupplierService.Instance.Update(supplier);
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(string strIds)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                strIds = strIds.TrimEnd(',');
                List<string> ids = strIds.Split(',').ToList();

                for (int i = 0; i < ids.Count; i++)
                {
                    int id = int.Parse(ids[0]);
                    bRet = SupplierService.Instance.Delete(id);
                }

                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;
                else
                    strNotice = NoticeResource.DONE_FAILURE;

                oNotice = new CNotice
                {
                    Bresult = bRet,
                    Notice = strNotice,
                    Param1 = string.Empty,
                    Param2 = string.Empty,
                    Param3 = string.Empty
                };
            }
            catch (Exception ex)
            {
                oNotice.Bresult = false;
                oNotice.Notice = NoticeResource.DONE_FAILURE;
            }

            return Json(oNotice, JsonRequestBehavior.AllowGet);
        }

        //Private
        private bool isExistSupplier(string strName, string type)
        {
            bool bRet = false;
            try
            {
                var item = SupplierService.Instance.GetByName(strName,type);
                if (item != null)
                    bRet = true;
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

    }
}