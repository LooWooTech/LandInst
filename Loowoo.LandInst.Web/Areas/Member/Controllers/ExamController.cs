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
            var exams = Core.ExamManager.GetMemberExamResult(Identity.UserID);
            ViewBag.List = exams;
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            var member = GetCurrentMember();

            var indateExam = Core.ExamManager.GetIndateExams();
            if (indateExam != null)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(indateExam.ID, member.ID, CheckType.Exam);

                //所有有效的考试都报过名了，跳转到成绩查询
                ViewBag.IndateExam = indateExam;
                ViewBag.CheckLog = checkLog;
            }
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID) ?? new MemberProfile(member);
            return View();
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
            Core.ExamManager.SignupExam(examId, member.ID);
            //保存用户资料
            Core.MemberManager.SaveProfile(member, profile);

            return JsonSuccess();
        }
    }
}
