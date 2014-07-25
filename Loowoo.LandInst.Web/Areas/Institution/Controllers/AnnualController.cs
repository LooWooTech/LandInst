using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class AnnualController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.AnnualCheckManager.GetInstAnnualChecks(Identity.UserID);
            return View();
        }

        public ActionResult Signup(int id = 0)
        {
            var model = Core.AnnualCheckManager.GetModel(id);
            if (model == null)
            {
                throw new ArgumentException("ID参数错误");
            }

            if (DateTime.Now < model.StartDate || DateTime.Now > model.EndDate)
            {
                throw new Exception("目前无法申请年检");
            }
            //TODO
            //Core.CheckLogManager.AddApproval(id, Identity.UserID, Model.InfoType.Annual);
            return JsonSuccess();
        }

    }
}
