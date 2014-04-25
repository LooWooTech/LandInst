using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Controllers
{
    [Authorize]
    [UserRole(Role = UserRole.Institution)]
    public class InstitutionController : ControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult ChangeProfile()
        {
            return View("EditProfile");
        }

        public ActionResult Logout()
        {
            return View();
        }

        public ActionResult AnnualCheck()
        {
            return View();
        }

        public ActionResult Educations()
        {
            return View();
        }

        public ActionResult Members()
        {
            return View();
        }

        public ActionResult ChangeMember()
        {
            return View();
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
