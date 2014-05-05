using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class ProfileController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.Profile = Core.InstitutionManager.GetProfile(Identity.UserID);
            ViewBag.Approval = Core.ApprovalManager.GetApproval(Identity.UserID, ApprovalType.Register);
            return View();
        }

        public ActionResult Submit()
        {
           //注册和变更只有一种存在
         return JsonSuccess();
        }

        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout(string fullName)
        {
            Core.InstitutionManager.LogoutInstitution(fullName);
            return JsonSuccess();
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
