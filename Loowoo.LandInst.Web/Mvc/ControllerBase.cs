﻿using System;
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

        private string sessionKey = "currentUser";

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
            ViewBag.Controller = RouteData.Values["controller"];
            ViewBag.Action = RouteData.Values["action"];
            ViewBag.CurrentUser = AuthUtils.GetCurrentUser(filterContext.HttpContext);
            base.OnActionExecuting(filterContext);
        }

        private Exception GetException(Exception ex)
        {
            var innerEx = ex.InnerException;
            if (innerEx != null)
            {
                return GetException(innerEx);
            }
            return ex;
        }
        
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 500;
            ViewBag.Exception = GetException(filterContext.Exception);// filterContext.Exception;
            filterContext.Result = View("Error");
        }
    }
}