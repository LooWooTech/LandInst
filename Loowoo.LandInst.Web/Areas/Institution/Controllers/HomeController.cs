using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class HomeController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            var currentInst = GetCurrentInst();
            ViewBag.Inst = currentInst;
            //尚未提交注册登记或注册登记尚未被审批通过
            if (currentInst.Status == Model.InstitutionStatus.Normal)
            {
                ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(Identity.UserID, Model.CheckType.Profile);
                ViewBag.Profile = Core.InstitutionManager.GetProfile(currentInst.ID);
            }
            else
            {
                var annualCheck = Core.AnnualCheckManager.GetIndateModel();
                if (annualCheck != null)
                {
                    var checkLog = Core.CheckLogManager.GetLastLog(currentInst.ID, CheckType.Annual);
                    if (checkLog == null || checkLog.Result == false)
                    {
                        ViewBag.AnnualCheck = annualCheck;
                    }
                }
            }

            return View();
        }

        public ActionResult Search(string keyword)
        {
            var list = Core.InstitutionManager.GetInstitutions(new InstitutionFilter
            {
                Keyword = keyword
            });

            return JsonSuccess(list);
        }
    }
}
