﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loowoo.LandInst.Web
{
    public class AuthenticateModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler((obj, ea) =>
            {
                var currentUser = new HttpContextWrapper(context.Context).GetCurrentUser();
                context.Context.User = new UserPrincipal(currentUser);
            });
        }
    }
}