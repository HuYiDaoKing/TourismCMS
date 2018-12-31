using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using TourismCMS.EntityDataModel;

namespace TourismCMS.Services
{
    public class UserService
    {
        public static BaseRepository<User> oBll = new BaseRepository<User>();
        private static UserService _instance = null;

        public static UserService Instance
        {
            get
            {
                return _instance ?? new UserService(); ;
            }
        }

        private UserService()
        {

        }

        #region Public

        #region Query

        public User GetById(int? id)
        {
            using (var context = new TourismDbContext())
            {
                var query = context.User.Find(id);
                return query;
            }
        }

        public User GetByAccountId(string strAccountId)
        {
            //2015.1210增加内置系统账号
            //User user = null;
            //if (string.Equals(strAccountId, SuperAdminAccount.SUPER_ADMIN_ACCOUNT))
            //{
            //    return new User
            //    {
            //        Id = 20000,
            //        Name = string.Empty,
            //        IDCard = string.Empty,
            //        Email = string.Empty,
            //        AccountId = SuperAdminAccount.SUPER_ADMIN_ACCOUNT,
            //        PasswordSalt = SuperAdminAccount.SUPER_ADMIN_PWD,
            //        Phone = string.Empty,
            //        Position = string.Empty,
            //        Description = string.Empty,
            //        Created = DateTime.Now,
            //        Modified = DateTime.Now
            //    };
            //}
            //else
            //{
            //    using (var context = new TourismDbContext())
            //    {
            //        var query = (from c in context.User where c.AccountId == strAccountId select c).FirstOrDefault();
            //        return query;
            //    }
            //}
            using (var context = new TourismDbContext())
            {
                var query = (from c in context.User where c.AccountId == strAccountId select c).FirstOrDefault();
                return query;
            }

        }

        public List<User> GetBySomeWhere(string strKeyWord, int iStart, int iLimit, out int iTotal)
        {
            var filtered = GetSearchResult(strKeyWord);
            iTotal = filtered.Count;

            var query = from c in filtered.Skip(iStart).Take(iLimit) select c;

            return query.ToList<User>();
        }

        public List<User> GetSellers()
        {
            List<User> newUsers = new List<User>();
            List<User> users=new List<User>();
            using (var context = new TourismDbContext())
            {
                //暂时迂回处理
                users = context.User.ToList();
                foreach(User user in users)
                {
                    if (user == null)
                        continue;
                    UserRole userRole = context.UserRole.FirstOrDefault(c=>c.UserId==user.Id);
                    Role role = context.Role.Find(userRole.RoleId);
                    if (role.Name.Equals("销售员"))
                    {
                        newUsers.Add(user);
                    }
                }
                return newUsers;
            }
        }

        #endregion

        #region Create

        public User Create(User oItem)
        {
            var oRet = new User();

            try
            {
                oRet = oBll.AddEntities(oItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //var bRet = oRet == null ? false : true;
            return oRet;
        }

        #endregion

        #region Update

        public bool Update(User oIem)
        {
            using (var context = new TourismDbContext())
            {
                context.User.Attach(oIem);
                context.Entry<User>(oIem).State = EntityState.Modified;
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
                    User oItem = GetById(id);
                    db.User.Attach(oItem);
                    db.Entry<User>(oItem).State = EntityState.Deleted;
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

        private List<User> GetSearchResult(string strKeyWord)
        {
            using (var context = new TourismDbContext())
            {
                var query = from c in context.User.ToList()
                            select c;

                return query
               .Where(m => (string.IsNullOrEmpty(strKeyWord) || m.Name.Contains(strKeyWord)))
               .OrderByDescending(m => m.Created)
               //.ThenByDescending(m => m.Id)
               .ToList<User>();
            }
            
            //模拟数据
            /*List<User> users = new List<User>();
            for (int i = 1; i <= 20;i++ )
            {
                users.Add(new User { 
                    Id=i,
                    Name=string.Format("Luckyhu{0}",i),
                    IDCard = "510129**********",
                    Email="hu_xing_@126,.com",
                    AccountId="luckyhu",
                    Phone="18780287009",
                    Position="中层",
                    PasswordSalt=string.Empty,
                    Description=string.Empty,
                    Created=DateTime.Now,
                    Modified=DateTime.Now
                });
            }
            return users;*/
        }

        #endregion

    }
}