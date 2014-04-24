using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    [Authorize]
    [UserRole(Role = UserRole.Member)]
    public class MemberController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult ExamResult()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignUpExam()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUpExam(MemberProfile profile, int examId)
        {
            return JsonSuccess();
        }
    }
}
