﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web
{
    public class UserPrincipal : IPrincipal
    {
        public UserPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }

    public class UserIdentity : System.Security.Principal.IIdentity
    {
        public static UserIdentity Guest = new UserIdentity();

        public int UserID { get; set; }

        public UserRole Role { get; set; }

        public string Username { get; set; }

        public string AuthenticationType
        {
            get { return "Web.Session"; }
        }

        public string Name
        {
            get { return Username; }
        }

        public bool IsAuthenticated
        {
            get { return UserID > 0; }
        }
    }
}