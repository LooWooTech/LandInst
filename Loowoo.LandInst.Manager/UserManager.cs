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
            //var user = GetUser(username);
            //if (user == null)
            //{
            //    throw new ArgumentException("用户名不存在");
            //}

            //if (user.Password != password)
            //{
            //    throw new ArgumentException("密码不正确！");
            //}

            return new User
            {
                ID = 1,
                Username = username,
                Password = password,
                Role = UserRole.Institution
            };
        }

        public User GetUser(string username)
        {
            return new User();
        }

        public User GetUser(int userId)
        {
            return new User();
        }
    }
}
