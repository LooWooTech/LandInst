﻿using System;
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
        public void AddUser(User user)
        {
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
    }
}
