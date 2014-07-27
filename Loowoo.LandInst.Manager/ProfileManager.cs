using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Manager
{
    public class ProfileManager : ManagerBase
    {
        public T GetProfile<T>(int profileId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Profiles.FirstOrDefault(e => e.ID == profileId);
                if (entity == null)
                    return default(T);
                return entity.Convert<T>();
            }
        }

        public T GetLastProfile<T>(int userId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Profiles.FirstOrDefault(e => e.UserID == userId);
                if (entity == null)
                    return default(T);
                return entity.Convert<T>();
            }
        }

        internal int AddProfile<T>(int userId, T profile)
        {
            using (var db = GetDataContext())
            {
                var entity = new Profile
                {
                    UserID = userId,
                    Data = profile.ToBytes(),
                    CreateTime = DateTime.Now
                };
                db.Profiles.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        internal void UpdateProfile<T>(int profileId, T profile)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Profiles.FirstOrDefault(e => e.ID == profileId);
                if (entity != null)
                {
                    entity.Data = profile.ToBytes();
                    entity.UpdateTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
    }

    public static class ProfileExtension
    {
        public static T Convert<T>(this Profile model)
        {
            if (model != null && model.Data != null)
            {
                return Encoding.UTF8.GetString(model.Data).ToObject<T>();
            }
            return default(T);
        }

        public static byte[] ToBytes<T>(this T data)
        {
            return Encoding.UTF8.GetBytes(data.ToJson());
        }
    }
}
