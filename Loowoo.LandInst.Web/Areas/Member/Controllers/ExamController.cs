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
            var profile = Core.MemberManager.GetProfile(Identity.UserID);
            ViewBag.Profile = profile;
            if (profile.Status == MemberStatus.ApprovalExam)
            {
                return View();
            }
            var exams = Core.ExamManager.GetExams(new ExamFilter
            {
                SignTime = DateTime.Now.Date
            });

            if (exams.Count == 0)
            {
                ViewBag.Exams = exams;
                return View();
            }

            var memberExams = Core.ExamManager.GetMemberExams(Identity.UserID);
            foreach (var item in memberExams)
            {
                var index = exams.FindIndex(e => e.ID == item.ExamID);
                if (index > -1)
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
                ViewBag.Exams = exams;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Signup(int examId, MemberProfile profile)
        {
            var exam = Core.ExamManager.GetExam(examId);
            if (exam == null)
            {
                throw new ArgumentException("没有选择正确的考试条目！");
            }

            var member = GetCurrentMember();
            //用户状态由新注册用户变成报名考试用户
            member.Status = MemberStatus.SingupExam;
            Core.MemberManager.UpdateMember(member);
            //保存用户资料
            Core.MemberManager.SaveProfile(member, profile);
            //保存用户的考试记录
            Core.ExamManager.SaveMemberExam(Identity.UserID, new MemberExam { ExamID = examId, ExamName = exam.Name });
            
            return JsonSuccess();
        }

    }
}
