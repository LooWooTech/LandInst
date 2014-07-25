using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class HomeController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            var currentInst = GetCurrentInst();
            ViewBag.Profile = Core.InstitutionManager.GetProfile(currentInst.ID);
            return View();
        }

        public ActionResult Search(string keyword)
        {
            var list = Core.InstitutionManager.GetInstitutions(new InstitutionFilter
            {
                PageSize = int.MaxValue,
                Keyword = keyword
            });

            return JsonSuccess(list);
        }
    }
}
