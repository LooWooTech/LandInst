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
            ViewBag.List = Core.ExamManager.GetExams(null);
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
            var filter = new ApprovalFilter
            {
                PageIndex = page
            };
            ViewBag.List = Core.ExamManager.GetApprovalExams(filter);
            ViewBag.Page = filter;
            return View();
        }

        public ActionResult Approval(int userId, int examId)
        {
            return JsonSuccess();
        }

        public ActionResult Delete(int id)
        {
            Core.ExamManager.Delete(id);
            return JsonSuccess();
        }
    }
}
