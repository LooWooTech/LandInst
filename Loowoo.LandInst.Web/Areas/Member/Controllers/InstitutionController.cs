using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class InstitutionController : MemberControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
