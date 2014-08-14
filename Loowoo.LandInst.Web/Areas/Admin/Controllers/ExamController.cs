using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Common;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using System.IO;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class ExamController : AdminControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.ExamManager.GetExams();
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

        public ActionResult Approvals(string name, int? examId, int page = 1)
        {
            var filter = new CheckLogFilter
            {
                Keyword = name,
                InfoID = examId,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.Exams = Core.ExamManager.GetExams();
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        //[HttpPost]
        //public ActionResult Approval(int id, bool result)
        //{
        //    var checkLog = Core.CheckLogManager.GetCheckLog(id);
        //    if (checkLog == null || checkLog.Result.HasValue)
        //    {
        //        throw new ArgumentException("参数错误");
        //    }

        //    checkLog.Result = result;
        //    checkLog.UpdateTime = DateTime.Now;
        //    Core.CheckLogManager.UpdateCheckLog(checkLog);
        //    Core.MemberManager.UpdateMemberStatus(checkLog.UserID, MemberStatus.Registered);
        //    return JsonSuccess();
        //}

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
                Core.ExamManager.Approval(approvalId, result);
            }
            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult ExamResult(string name, int? examId, int page = 1)
        {
            var filter = new CheckLogFilter
            {
                Result = true,
                Keyword = name,
                InfoID = examId,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Exams = Core.ExamManager.GetExams();
            ViewBag.Page = filter.Page;
            return View();
        }

        [HttpGet]
        public ActionResult Import()
        {
            ViewBag.Exams = Core.ExamManager.GetExams();
            return View();
        }

        [HttpPost]
        public ActionResult Import(int examId)
        {
            if (examId == 0)
            {
                throw new ArgumentException("没有选择哪一次考试");
            }

            var file = Request.Files[0];
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                throw new ArgumentException("没有选择Excel文件");
            }

            var filePath = Core.FileManager.Upload(HttpContext, file);

            var columns = NOPIHelper.ReadSimpleColumns(filePath);
            var data = NOPIHelper.ReadExcelData(filePath, 1);

            foreach (var values in data)
            {
                var realName = (string)values[0];

                var idNo = (string)values[1];
                var member = Core.MemberManager.GetMember(realName, idNo);
                if (member == null)
                {
                    continue;
                }

                var examResult = Core.ExamManager.GetExamResult(examId, member.ID);
                if (examResult == null)
                {
                    continue;
                }

                examResult.Result = Convert.ToInt32(values[2]) == 1;
                examResult.Note = null; 
                if (columns.Count > 3)
                {
                    try
                    {
                        for (var i = 3; i < columns.Count; i++)
                        {
                            examResult.Note += columns[i] + "\t" + values[i] + "\r\n";
                        }
                    }
                    catch { }
                }
                var checkLog = Core.CheckLogManager.GetCheckLog(examId, member.ID, CheckType.Exam);
                Core.ExamManager.UpdateExamResult(checkLog, examResult);
            }

            return JsonSuccess();
        }

        //[HttpPost]
        //public ActionResult ExamResult(string id, bool result = true)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        throw new ArgumentException("缺少参数");
        //    }
        //    var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s));
        //    foreach (var approvalId in ids)
        //    {
        //        var app = Core.CheckLogManager.GetCheckLog(approvalId);
        //        if (app != null)
        //        {
        //            Core.ExamManager.UpdateExamResult(app.InfoID, app.UserID, result);

        //            //Core.MemberManager.UpdateMemberStatus(app.UserID, result ? MemberStatus.ExamSuccess : MemberStatus.Registered);
        //        }
        //    }
        //    return JsonSuccess();
        //}

        [HttpGet]
        public ActionResult EditResult(int checkLogId)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            var result = Core.ExamManager.GetExamResult(checkLog);
            result.Exam = Core.ExamManager.GetExam(result.ExamID);
            ViewBag.Model = result;
            ViewBag.CheckLog = checkLog;
            return View();
        }

        [HttpPost]
        public ActionResult EditResult(int checkLogId, ExamResult data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            if (checkLog != null)
            {
                Core.ExamManager.UpdateExamResult(checkLog, data);
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
