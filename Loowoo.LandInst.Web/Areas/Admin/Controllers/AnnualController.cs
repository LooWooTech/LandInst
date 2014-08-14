using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class AnnualController : AdminControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.AnnualCheckManager.GetAnnualChecks();
            return View();
        }

        //public ActionResult Approvals(string name, int page = 1)
        //{
        //    var filter = new ApprovalFilter
        //    {
        //        Keyword = name,
        //        PageIndex = page
        //    };
        //    ViewBag.List = Core.AnnualCheckManager.GetApprovalAnnualChecks(filter);
        //    ViewBag.Page = filter.Page;
        //    return View();
        //}

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.AnnualCheckManager.GetModel(id) ?? new AnnualCheck();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(AnnualCheck annualCheck)
        {
            Core.AnnualCheckManager.Save(annualCheck);
            return JsonSuccess();
        }

        public ActionResult Delete(int id)
        {
            Core.AnnualCheckManager.Delete(id);
            return JsonSuccess();
        }

    }
}
