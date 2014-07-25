using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class CheckLogManager : ManagerBase
    {
        public CheckLog GetCheckLog(int logId)
        {
            using (var db = GetDataContext())
            {
                return db.CheckLogs.FirstOrDefault(e => e.ID == logId);
            }
        }

        public CheckLog GetLastLog(int userId, CheckType type, bool? result = null)
        {
            using (var db = GetDataContext())
            {
                var query = db.CheckLogs.Where(e => e.UserID == userId && e.CheckType == type);
                if (result.HasValue)
                {
                    query = query.Where(e => e.Result.Value == result.Value);
                }
                return query.OrderByDescending(e => e.CreateTime).FirstOrDefault();
            }
        }

        public CheckLog GetCheckLog(int infoId, int userId, CheckType type)
        {
            using (var db = GetDataContext())
            {
                return db.CheckLogs.OrderByDescending(e => e.CreateTime).FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.CheckType == type);
            }
        }

        public List<CheckLog> GetList(int infoId)
        {
            using (var db = GetDataContext())
            {
                return db.CheckLogs.Where(e => e.InfoID == infoId).OrderByDescending(e => e.CreateTime).ToList();
            }
        }

        public void UpdateCheckLog(int approvalId, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.ID == approvalId);
                if (entity == null) return;
                entity.UpdateTime = DateTime.Now;
                entity.Result = result;
                db.SaveChanges();
            }
        }

        public int AddCheckLog(int infoId, int userId, CheckType type)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.CheckType == type);
                if (entity != null)
                {
                    if (entity.UpdateTime.HasValue)
                    {
                        return entity.ID;
                    }
                }
                else
                {
                    entity = new CheckLog
                    {
                        InfoID = infoId,
                        UserID = userId,
                        CheckType = type,
                        CreateTime = DateTime.Now
                    };
                    db.CheckLogs.Add(entity);
                }
                db.SaveChanges();
                return entity.ID;
            }
        }
    }
}
