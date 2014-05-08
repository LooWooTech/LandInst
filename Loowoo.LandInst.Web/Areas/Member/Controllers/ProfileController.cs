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
            var profile = Core.MemberManager.GetProfile(Identity.UserID);
            var member = GetCurrentMember();
            profile.SetMemberField(member);
            ViewBag.Profile = profile;
            ViewBag.Memeber = member;
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Model.Member member, MemberProfile profile)
        {
            return JsonSuccess();
        }

        public ActionResult Register()
        {
            return JsonSuccess();
        }

        public ActionResult Change()
        {
            return JsonSuccess();
        }

    }
}
