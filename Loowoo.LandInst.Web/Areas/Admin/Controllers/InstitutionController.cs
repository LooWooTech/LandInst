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
        public ActionResult Search(string name)
        {
            var list = Core.InstitutionManager.GetInstitutions(new InstitutionFilter
            {
                LikeName = name
            });
            return JsonSuccess(new { list });
        }


        public ActionResult Index(string keyword, int businessType = 0, int page = 1)
        {
            var filter = new InstitutionFilter
            {
                LikeName = keyword,
                PageIndex = page
            };
            ViewBag.List = Core.InstitutionManager.GetInstitutions(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.InstitutionManager.GetInstitution(id) ?? new Model.Institution();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string username, Model.Institution inst, Model.InstitutionProfile profile)
        {
            var user = Core.UserManager.GetUser(username);
            if (user != null)
            {
                throw new ArgumentException("用户名已被使用！");
            }

            user = new User
            {
                Username = username,
                Password = StringHelper.GenerateRandomString(8),
                Role = UserRole.Institution
            };

            Core.UserManager.AddUser(user);
            Core.InstitutionManager.SaveInstitution(inst);
            Core.InstitutionManager.SaveProfile(profile);
            return JsonSuccess(new { user });
        }

        public ActionResult Approval(int id)
        {
            return JsonSuccess();
        }

    }
}
