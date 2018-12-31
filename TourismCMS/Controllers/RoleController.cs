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
    public class RoleController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult GetTableRecords(int start, int limit, string keyword)
        {
            var results = new Object();
            var roles = new List<Role>();
            try
            {
                int total = 0;
                int pageNum = 0;
                roles = RoleService.Instance.GetBySomeWhere(keyword,start,limit,out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                var newRoles = new List<Object>();
                foreach (Role role in roles)
                {
                    newRoles.Add(new
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description,
                        Created = DateTime.Parse(role.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(role.Created.ToString()).ToString(DATETIME_FORMAT),
                    });
                }

                results = new { pageNum = pageNum, total = total, roles = newRoles };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Insert(
            string strName,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Role role = new Role
                {
                    Name = strName,
                    Description = strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistRole(strName);
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
                bRet = RoleService.Instance.Create(role);
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
            string strName,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Role role = RoleService.Instance.GetById(id);
                role.Name = strName;
                role.Description = strDescription;
                role.Modified = DateTime.Now;


                bRet = RoleService.Instance.Update(role);
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
                    bRet = RoleService.Instance.Delete(id);
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

        //角色下拉列
        public JsonResult GetRoleComboData()
        {
            List<object> _reuslts = new List<object>();
            List<Role> roles = RoleService.Instance.GetAll();
            foreach(Role role in roles)
            {
                _reuslts.Add(new { Id = role.Id, Text = role.Name });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //Private
        public bool isExistRole(string strName)
        {
            bool bRet = false;
            try
            {
                var item = RoleService.Instance.GetByName(strName);
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