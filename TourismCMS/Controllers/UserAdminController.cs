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
using TourismCMS.Helper;

namespace TourismCMS.Controllers
{
    public class UserAdminController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: UserAdmin
        public ActionResult Index()
        {
            User currentUser = Session[AccountHashKeys.CurrentAdminUser] as User;
            if (currentUser == null)
                return View();

            if (string.Equals(currentUser.AccountId, SuperAdminAccount.SUPER_ADMIN_ACCOUNT))
            {
                ViewData["RoleName"] = "超级管理员";
            }
            else
            {
                UserRole userRole = UserRoleService.Instance.GetByUserId(currentUser.Id);
                Role role = RoleService.Instance.GetById(userRole.RoleId);
                ViewData["RoleName"] = role.Name;
            }
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

        public ActionResult Personal(int id)
        {
            User user = UserService.Instance.GetById(id);

            return View(user);
        }

        public JsonResult GetUserById(int id)
        {
            User user = new User();
            var newUser = new Object();
            try
            {
                user = UserService.Instance.GetById(id);
                if (user != null)
                {
                    string roleName = string.Empty;
                    UserRole userRole = UserRoleService.Instance.GetByUserId(user.Id);
                    if (userRole != null)
                    {
                        Role role = RoleService.Instance.GetById(userRole.RoleId);
                        roleName = role.Name;
                    }
                    newUser = new
                    {
                        Id = user.Id,
                        Name = user.Name,
                        IDCard = user.IDCard,
                        Email = user.Email,
                        AccountId = user.AccountId,
                        Phone = user.Phone,
                        Position = user.Position,
                        Description = user.Description,
                        RoleName = roleName,
                        Created = DateTime.Parse(user.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(user.Modified.ToString()).ToString(DATETIME_FORMAT)
                    };
                }
            }
            catch
            {

            }
            return Json(newUser, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTableRecords(int start, int limit, string keyword)
        {
            var results = new Object();
            var users = new List<User>();

            try
            {
                int total = 0;
                int pageNum = 0;

                users = UserService.Instance.GetBySomeWhere(keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                List<object> _users = new List<object>();
                foreach (User user in users)
                {
                    string roleName = string.Empty;
                    if (user == null)
                        continue;
                    UserRole userRole = UserRoleService.Instance.GetByUserId(user.Id);
                    if (userRole != null)
                    {
                        Role role = RoleService.Instance.GetById(userRole.RoleId);
                        roleName = role.Name;
                    }

                    _users.Add(new
                    {
                        Id = user.Id,
                        Name = user.Name,
                        IDCard = user.IDCard,
                        Email = user.Email,
                        AccountId = user.AccountId,
                        Phone = user.Phone,
                        Position = user.Position,
                        Description = user.Description,
                        RoleName = roleName,
                        Created = DateTime.Parse(user.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(user.Modified.ToString()).ToString(DATETIME_FORMAT)
                    });
                }

                results = new { pageNum = pageNum, total = total, users = _users };
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
            string strIDCard,
            string strEmail,
            string strAccountId,
            string strPassword,
            string strPhone,
            string strPosition,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                User user = new User
                {
                    Name = strName,
                    IDCard = strIDCard,
                    Email = strEmail,
                    AccountId = strAccountId,
                    Phone = strPhone,
                    Position = strPosition,
                    PasswordSalt = BCrypt.Net.BCrypt.HashPassword(strPassword, BCrypt.Net.BCrypt.GenerateSalt()),
                    Description = strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistAccountId(strAccountId);
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
                var oRet = UserService.Instance.Create(user);

                //2015.12.18增加用户默认设置销售员
                if (oRet != null)
                {
                    DeployRoles();

                    //设置默认值
                    Role role = RoleService.Instance.GetByName("销售员");
                    UserRole userRole = new UserRole
                    {
                        UserId = oRet.Id,
                        RoleId = role.Id,
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    };
                    bRet = UserRoleService.Instance.Create(userRole);
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


        public JsonResult Update(
            int id,
            string strName,
            string strIDCard,
            string strEmail,
            string strAccountId,
            string strPhone,
            string strPosition,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                User user = UserService.Instance.GetById(id);
                //var item = UserService.Instance.GetByAccountId(strAccountId);

                //if (item.Id == id)
                //{
                //    oNotice.Bresult = false;
                //    oNotice.Notice = NoticeResource.HAS_EXISIT;
                //    return Json(oNotice,JsonRequestBehavior.AllowGet);
                //}

                user.Name = strName;
                user.IDCard = strIDCard;
                user.Email = strEmail;
                user.AccountId = strAccountId;
                //user.PasswordSalt = BCrypt.Net.BCrypt.HashPassword(strPassword, BCrypt.Net.BCrypt.GenerateSalt());
                user.Phone = strPhone;
                user.Position = strPosition;
                user.Description = strDescription;
                bRet = UserService.Instance.Update(user);
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
                    UserService.Instance.Delete(id);
                }

                bRet = true;
                if (bRet)
                    strNotice = NoticeResource.DONE_SUCCESS;

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
        public JsonResult UpdateUserRole(int userId, int roleId)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                UserRole userRole = UserRoleService.Instance.GetByUserId(userId);
                if (userRole == null)
                {
                    //add
                    UserRole item = new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId,
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    };

                    bRet = UserRoleService.Instance.Create(item);
                }
                else
                {
                    //update
                    userRole.RoleId = roleId;
                    userRole.Modified = DateTime.Now;
                    bRet = UserRoleService.Instance.Update(userRole);
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

        [HttpPost]
        public JsonResult UpdateUserInfo(
            int userId,
            //string name,
            string IdCard,
            string email,
            string phone,
            string position)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                User user = UserService.Instance.GetById(userId);

                user.Position = position;
                user.Email = email;
                user.Phone = phone;
                user.Position = position;
                user.IDCard = IdCard;
                bRet = UserService.Instance.Update(user);
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

        public JsonResult UpdatePwd(int userId,string pwd)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                User user = UserService.Instance.GetById(userId);

                user.PasswordSalt = BCrypt.Net.BCrypt.HashPassword(pwd, BCrypt.Net.BCrypt.GenerateSalt());
                user.Modified = DateTime.Now;
                bRet = UserService.Instance.Update(user);
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

        //测试自动补全下拉列表
        public JsonResult GetComboData()
        {
            List<object> _reuslts = new List<object>();
            for (int i = 0; i < 100; i++)
            {
                _reuslts.Add(new { Id = i, Text = string.Format("wang{0}", i) });
            }
            return Json(_reuslts, JsonRequestBehavior.AllowGet);
        }

        //Private
        public bool isExistAccountId(string strAccountId)
        {
            bool bRet = false;
            try
            {
                var item = UserService.Instance.GetByAccountId(strAccountId);
                if (item != null)
                    bRet = true;
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        public bool isExistUserRole(int userId, int roleId)
        {
            bool bRet = false;
            try
            {
                var item = UserRoleService.Instance.GetByUserIdAndRoleId(userId, roleId);

                if (item != null)
                    bRet = true;
            }
            catch (Exception)
            {
                bRet = false;
            }
            return bRet;
        }

        private void DeployRoles()
        {
            Role role1 = RoleService.Instance.GetByName("销售员");
            if (role1 == null)
            {
                Role newRole = new Role
                {
                    Name = "销售员",
                    Description = String.Empty,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                RoleService.Instance.Create(newRole);
            }

            Role role2 = RoleService.Instance.GetByName("超级管理员");
            if (role2 == null)
            {
                Role newRole = new Role
                {
                    Name = "超级管理员",
                    Description = String.Empty,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                RoleService.Instance.Create(newRole);
            }
        }

    }
}