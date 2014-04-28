using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class AnnualController : AdminControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.AnnualCheckManager.GetAnnualApprovals();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int annualCheckId)
        {
            ViewBag.AnnualCheck = Core.AnnualCheckManager.GetAnnualApproval(annualCheckId);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(AnnualApproval annualCheck)
        {
            return JsonSuccess();
        }

    }
}
