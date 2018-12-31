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
    public class TicketController : BaseController
    {
        private const string DATETIME_FORMAT = @"yyyy-MM-dd HH:mm:ss";

        // GET: Ticket
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
            var tickets = new List<Ticket>();
            try
            {
                int total = 0;
                int pageNum = 0;
                tickets = TicketService.Instance.GetBySomeWhere(keyword, start, limit, out total);

                if (total % limit == 0)
                {
                    pageNum = total / limit;
                }
                else
                {
                    pageNum = total / limit + 1;
                }

                var newTickets = new List<Object>();
                foreach (Ticket ticket in tickets)
                {
                    newTickets.Add(new
                    {
                        Id = ticket.Id,
                        Name = ticket.Name,
                        Description = ticket.Description,
                        Created = DateTime.Parse(ticket.Created.ToString()).ToString(DATETIME_FORMAT),
                        Modified = DateTime.Parse(ticket.Modified.ToString()).ToString(DATETIME_FORMAT),
                    });
                }

                results = new { pageNum = pageNum, total = total, tickets = newTickets };
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

                Ticket ticket = new Ticket
                {
                    Name = strName,
                    Description = strDescription,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };

                var bExist = isExistTicket(strName);
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

                bRet = TicketService.Instance.Create(ticket);
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

                Ticket ticket = TicketService.Instance.GetById(id);
                ticket.Name = strName;
                ticket.Description = strDescription;
                ticket.Modified = DateTime.Now;

                bRet = TicketService.Instance.Update(ticket);
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
                    bRet = TicketService.Instance.Delete(id);
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
        private bool isExistTicket(string strName)
        {
            bool bRet = false;
            try
            {
                var item = TicketService.Instance.GetByName(strName);
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