using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class InstitutionController : AdminControllerBase
    {
        public ActionResult Search(string name)
        {
            var list = Core.InstitutionManager.GetInstitutions(new InstitutionFilter
            {
                LikeName = name
            });
            return JsonSuccess(new { list });
        }


        public ActionResult Index(string keyword, int businessType = 0, int page = 1)
        {
            var filter = new InstitutionFilter
            {
                LikeName = keyword,
                PageIndex = page
            };
            ViewBag.List = Core.InstitutionManager.GetInstitutions(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int instId = 0)
        {
            if (instId > 0)
            {
                ViewBag.Model = Core.InstitutionManager.GetInstitution(instId);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Model.Institution entity)
        {
            return JsonSuccess();
        }

        public ActionResult Approval(int instId)
        {
            return JsonSuccess();
        }

    }
}
