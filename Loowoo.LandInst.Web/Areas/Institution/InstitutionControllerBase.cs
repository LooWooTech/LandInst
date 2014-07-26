using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution
{
    [UserAuthorize]
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
                var inst = Core.InstitutionManager.GetInstitution(Identity.UserID);
                Session[sessionKey] = inst;
            }
            ViewBag.CurrentInst = Session[sessionKey];
            base.OnActionExecuting(filterContext);
        }

    }
}
