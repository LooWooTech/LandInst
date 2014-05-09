using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class EducationController : AdminControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            var filter = new EducationFilter { PageIndex = page };
            ViewBag.List = Core.EducationManager.GetEducations(filter);
            ViewBag.Page = filter;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.EducationManager.GetEducatoin(id) ?? new Education();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Education education)
        {
            Core.EducationManager.SaveEducation(education);
            return JsonSuccess();
        }

        public ActionResult Approvals(int? eduId = 0, int page = 1)
        {
            var filter = new EducationFilter
            {
                PageIndex = page,
                EducationID = eduId,
            };
            ViewBag.List = Core.EducationManager.GetMemberEducations(filter);
            return View();
        }


        public ActionResult Approval(string memberId, string eduId, bool result = true)
        {
            var memberIds = memberId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id)).ToArray();
            var eduIds = eduId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => int.Parse(id)).ToArray();

            for (var i = 0; i < memberIds.Length; i++)
            {
                try
                {
                    Core.EducationManager.Approval(memberIds[i], eduIds[i], result);
                }
                catch
                {

                }
            }

            return JsonSuccess();
        }
    }
}
