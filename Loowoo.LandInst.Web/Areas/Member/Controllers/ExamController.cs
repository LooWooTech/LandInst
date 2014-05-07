using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class ExamController : MemberControllerBase
    {
        public ActionResult Index()
        {
            var memberExams = Core.ExamManager.GetMemberExams(Identity.UserID);
            ViewBag.List = memberExams;
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID);
            var exams = Core.ExamManager.GetExams(new ExamFilter
            {
                SignTime = DateTime.Now.Date
            });

            var memberExams = Core.ExamManager.GetMemberExams(Identity.UserID);
            foreach (var item in memberExams)
            {
                var index = exams.FindIndex(e => e.ID == item.ExamID);
                if (index > 0)
                {
                    exams.RemoveAt(index);
                }
            }

            if (exams.Count == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Signup(int examId, MemberProfile profile)
        {
            Core.MemberManager.SaveProfile(Identity.UserID, profile);
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
