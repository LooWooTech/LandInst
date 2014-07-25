using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            if (Identity.IsAuthenticated)
            {
                switch (Identity.Role)
                {
                    case Model.UserRole.Admin:
                    case Model.UserRole.Institution:
                    case Model.UserRole.Member:
                        return Redirect("/" + Identity.Role.ToString());
                }
                return Redirect("");
            }
            return Redirect("/user/signin");
        }

    }
}
