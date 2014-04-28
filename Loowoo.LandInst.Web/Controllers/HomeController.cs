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
                
            }
            return Redirect("/user/signin");
        }

    }
}
