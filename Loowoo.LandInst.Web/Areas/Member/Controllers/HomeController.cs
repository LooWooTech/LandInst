using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class HomeController : MemberControllerBase
    {
        public ActionResult Index()
        {
            var member = Core.MemberManager.GetMember(Identity.UserID);
            ViewBag.Member = member;
            if (member.InstitutionID > 0)
            {
                ViewBag.Institution = Core.InstitutionManager.GetInstitution(member.InstitutionID);
            }
            return View();
        }

    }
}
