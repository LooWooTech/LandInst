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
            if (!member.CanSingup)
            {
                return View();
            }

            var profile = Core.MemberManager.GetProfile(Identity.UserID);
            ViewBag.Profile = profile;

            var exams = Core.ExamManager.GetExams(new ExamFilter
            {
                SignTime = DateTime.Now.Date
            });


            ViewBag.Exams = exams;
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

            Core.MemberManager.UpdateMemberStatus(member.ID, MemberStatus.SingupExam);

            return JsonSuccess();
        }

    }
}
