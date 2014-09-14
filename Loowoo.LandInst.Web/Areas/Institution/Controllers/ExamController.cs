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
                if (examResult != null && !string.IsNullOrEmpty(examResult.Scores))
                {
                    exam = null;
                }
                else
                {

                    var checkLogs = Core.CheckLogManager.GetList(memberId, CheckType.Exam);
                    var signedSubjects = checkLogs.Where(e => e.Result == true).Select(e => e.Data);

                    if (!string.IsNullOrEmpty(exam.Subjects))
                    {

                        ViewBag.Subjects = exam.Subjects.Split(',').Where(name => !signedSubjects.Contains(name)).Select(name => Core.ExamManager.GetSubject(name)).ToList();
                        //如果所有科目都已经报过，并且得到了批准，则不能再报名
                        ViewBag.SignedSubjects = checkLogs.Where(e => !e.Result.HasValue).SelectMany(e => e.Data.Split(',')).Select(name => Core.ExamManager.GetSubject(name)).ToList();
                    }
                    ViewBag.CheckLogs = checkLogs;

                    ViewBag.MemberProfile = Core.MemberManager.GetProfile(memberId);
                    ViewBag.Exam = exam;
                }

            }

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
