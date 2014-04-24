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
            return View();
        }

        public ActionResult SubmitProfile()
        {
            return View();
        }

        public ActionResult ExamResult()
        {
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
            return JsonSuccess();
        }
    }
}
