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

        //[HttpGet]
        //public ActionResult Transfer()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Transfer(int userId, int oldInstId, int newInstId)
        //{
        //    var member = Core.MemberManager.GetMember(userId);
        //    if (member == null)
        //    {
        //        throw new ArgumentException("UserId");
        //    }

        //    if (member.InstitutionID != oldInstId)
        //    {
        //        throw new ArgumentException("oldInstId");
        //    }

        //    if (member.InstitutionID == newInstId)
        //    {
        //        throw new ArgumentException("newInstId");
        //    }

        //    member.InstitutionID = newInstId;
        //    Core.MemberManager.UpdateMember(member);

        //    return JsonSuccess();
        //}

        public ActionResult Approval(int id, CheckLog data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(id);
            checkLog.Note = data.Note;
            checkLog.Result = data.Result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);
            var member = Core.MemberManager.GetMember(checkLog.UserID);
            switch (checkLog.CheckType)
            {
                case CheckType.Exam:
                    if (checkLog.Result == true && member.Status == MemberStatus.Normal)
                    {
                        Core.MemberManager.UpdateMemberStatus(member.ID, MemberStatus.Registered);
                    }
                    break;
                case CheckType.Education:
                    break;
                case CheckType.Transfer:
                    Core.TransferManager.TransferMember(checkLog.InfoID, member);
                    break;
                case CheckType.Practice:
                    if (checkLog.Result == true)
                    {
                        if (member.InstitutionID == 0)
                        {
                            member.InstitutionID = checkLog.InfoID;
                        }
                        member.Status = MemberStatus.Practice;
                        Core.MemberManager.UpdateMember(member);
                        //Core.MemberManager.UpdateMemberStatus(ember.ID, MemberStatus.Practice);
                    }
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
            var user = Core.UserManager.GetUser(id);
            if (user == null)
            {
                throw new ArgumentException("UserId");

            }

            var member = Core.MemberManager.GetMember(id);
            var profile = Core.MemberManager.GetProfile(id);

            profile.SetMemberField(member);
            ViewBag.User = user;
            ViewBag.Member = member;
            ViewBag.Profile = profile;
            ViewBag.ExamResults = Core.ExamManager.GetMemberExamResult(id);
            ViewBag.Educations = Core.EducationManager.GetMemberEducations(id);
            //TODO 执业信息
            ViewBag.PracticeInfo = Core.PracticeManager.GetPracticeInfo(id);
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

        public ActionResult Practices(string name, bool? result, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                Result = result,
                Type = CheckType.Practice
            };
            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
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
