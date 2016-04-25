using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Web.Controllers
{
    public class UserController : ControllerBase
    {
        public ActionResult Index()
        {
            return Redirect("/user/signin");
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
            user.LastLoginIP = Request.UserHostAddress;
            Core.UserManager.UpdateLogin(user);
            return JsonSuccess(new { role = user.Role.ToString().ToLower() });
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            HttpContext.ClearAuth();
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
            var userId = Core.UserManager.AddUser(user);
            member.ID = userId;
            Core.MemberManager.AddMember(member);

            return JsonSuccess();
        }

        [UserAuthorize]
        [HttpGet]
        public ActionResult EditPassword()
        {
            var user = Core.UserManager.GetUser(Identity.UserID);
            ViewBag.User = user;
            switch (user.Role)
            {
                case UserRole.Institution:
                    ViewBag.CurrentInst = Core.InstitutionManager.GetInstitution(user.ID);
                    break;
                case UserRole.Member:
                    ViewBag.Member = Core.MemberManager.GetMember(user.ID);
                    break;
            }
            return View();
        }

        [UserAuthorize]
        [HttpPost]
        public ActionResult EditPassword(string newPwd, string reNewPwd)
        {
            if (string.IsNullOrEmpty(newPwd))
            {
                throw new ArgumentException("新密码没有填写");
            }

            if (string.IsNullOrEmpty(reNewPwd))
            {
                throw new ArgumentException("确认密码没有填写");
            }

            if (newPwd != reNewPwd)
            {
                throw new ArgumentException("两次密码输入不一致");
            }

            var user = Core.UserManager.GetUser(Identity.UserID);
            ViewBag.User = user;

            Core.UserManager.UpdatePassword(Identity.UserID, newPwd);

            return JsonSuccess();
        }


        [HttpGet]
        public ActionResult ForgetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPwd(string username, string answer)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(answer))
            {
                throw new ArgumentException("参数不正确");
            }
            if (Core.UserManager.ValidateQuestion(username, answer))
            {
                return JsonSuccess(new { key = (username + "|" + answer + "|" + DateTime.Now).AESEncrypt() });
            }
            throw new HttpException(401, "答案不正确");
        }

        [HttpGet]
        public ActionResult ResetPwd(string key)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPwd(string key, string password, string repassword)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("参数不正确");
            }
            if (password != repassword)
            {
                throw new ArgumentException("两次输入密码不正确");
            }
            var userInfo = key.AESDecrypt().Split('|');
            var username = userInfo[0];
            var createTime = DateTime.Parse(userInfo[2]);
            if ((DateTime.Now - createTime).TotalDays > 1)
            {
                throw new HttpException(401, "请重新找回密码");
            }
            var user = Core.UserManager.GetUser(username);

            if (user != null)
            {
                Core.UserManager.UpdatePassword(user.ID, password);
                return JsonSuccess();
            }
            throw new HttpException(401, "参数不正确");
        }
    }
}
