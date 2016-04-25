using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Common;
using System.Data.Entity.Core.Objects;

namespace Loowoo.LandInst.Manager
{
    public class UserManager : ManagerBase
    {
        public int AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException("User");

            if (string.IsNullOrEmpty(user.Username)) throw new ArgumentNullException("用户名没有填写");

            if (string.IsNullOrEmpty(user.Password)) throw new ArgumentNullException("密码没有填写");

            using (var db = GetDataContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.Username.ToLower() == user.Username.ToLower());
                if (entity != null)
                {
                    throw new ArgumentException("用户名已存在");
                }
                var pwdCopy = user.Password;
                user.Password = user.Password.MD5();
                db.Users.Add(user);
                db.SaveChanges();
                return user.ID;
            }
        }

        public User GetUser(string username, string password)
        {
            var user = GetUser(username);
            if (user == null)
            {
                throw new ArgumentException("用户名不存在");
            }

            if (user.Password != password.MD5())
            {
                throw new ArgumentException("密码不正确！");
            }

              return user;
        }

        public bool Exists(string username)
        {
            using (var db = GetDataContext())
            {
                return db.Users.Count(e => e.Username.ToLower() == username.ToLower()) > 0;
            }
        }

        public User GetUser(string username)
        {
            using (var db = GetDataContext())
            {
                return db.Users.FirstOrDefault(e => e.Username.ToLower() == username.ToLower());
            }
        }

        public User GetUser(int userId)
        {
            using (var db = GetDataContext())
            {
                return db.Users.FirstOrDefault(e => e.ID == userId);
            }
        }

        public void UpdateLogin(User user)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == user.ID);
                if (entity != null)
                {
                    entity.LastLoginIP = user.LastLoginIP;
                    entity.LastLoginTime = DateTime.Now;
                    entity.LoginTimes++;

                    db.SaveChanges();
                }
            }
        }

        public void ResetPwd(int userId, string newPwd)
        {
            if (string.IsNullOrEmpty(newPwd))
            {
                throw new ArgumentNullException("新密码没有填写");
            }

            if (userId == 0)
            {
                throw new ArgumentNullException("没有找到该用户");
            }


            using (var db = GetDataContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == userId);
                if (entity == null)
                {
                    throw new ArgumentNullException("没有找到该用户");
                }
                entity.Password = newPwd.MD5();
                db.SaveChanges();
            }
        }

        public bool ValidateQuestion(string userName, string answer)
        {
            var user = GetUser(userName);
            return user != null && user.Answer == answer;
        }

        public void UpdatePassword(int userId, string newPwd)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == userId);
                entity.Password = newPwd.MD5();
                db.SaveChanges();
            }
        }

        internal void UpdateUsername(int userId, string username)
        {
            if (userId == 0 || string.IsNullOrEmpty(username))
            {
                throw new Exception("没找到这个用户");
            }

            using (var db = GetDataContext())
            {
                var entity = db.Users.FirstOrDefault(e => e.ID == userId);
                if (entity.Username == username) return;
                if (db.Users.Any(e => e.Username == username))
                {
                    throw new ArgumentException("用户名已被占用");
                }
                entity.Username = username;
                db.SaveChanges();
            }
        }
    }
}
