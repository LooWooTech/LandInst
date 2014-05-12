using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution
{
    public class InstitutionControllerBase : ControllerBase
    {
        protected Model.Institution GetCurrentInst()
        {
            return Session[sessionKey] as Model.Institution;
        }

        private string sessionKey = "currentInst";
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session[sessionKey] == null)
            {
                Session[sessionKey] = Core.InstitutionManager.GetInstitution(Identity.UserID);
            }
            ViewBag.Model = Session[sessionKey];
            base.OnActionExecuting(filterContext);
        }

    }
}
