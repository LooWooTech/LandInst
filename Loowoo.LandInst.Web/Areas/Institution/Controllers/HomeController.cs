﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class HomeController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
