using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Common;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class InstitutionController : AdminControllerBase
    {
        public ActionResult Index(string keyword, ApprovalType? type, int page = 1)
        {
            if (type.HasValue)
            {
                var filter = new InstitutionFilter
                {
                    Keyword = keyword,
                    PageIndex = page,
                    ApprovalType = type.Value
                };
                ViewBag.List = Core.InstitutionManager.GetApprovalInsts(filter);
                ViewBag.Page = filter;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.InstitutionManager.GetInstitution(id) ?? new Model.Institution();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(User user, Model.Institution inst)
        {
            if (Core.UserManager.Exists(user.Username))
            {
                throw new ArgumentException("用户名已被使用！");
            }

            var randomPwd = StringHelper.GenerateRandomString(8);
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = randomPwd;
            }

            user.Question = "初始密码是什么";
            user.Answer = randomPwd;

            Core.UserManager.AddUser(user);
            inst.ID = user.ID;
            Core.InstitutionManager.AddInstitution(inst);
            Core.InstitutionManager.SaveProfile(new InstitutionProfile(inst));
            return JsonSuccess(new { password = randomPwd });
        }

        public ActionResult Approval(int id)
        {
            return JsonSuccess();
        }

    }
}
