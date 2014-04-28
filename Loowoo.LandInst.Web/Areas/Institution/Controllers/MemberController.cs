using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class MemberController : InstitutionControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            var filter = new MemberFilter { PageIndex = page };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpGet]
        public ActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(int transferType = 1)
        {
            return JsonSuccess();
        }

    }
}
