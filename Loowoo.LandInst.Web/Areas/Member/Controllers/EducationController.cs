using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member.Controllers
{
    public class EducationController : MemberControllerBase
    {
        public ActionResult Index()
        {
            ViewBag.List = Core.EducationManager.GetSelfEducations(Identity.UserID);

            return View();
        }

        public ActionResult SignUp(int id)
        {
            var edu = Core.EducationManager.GetEducatoin(id);
            if (edu == null)
            {
                throw new ArgumentException("参数错误");
            }

            Core.EducationManager.AddMemberEducation(GetCurrentMember(), edu);
            return JsonSuccess();
        }

    }
}
