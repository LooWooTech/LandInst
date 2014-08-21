using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class TransferController : AdminControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Approvals(string name, int? instId, bool? hasCheck, int page = 1)
        {
            ViewBag.List = Core.MemberManager.GetVCheckTransfers(new Model.Filters.MemberFilter
            {
                CheckType = Model.CheckType.Transfer,
                Keyword = name,
                HasCheck = hasCheck,
                InstID = instId,
                Page = new Model.Filters.PageFilter { PageIndex = page }
            });
            return View();
        }
    }
}
