using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class EducationController : InstitutionControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            var filter = new Model.Filters.ApprovalFilter
            {
                PageIndex = page
            };
            ViewBag.Educations = Core.EducationManager.GetApprovalEducations(filter);
            ViewBag.PageFilter = filter;
            return View();
        }

        public ActionResult Import()
        {
            return View();
        }

    }
}
