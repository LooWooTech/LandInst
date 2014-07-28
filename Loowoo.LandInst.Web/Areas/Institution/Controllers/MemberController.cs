using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class MemberController : InstitutionControllerBase
    {
        public ActionResult Index(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                InstID = string.IsNullOrEmpty(name) ? Identity.UserID : 0,
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.MemberManager.GetInstMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public new ActionResult Profile(int id)
        {
            return View();
        }

        public ActionResult Search(string keyword, int instId = 0)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return JsonSuccess(null);
            }
            var filter = new MemberFilter
            {
                InstID = instId,
                Keyword = keyword,
            };
            return JsonSuccess(Core.MemberManager.GetInstMembers(filter));
        }


        [HttpGet]
        public ActionResult TransferIn(int memberId = 0)
        {
            ViewBag.Member = Core.MemberManager.GetMember(memberId);
            return View();
        }

        [HttpGet]
        public ActionResult TransferOut(int memberId = 0)
        {
            ViewBag.Member = Core.MemberManager.GetMember(memberId);
            return View();
        }


        [HttpPost]
        public ActionResult Transfer(int memberId, TransferMode mode, int instId = 0)
        {
            var member = Core.MemberManager.GetMember(memberId);
            if (member == null)
            {
                throw new ArgumentException("没有选择有效用户");
            }

            if (mode == TransferMode.Out)
            {
                if (member.InstitutionID != Identity.UserID)
                {
                    throw new ArgumentException("你没有权限转移此用户");
                }
                Core.TransferManager.Submit(member, instId, mode);
            }
            else
            {
                Core.TransferManager.Submit(member, Identity.UserID, mode);
            }


            return JsonSuccess();
        }

        public ActionResult Transfers(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                Type = CheckType.Transfer

            };

            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        //public ActionResult Transfer(string mode = "in")
        //{
        //    if (Request.HttpMethod == "GET")
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        var id = Request.QueryString["id"];
        //        if (string.IsNullOrEmpty(id))
        //        {
        //            throw new ArgumentException("没有选择转移的用户");
        //        }
        //        var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
        //        var members = Core.MemberManager.GetInstMembers(new MemberFilter { MemberIds = ids, InstID = Identity.UserID });
        //        foreach (var member in members)
        //        {
        //            if (member.InstitutionID != 0 && member.InstitutionID != Identity.UserID)
        //            {
        //                continue;
        //            }
        //            member.InstitutionID = mode == "transfer-in" ? Identity.UserID : 0;
        //            Core.MemberManager.UpdateMember(member);
        //        }
        //        return JsonSuccess();
        //    }
        //}

        public ActionResult Staff(int memberId = 0)
        {
            if (memberId > 0)
            {
                ViewBag.Member = Core.MemberManager.GetMember(memberId);
            }
            return View();
        }

    }
}
