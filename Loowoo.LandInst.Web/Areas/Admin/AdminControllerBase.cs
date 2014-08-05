using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Admin
{
    [UserAuthorize]
    [UserRole(Role = UserRole.Admin)]
    public class AdminControllerBase : ControllerBase
    {

    }
}
