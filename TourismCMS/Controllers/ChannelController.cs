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
    public class ChannelController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: Channel
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
            var channels = new List<Channel>();
            try
            {
                int total = 0;
                int pageNum = 0;
                channels = ChannelService.Instance.GetBySomeWhere(keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                var newChannels = new List<Object>();
                foreach(Channel channel in channels)
                {
                    newChannels.Add(new { 
                        Id =channel.Id,
                        Type=channel.Type,
                        Name =channel.Name,
                        Description =channel.Description,
                        Created = DateTime.Parse(channel.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(channel.Modified.ToString()).ToString(DATETIME_FORMAT),
                    });
                }

                results = new { pageNum = pageNum, total = total, channels = newChannels };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(results, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult Insert(
            string strType,
            string strName,
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Channel channel = new Channel {
                    Type=strType,
                    Name = strName,
                    Description = strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                var bExist = isExistChannel(strName,strType);
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

                bRet = ChannelService.Instance.Create(channel);
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
            string strDescription)
        {
            CNotice oNotice = new CNotice();
            try
            {
                string strNotice = string.Empty;
                var bRet = false;

                Channel channel = ChannelService.Instance.GetById(id);
                channel.Type = strType;
                channel.Name = strName;
                channel.Description = strDescription;
                channel.Modified = DateTime.Now;

                //重名验证
                /*var bExist = isExistChannel(strName, strType);
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

                bRet = ChannelService.Instance.Update(channel);
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
                    bRet = ChannelService.Instance.Delete(id);
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
        private bool isExistChannel(string strName,string type)
        {
            bool bRet = false;
            try
            {
                var item = ChannelService.Instance.GetByName(strName,type);
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