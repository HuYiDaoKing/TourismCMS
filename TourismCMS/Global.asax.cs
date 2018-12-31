using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TourismCMS.Services;
using System.Data.Entity;
using TourismCMS.EntityDataModel;
using TourismCMS.TranslateResources;

namespace TourismCMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //系统角色
            DeployRoles();

            //系统账户注册
            AddSystemAccount();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private bool AddSystemAccount()
        {
            bool bRet = false;
            try
            {
                User model = new User
                {
                    Name = string.Empty,
                    IDCard = string.Empty,
                    Email = string.Empty,
                    AccountId = SuperAdminAccount.SUPER_ADMIN_ACCOUNT,
                    PasswordSalt = BCrypt.Net.BCrypt.HashPassword(SuperAdminAccount.SUPER_ADMIN_PWD, BCrypt.Net.BCrypt.GenerateSalt()),
                    Phone = string.Empty,
                    Position = string.Empty,
                    Description = string.Empty,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistAccountId(SuperAdminAccount.SUPER_ADMIN_ACCOUNT);
                if (bExist)
                {
                    return false;
                }

                User user = UserService.Instance.Create(model);
                bRet = null == user ? false : true;

                if (bRet)
                {
                    //设置默认值
                    Role role = RoleService.Instance.GetByName("超级管理员");
                    UserRole userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = role.Id,
                        Created = DateTime.Now,
                        Modified = DateTime.Now
                    };
                    UserRoleService.Instance.Create(userRole);
                }

            }
            catch (Exception ex)
            {
                throw ex;
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

        private bool isExistAccountId(string strAccountId)
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
    }
}
