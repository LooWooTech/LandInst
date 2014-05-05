using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class ExamController : MemberControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.ExamManager.GetMemberExams(Identity.UserID);
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID);
            ViewBag.Exams = Core.ExamManager.GetMemberExams(Identity.UserID);

            return View();
        }

        [HttpPost]
        public ActionResult SingUp(MemberProfile profile, int examId)
        {
            Core.MemberManager.SaveProfile(profile);
            var exam = Core.ExamManager.GetExam(examId);
            if (exam == null)
            {
                throw new ArgumentException("examId");
            }
            Core.ExamManager.SaveMemberExam(Identity.UserID, new MemberExam { ExamID = examId, ExamName = exam.Name });
            return JsonSuccess();
        }

    }
}
