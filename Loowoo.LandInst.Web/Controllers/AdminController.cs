using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    [Authorize]
    [UserRole(Role = UserRole.Admin)]
    public class AdminController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
