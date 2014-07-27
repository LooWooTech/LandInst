using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class ExamController : AdminControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.ExamManager.GetIndateExams();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.ExamManager.GetExam(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Exam model)
        {
            Core.ExamManager.SaveExam(model);
            return JsonSuccess();
        }

        public ActionResult Approvals(int page = 1)
        {
            var filter = new CheckLogFilter
            {
                PageIndex = page
            };
            ViewBag.Exams = Core.ExamManager.GetExams();
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpPost]
        public ActionResult Approval(string id, bool result = true)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("缺少参数");
            }
            var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s));
            foreach (var approvalId in ids)
            {
                var app = Core.CheckLogManager.GetCheckLog(approvalId);
                if (app != null)
                {
                    //TODO
                    //Core.CheckLogManager.UpdateCheckLog(approvalId, result);
                    //Core.MemberManager.UpdateMemberStatus(app.UserID, result ? MemberStatus.SingupExam : MemberStatus.Registered);
                }
            }
            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult ExamResult(string name, int examId = 0, int page = 1)
        {
            var filter = new CheckLogFilter
            {
                Result = true,
                Keyword = name,
                InfoID = examId,
                PageIndex = page
            };
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Exams = Core.ExamManager.GetExams();
            ViewBag.Page = filter;
            return View();
        }

        [HttpPost]
        public ActionResult ExamResult(string id, bool result = true)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("缺少参数");
            }
            var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s));
            foreach (var approvalId in ids)
            {
                var app = Core.CheckLogManager.GetCheckLog(approvalId);
                if (app != null)
                {
                    Core.ExamManager.UpdateExamResult(app.InfoID, app.UserID, result);

                    //Core.MemberManager.UpdateMemberStatus(app.UserID, result ? MemberStatus.ExamSuccess : MemberStatus.Registered);
                }
            }
            return JsonSuccess();
        }

        public ActionResult Delete(int id)
        {
            Core.ExamManager.Delete(id);
            return JsonSuccess();
        }
    }
}
