using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Transfer(string id, string mode = "transfer-in")
        {
            if (Request.HttpMethod == "GET")
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
                    var members = Core.MemberManager.GetInstMembers(new MemberFilter { MemberIds = ids, InstID = Identity.UserID, PageSize = int.MaxValue });
                    ViewBag.Members = members;
                }
                return View();
            }
            else
            {
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

    }
}
