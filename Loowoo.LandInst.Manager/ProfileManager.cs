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

        public Profile GetLastProfile(int userId, bool? checkResult = null)
        {
            using (var db = GetDataContext())
            {
                var query = db.Profiles.Where(e => e.UserID == userId);
                if (checkResult.HasValue)
                {
                    query = query.Where(e => e.CheckResult == checkResult.Value);
                }
                return query.OrderByDescending(e => e.ID).FirstOrDefault();
            }
        }

        public T GetLastProfile<T>(int userId, bool? checkResult = null)
        {
            var entity = GetLastProfile(userId, checkResult);
            return entity == null ? default(T) : entity.Convert<T>();
        }

        internal int AddProfile<T>(int userId, T profile)
        {
            using (var db = GetDataContext())
            {
                var entity = new Profile
                {
                    UserID = userId,
                    Json = profile.ToJson(),
                    CreateTime = DateTime.Now,
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
                    entity.Json = profile.ToJson();
                    entity.UpdateTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }

        internal void UpdateProfileCheckResult(int profileId, bool? checkResult)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Profiles.FirstOrDefault(e => e.ID == profileId);
                if (entity != null)
                {
                    entity.CheckResult = checkResult;
                    entity.UpdateTime = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }

        //internal void SaveCheckProfile(int checkLogId, int profileId)
        //{
        //    using (var db = GetDataContext())
        //    {
        //        var entity = db.CheckProfiles.FirstOrDefault(e => e.CheckLogID == checkLogId && e.ProfileID == profileId);
        //        if (entity == null)
        //        {
        //            db.CheckProfiles.Add(new CheckProfile
        //            {
        //                ProfileID = profileId,
        //                CheckLogID = checkLogId,
        //            });
        //            db.SaveChanges();
        //        }
        //    }
        //}

        //internal int GetProfileId(int checkLogId)
        //{
        //    using (var db = GetDataContext())
        //    {
        //        var entity = db.CheckProfiles.FirstOrDefault(e => e.CheckLogID == checkLogId);
        //        return entity == null ? 0 : entity.ProfileID;
        //    }
        //}
    }
}