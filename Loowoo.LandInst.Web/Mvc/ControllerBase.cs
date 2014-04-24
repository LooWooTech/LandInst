using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Manager;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Web
{
    public class ControllerBase : AsyncController
    {
        protected ManagerCore Core = new ManagerCore();

        protected UserIdentity Identity
        {
            get
            {
                return (UserIdentity)HttpContext.User.Identity;
            }
        }

        protected ActionResult JsonSuccess(object data = null)
        {
            return Content(new { result = true, data = data }.ToJson());
        }
    }
}