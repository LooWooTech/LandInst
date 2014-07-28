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
            var profile = Core.MemberManager.GetProfile(member.ID);
            profile.SetMemberField(member);
            ViewBag.Profile = profile;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(MemberProfile profile)
        {
            var member = GetCurrentMember();
            Core.MemberManager.SaveProfile(member, profile);
            return JsonSuccess();
        }
    }
}
