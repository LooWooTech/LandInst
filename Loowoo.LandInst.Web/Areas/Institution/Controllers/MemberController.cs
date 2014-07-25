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
        public ActionResult Index(string keyword, int page = 1)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                ViewBag.List = null;
            }
            else
            {
                var filter = new MemberFilter
                {
                    InstID = Identity.UserID,
                    Keyword = keyword,
                    PageIndex = page
                };
                ViewBag.List = Core.MemberManager.GetInstMembers(filter);
            }
            return View();
        }

        public new ActionResult Profile(int id)
        {
            return View();
        }

        public ActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return JsonSuccess(null);
            }
            var filter = new MemberFilter
            {
                PageSize = int.MaxValue,
                InstID = Identity.UserID,
                Keyword = keyword,
            };
            return JsonSuccess(Core.MemberManager.GetInstMembers(filter));
        }

        [HttpGet]
        public ActionResult TransferIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferIn(int memberId)
        {
            var member = Core.MemberManager.GetMember(memberId);
            if (member == null)
            {
                throw new ArgumentException("没有选择有效用户");
            }

            Core.MemberManager.Transfer(member, Identity.UserID, TransferMode.In);

            return JsonSuccess();
        }


        [HttpGet]
        public ActionResult TransferOut()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TransferOut(int memberId, int instId)
        {
            var member = Core.MemberManager.GetMember(memberId);
            if (member == null)
            {
                throw new ArgumentException("没有选择有效用户");
            }

            if (member.InstitutionID != Identity.UserID)
            {
                throw new ArgumentException("你没有权限转移此用户");
            }

            Core.MemberManager.Transfer(member,instId, TransferMode.Out);

            return JsonSuccess();
        }

        public ActionResult Transfers(string keyword, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = keyword,
                PageIndex = page,
                Type = CheckType.Transfer

            };

            ViewBag.List = Core.MemberManager.GetApprovalMembers(filter);
            ViewBag.Page = filter;
            return View();
        }

        public ActionResult Transfer(string mode = "in")
        {
            if (Request.HttpMethod == "GET")
            {
                return View();
            }
            else
            {
                var id = Request.QueryString["id"];
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("没有选择转移的用户");
                }
                var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                var members = Core.MemberManager.GetInstMembers(new MemberFilter { MemberIds = ids, InstID = Identity.UserID, PageSize = int.MaxValue });
                foreach (var member in members)
                {
                    if (member.InstitutionID != 0 && member.InstitutionID != Identity.UserID)
                    {
                        continue;
                    }
                    member.InstitutionID = mode == "transfer-in" ? Identity.UserID : 0;
                    Core.MemberManager.UpdateMember(member);
                }
                return JsonSuccess();
            }
        }

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
