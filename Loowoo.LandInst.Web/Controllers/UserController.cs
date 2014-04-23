using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    public class UserController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user, Member member)
        {

            user.Role = UserRole.Member;
            Core.UserManager.AddUser(user);
            Core.UserManager.SaveMember(user, member);

            return JsonSuccess();
        }


        public ActionResult ForgetPwd()
        {
            return View();
        }


    }
}
