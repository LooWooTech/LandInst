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
            ViewBag.Inst = currentInst;
            //尚未提交注册登记或注册登记尚未被审批通过
            if (currentInst.Status == Model.InstitutionStatus.Normal)
            {
                var checkLog = Core.CheckLogManager.GetLastLog(Identity.UserID, Model.CheckType.Profile);
                ViewBag.CheckLog = checkLog;
                ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog);
            }

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
