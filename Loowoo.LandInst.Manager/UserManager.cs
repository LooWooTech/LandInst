using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Manager
{
    public class UserManager
    {
        public void AddUser(User user)
        {

        }

        public User GetUser(string username, string password)
        {
            return new User
            {
                ID = 1,
                Username = username,
                Password = password,
                Role = UserRole.Institution
            };
        }

    }
}
