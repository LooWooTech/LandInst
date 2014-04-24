using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web
{
    public static class AuthUtils
    {
        private const string _cookieName = ".inst_user";

        public static void SaveAuth(this HttpContextBase context, User user)
        {
            var ticket = new FormsAuthenticationTicket(user.UserID.ToString() + "|" + user.Role, true, 60);
            var cookieValue = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(_cookieName, cookieValue);
            context.Response.Cookies.Remove(_cookieName);
            context.Response.Cookies.Add(cookie);
        }

        public static UserIdentity GetCurrentUser(this HttpContextBase context)
        {
            var cookie = context.Request.Cookies.Get(_cookieName);
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
                {
                    var values = ticket.Name.Split('|');

                    var userId = 0;
                    if (int.TryParse(values[0], out userId))
                    {
                        var role = UserRole.Everyone;
                        if (values.Length > 1 && Enum.TryParse<UserRole>(values[1], out role))
                        {
                            return new UserIdentity
                            {
                                UserID = userId,
                                Role = role
                            };
                        }
                    }
                }
            }
            return UserIdentity.Guest;
        }

        public static void ClearAuth()
        {
            FormsAuthentication.SignOut();
        }
    }
}