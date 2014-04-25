using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    [Authorize]
    [UserRole(Role = UserRole.Member)]
    public class MemberController : ControllerBase
    {
        public ActionResult Index()
        {
            var member = Core.MemberManager.GetMember(Identity.UserID);
            ViewBag.Member = member;
            if (member.InstitutionID > 0)
            {
                ViewBag.Institution = Core.InstitutionManager.GetInsitution(member.InstitutionID);
            }
            return View();
        }

        public new ActionResult Profile()
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID);
            return View();
        }

        public ActionResult SubmitProfile()
        {
            return View();
        }

        public ActionResult ExamResult()
        {
            ViewBag.Exams = Core.ExamManager.GetMemberExams(Identity.UserID);
            return View();
        }

        [HttpGet]
        public ActionResult SignUpExam()
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID);
            ViewBag.Exams = Core.ExamManager.GetExams(new Model.Filters.ExamFilter
            {
                SignTime = DateTime.Now.Date,
                UserID = Identity.UserID
            });
            return View();
        }

        [HttpPost]
        public ActionResult SignUpExam(MemberProfile profile, int examId)
        {
            Core.MemberManager.SaveProfile(profile);
            var exam  = Core.ExamManager.GetExam(examId);
            if(exam == null)
            {
                throw new ArgumentException("examId");
            }
            Core.ExamManager.Add(new MemberExam { UserID = Identity.UserID, ExamID = examId, ExamName = exam.Name});
            return JsonSuccess();
        }

        public ActionResult Education()
        {
            return View();
        }
    }
}
