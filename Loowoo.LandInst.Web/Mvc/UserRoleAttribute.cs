using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web
{
    public class UserRoleAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public UserRoleAttribute()
        {
            Role = UserRole.Everyone;
        }

        public UserRole Role { get; set; }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var currentUser = (UserIdentity)Thread.CurrentPrincipal.Identity;
            if (Role == UserRole.Everyone)
            {
                return;
            }

            if (Role == currentUser.Role)
            {
                return;
            }
            else
            {
                //throw new HttpException(401, "你没有权限查看此页面");
            }

            return;
        }
    }
}