using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class ProfileController : MemberControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(Identity.UserID);
            return View();
        }

        public ActionResult Edit()
        {
            return View();
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
