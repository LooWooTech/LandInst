using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class EducationController : MemberControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.EducationManager.GetEducations();
            return View();
        }

        public ActionResult SignUp()
        {
            return JsonSuccess();
        }

    }
}
