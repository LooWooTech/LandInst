using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class ExamController : InstitutionControllerBase
    {
        public ActionResult Index(string name, int examId = 0, int page = 1)
        {
            var filter = new Model.Filters.MemberFilter
            {
                Keyword = name,
                InfoID = examId,
                InstID = GetCurrentInst().ID,
                Page = new Model.Filters.PageFilter { PageIndex = page }
            };
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Page = filter.Page;
            ViewBag.Exams = Core.ExamManager.GetExams();
            return View();
        }

        public ActionResult Results(string name, int examId = 0, int page = 1)
        {
            var filter = new Model.Filters.MemberFilter
            {
                Keyword = name,
                InfoID = examId,
                InstID = GetCurrentInst().ID,
                Page = new Model.Filters.PageFilter { PageIndex = page }
            };
            ViewBag.List = Core.ExamManager.GetVExamResults(filter);
            ViewBag.Page = filter.Page;
            ViewBag.Exams = Core.ExamManager.GetExams();
            return View();
        }

        [HttpGet]
        public ActionResult SignUp(int memberId = 0)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Search", "Member", new { target = "/institution/exam/signup" });
            }
            var exam = Core.ExamManager.GetIndateExam();
            if (exam != null)
            {
                var examResult = Core.ExamManager.GetExamResult(exam.ID, memberId);

                ViewBag.Subjects = exam.Subjects.Split(',').Select(name => Core.ExamManager.GetSubject(name)).ToList();
                if (examResult != null)
                {
                    ViewBag.SignedSubjects = examResult.Subjects.Split(',').Select(name => Core.ExamManager.GetSubject(name)).ToList();
                }

                ViewBag.CheckLog = Core.CheckLogManager.GetCheckLog(exam.ID, memberId, CheckType.Exam);

                ViewBag.MemberProfile = Core.MemberManager.GetProfile(memberId);

            }

            ViewBag.Exam = exam;
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(int memberId, int examId)
        {
            var subjectNames = Request.Form["Subjects"];
            if (string.IsNullOrEmpty(subjectNames))
            {
                throw new ArgumentException("没有选择报考科目");
            }

            subjectNames = subjectNames.Trim(',');

            Core.ExamManager.SubmitExam(examId, memberId, subjectNames);

            return JsonSuccess();
        }
    }
}
