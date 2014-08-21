using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class MemberController : AdminControllerBase
    {

        public ActionResult Index(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                Page = new Model.Filters.PageFilter { PageIndex = page },
                GetInstName = true,
            };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Search(string name)
        {
            var list = Core.MemberManager.GetMembers(new MemberFilter
            {
                Keyword = name
            });
            return JsonSuccess(list);
        }

        [HttpPost]
        public ActionResult Import()
        {
            return JsonSuccess();
        }


        public ActionResult Approvals(string name, CheckType? type, bool? hasCheck, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = type,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
            switch (filter.CheckType)
            {
                default:
                //case CheckType.Profile:
                //case CheckType.Practice:
                    return View();
                case CheckType.Transfer:
                    return RedirectToAction("Transfers");
            }
        }

        public ActionResult Transfers(string name, bool? hasCheck, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = CheckType.Transfer,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.MemberManager.GetVCheckTransfers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Approval(int id, CheckLog data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(id);
            checkLog.Note = data.Note;
            checkLog.Result = data.Result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);
            switch (checkLog.CheckType)
            {
                case CheckType.Exam:
                    Core.ExamManager.Approval(checkLog);
                    break;
                case CheckType.Education:
                    break;
                case CheckType.Transfer:
                    Core.MemberManager.ApprovalTransfer(checkLog);
                    break;
                case CheckType.Practice:
                case CheckType.Profile:
                    Core.MemberManager.ApprovalMember(checkLog);
                    break;
            }
            return JsonSuccess();
        }

        public new ActionResult Profile(int id, int checkLogId = 0)
        {
            if (id == 0)
            {
                return View();
            }

            var member = Core.MemberManager.GetMember(id);
            if (member == null)
            {
                throw new ArgumentException("参数不正确");
            }

            var profile = Core.MemberManager.GetProfile(id) ?? new MemberProfile(member);

            profile.SetMemberField(member);
            ViewBag.Member = member;
            ViewBag.Profile = profile;
            ViewBag.ExamResults = Core.ExamManager.GetVExamResults(new MemberFilter { UserID = id });
            ViewBag.Educations = Core.EducationManager.GetMemberEducations(id);
            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);
            if (checkLogId > 0)
            {
                ViewBag.CheckLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            }
            else
            {
                ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(id);
            }

            return View();
        }


        [HttpGet]
        public ActionResult ResetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPwd(int memberId, string newPwd)
        {
            Core.UserManager.ResetPwd(memberId, newPwd);

            return JsonSuccess();
        }
    }
}
