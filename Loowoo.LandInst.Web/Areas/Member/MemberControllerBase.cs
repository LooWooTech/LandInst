using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Member
{
    [UserAuthorize]
    [UserRole(Role = UserRole.Member)]
    public class MemberControllerBase : ControllerBase
    {
        private string sessionKey = "currentMemeber";

        protected Model.Member GetCurrentMember()
        {
            return ViewBag.Member as Model.Member;
        }

        protected void UpdateCurrentMember()
        {
            Session.Remove(sessionKey);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session[sessionKey] == null)
            {
                var member = Core.MemberManager.GetMember(Identity.UserID);
                Session[sessionKey] = member;
            }
            ViewBag.Member = Session[sessionKey] as Model.Member;
            base.OnActionExecuting(filterContext);
        }
    }
}
