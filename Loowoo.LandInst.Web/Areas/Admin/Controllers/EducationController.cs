using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class EducationController : AdminControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            var filter = new EducationFilter { PageIndex = page };
            ViewBag.List = Core.EducationManager.GetEducations(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.Model = Core.EducationManager.GetEducatoin(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Education education)
        {
            Core.EducationManager.SaveEducation(education);
            return JsonSuccess();
        }

        public ActionResult Approval(int id)
        {
            return JsonSuccess();
        }

    }
}
