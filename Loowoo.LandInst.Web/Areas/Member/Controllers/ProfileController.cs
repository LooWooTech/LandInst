using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class ProfileController : MemberControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var member = GetCurrentMember();
            var profile = Core.MemberManager.GetProfile(member.ID) ?? new MemberProfile(member);
            ViewBag.Profile = profile;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(MemberProfile profile)
        {
            var member = GetCurrentMember();
            profile.ID = member.ID;
            Core.MemberManager.SaveProfile(member.ID, profile);
            return JsonSuccess();
        }

        public ActionResult Practice()
        {
            var member = GetCurrentMember();
            ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Practice);
            ViewBag.Practice = Core.PracticeManager.GetPracticeInfo(member.ID, member.InstitutionID);
            return View();
        }
    }
}
