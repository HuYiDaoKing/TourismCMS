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
using TourismCMS.Filters;

namespace TourismCMS.Controllers
{
    public class HotelController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: Hotel
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
            //var roles = new List<Role>();
            var hotels = new List<Hotel>();
            try
            {
                int total = 0;
                int pageNum = 0;
                //roles = RoleService.Instance.GetBySomeWhere(keyword, start, limit, out total);
                hotels = HotelService.Instance.GetBySomeWhere(keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                var newHotels = new List<Object>();
                foreach (Hotel hotel in hotels)
                {
                    newHotels.Add(new
                    {
                        Id = hotel.Id,
                        Name = hotel.Name,
                        Phone= hotel.Phone,
                        Address=hotel.Address,
                        Description = hotel.Description,
                        Created = DateTime.Parse(hotel.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(hotel.Modified.ToString()).ToString(DATETIME_FORMAT),
                    });
                }

                results = new { pageNum = pageNum, total = total, hotels = newHotels };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Insert(
            string strName,
            string strPhone,
            string strAddress,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Hotel hotel = new Hotel { 
                    Name=strName,
                    Phone=strPhone,
                    Address=strAddress,
                    Description=strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistHotel(strName);
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

                bRet = HotelService.Instance.Create(hotel);
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

        [AdminUserAuthentication]
        [HttpPost]
        public JsonResult Update(
            int id,
            string strName,
            string strPhone,
            string strAddress,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Hotel hotel = HotelService.Instance.GetById(id);
                hotel.Name = strName;
                hotel.Phone = strPhone;
                hotel.Address = strAddress;
                hotel.Description = strDescription;
                hotel.Modified = DateTime.Now;

                //重复性验证
                /*var bExist = isExistHotel(strName);
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
                }*/


                bRet = HotelService.Instance.Update(hotel);
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

        [AdminUserAuthentication]
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
                    bRet = HotelService.Instance.Delete(id);
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
        private bool isExistHotel(string strName)
        {
            bool bRet = false;
            try
            {
                var item = HotelService.Instance.GetByName(strName);
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