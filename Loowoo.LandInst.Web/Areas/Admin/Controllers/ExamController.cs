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
            ViewBag.Subjects = Core.ExamManager.GetSubjects();
            ViewBag.Model = Core.ExamManager.GetExam(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Exam model)
        {
            model.Subjects = Request.Form["Subjects"];

            Core.ExamManager.SaveExam(model);
            return JsonSuccess();
        }

        public ActionResult Approvals(string name, int? examId, bool? hasCheck, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                InfoID = examId,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                HasCheck = hasCheck,
            };
            ViewBag.Exams = Core.ExamManager.GetExams();
            ViewBag.List = Core.ExamManager.GetVCheckExams(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Subjects()
        {
            ViewBag.List = Core.ExamManager.GetSubjects();
            return View();
        }

        [HttpGet]
        public ActionResult EditSubject(int id = 0)
        {
            ViewBag.Model = Core.ExamManager.GetSubject(id) ?? new ExamSubject();
            return View();
        }

        [HttpPost]
        public ActionResult EditSubject(string name, int totalScore = 0)
        {
            Core.ExamManager.SaveSubject(name, totalScore);
            return JsonSuccess();
        }

        public ActionResult DeleteSubject(int id)
        {
            Core.ExamManager.DeleteSubject(id);
            return JsonSuccess();
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
                Core.ExamManager.Approval(approvalId, result);
            }
            return JsonSuccess();
        }

        public ActionResult Results(string name, int? examId, int page = 1)
        {
            var filter = new MemberFilter
            {
                Result = true,
                Keyword = name,
                InfoID = examId,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.ExamManager.GetVExamResults(filter);
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
                throw new ArgumentException("没有选择哪一次培训");
            }

            var file = Request.Files[0];
            if (file == null || string.IsNullOrEmpty(file.FileName))
            {
                throw new ArgumentException("没有选择Excel文件");
            }

            var filePath = Request.MapPath(Core.FileManager.Upload(HttpContext, file));

            var columns = NOPIHelper.ReadSimpleColumns(filePath);
            var data = NOPIHelper.ReadExcelData(filePath, 1);


            var errors = new List<string>();

            foreach (var values in data)
            {
                var realName = (string)values[0];

                var idNo = Convert.ToString(values[1]);

                var user = realName + "(" + idNo + ")：";

                var member = Core.MemberManager.GetMember(idNo);
                if (member == null)
                {
                    errors.Add(user + "用户不存在");
                    continue;
                }

                var checkLogs = Core.CheckLogManager.GetList(member.ID, CheckType.Exam);
                var approvaled = checkLogs.FirstOrDefault(e => e.Result == true);
                if (approvaled == null)
                {
                    errors.Add(user + "报名培训未通过审批");
                    continue;
                }

                var examResult = Core.ExamManager.GetExamResult(examId, member.ID);
                if (examResult == null)
                {
                    //var exam = Core.ExamManager.GetExam(examId);
                    //Core.ExamManager.ImportExamResult(new ExamResult
                    //{
                    //    ExamID = examId,
                    //    MemberID = member.ID,
                    //    CreateTime = approvaled.UpdateTime.Value,
                    //    Subjects = exam.Subjects,
                    //});
                    errors.Add(user + "未找到成绩单");
                    continue;
                }

                if (columns.Count > 2)
                {
                    string notSignedSubject = null;
                    for (var i = 2; i < columns.Count; i++)
                    {
                        var score = 0;
                        if (values[i] != null)
                        {
                            int.TryParse(values[i].ToString(), out score);
                        }
                        if (examResult.Subjects.Contains(columns[i]))
                        {
                            examResult.Scores += columns[i] + "\t" + score + "\r\n";
                        }
                        else
                        {
                            if (score > 0)
                            {
                                notSignedSubject = columns[i];
                            }
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(notSignedSubject))
                    {
                        errors.Add(user + "没有申请科目" + notSignedSubject);
                        //continue;
                    }
                }

                Core.ExamManager.ImportExamResult(examResult);
                Core.MemberManager.UpdateMemberStatus(member.ID, MemberStatus.Registered);

            }

            return JsonSuccess(errors);
        }

        [HttpGet]
        public ActionResult EditResult(int examId, int memberId)
        {
            var result = Core.ExamManager.GetExamResult(examId, memberId);
            result.Exam = Core.ExamManager.GetExam(result.ExamID);
            var member = Core.MemberManager.GetMember(result.MemberID);

            ViewBag.Member = member;
            ViewBag.Model = result;
            ViewBag.Institution = Core.InstitutionManager.GetInstitution(member.InstitutionID);
            return View();
        }

        [HttpPost]
        public ActionResult EditResult(int examId, int memberId, ExamResult data)
        {
            var scores = Request.Form["scores"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (scores.Length == 0)
            {
                throw new ArgumentException("填写不正确");
            }

            data.Scores = string.Join(",", scores);

            Core.ExamManager.UpdateExamScores(data.MemberID, data.ExamID, data.Scores);
            Core.MemberManager.UpdateMemberStatus(memberId, MemberStatus.Registered);
            return JsonSuccess();
        }

        public ActionResult Delete(int id)
        {
            Core.ExamManager.Delete(id);
            return JsonSuccess();
        }
    }
}
