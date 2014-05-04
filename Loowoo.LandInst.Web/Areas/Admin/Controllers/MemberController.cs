using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class MemberController : AdminControllerBase
    {
        public ActionResult Index(int businessType = 0, int page = 1)
        {
            var filter = new MemberFilter { PageIndex = page };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter;
            return View();
        }

        public ActionResult Search(string name, string no)
        {
            var list = Core.MemberManager.GetMembers(new MemberFilter
            {
                LikeName = name
            });
            return JsonSuccess(new { members = list });
        }

        [HttpPost]
        public ActionResult Import()
        {
            return JsonSuccess();
        }

        public ActionResult Transfer()
        {
            return View();
        }

        public ActionResult Transfer(int userId, int oldInstId, int newInstId)
        {
            var member = Core.MemberManager.GetMember(userId);
            if (member == null)
            {
                throw new ArgumentException("UserId");
            }

            if (member.InstitutionID != oldInstId)
            {
                throw new ArgumentException("oldInstId");
            }

            if (member.InstitutionID == newInstId)
            {
                throw new ArgumentException("newInstId");
            }

            member.InstitutionID = newInstId;
            Core.MemberManager.UpdateMember(member);

            return JsonSuccess();
        }

        public ActionResult Approval(int id, ApprovalType type, bool result = true)
        {
            Core.ApprovalManager.UpdateApproval(id, type, result);

            return JsonSuccess();
        }

        public new ActionResult Profile(int userId)
        {
            if (userId == 0)
            {
                return View();
            }
            var user = Core.UserManager.GetUser(userId);
            if (user == null)
            {
                throw new ArgumentException("UserId");
            }
            var member = Core.MemberManager.GetMember(userId);

            var profile = Core.MemberManager.GetProfile(userId);

            ViewBag.User = user;
            ViewBag.Member = member;
            ViewBag.Profile = profile;

            return View();
        }

        [HttpGet]
        public ActionResult ResetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPwd(string userId)
        {
            return JsonSuccess();
        }
    }
}
