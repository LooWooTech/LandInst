using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Controllers
{
    [Authorize]
    [UserRole(Role = UserRole.Institution)]
    public class InstitutionController : ControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.Profile = Core.InstitutionManager.GetProfile(Identity.UserID);
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            ViewBag.ProfileCheck = Core.ProfileCheckManager.GetCheck(Identity.UserID, CheckType.Register);
            return View();
        }

        [HttpGet]
        public ActionResult ChangeProfile()
        {
            return View("EditProfile");
        }

        [HttpPost]
        public ActionResult SubmitProfile()
        {
            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout(string fullName)
        {
            var inst = Core.InstitutionManager.GetInsitution(Identity.UserID);
            if (inst != null && inst.FullName == fullName)
            {
                Core.InstitutionManager.LogoutInstitution(inst);
            }
            return JsonSuccess();
        }

        public ActionResult AnnualCheck()
        {
            return View();
        }

        public ActionResult Educations(int page = 1)
        {
            var filter = new Model.Filters.EducationFilter
            {
                InstitutionID = Identity.UserID,
                PageIndex = page
            };
            ViewBag.Educations = Core.EducationManager.GetEducations(filter);
            ViewBag.PageFilter = filter;
            return View();
        }

        public ActionResult Members(int page = 1)
        {
            var filter = new MemberFilter { InstID = Identity.UserID, PageIndex = page };
            ViewBag.Members = Core.MemberManager.GetMembers(filter);
            ViewBag.PageFilter = filter;
            return View();
        }

        [HttpGet]
        public ActionResult ChangeMember()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeMember(int memberId)
        {
            return JsonSuccess();
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
