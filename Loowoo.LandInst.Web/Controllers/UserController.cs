﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    public class UserController : ControllerBase
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string username, string password)
        {
            var user = Core.UserManager.GetUser(username, password);
            if (user == null)
            {
                throw new ArgumentException("用户名或密码错误！");
            }

            //Save Login
            HttpContext.SaveAuth(user);

            return JsonSuccess();
        }

        public ActionResult SignOut()
        {
            AuthUtils.ClearAuth();
            return Redirect("/user/signin");
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
