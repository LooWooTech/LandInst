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

        public CheckLog GetLastLog(int userId, CheckType? checkType = null, bool? result = null)
        {
            using (var db = GetDataContext())
            {
                var query = db.CheckLogs.Where(e => e.UserID == userId);
                if (checkType.HasValue)
                {
                    query = query.Where(e => e.CheckType == checkType.Value);
                }
                if (result.HasValue)
                {
                    query = query.Where(e => e.Result.Value == result.Value);
                }
                return query.OrderByDescending(e => e.ID).FirstOrDefault();
            }
        }

        public CheckLog GetCheckLog(int infoId, int userId, CheckType type)
        {
            using (var db = GetDataContext())
            {
                return db.CheckLogs.OrderByDescending(e => e.ID).FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.CheckType == type);
            }
        }

        public void Delete(int id)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    db.CheckLogs.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public List<CheckLog> GetList(int userId, CheckType? type = null)
        {
            using (var db = GetDataContext())
            {
                var query = db.CheckLogs.Where(e => e.UserID == userId);
                if (type.HasValue)
                {
                    query = query.Where(e => e.CheckType == type.Value);
                }
                return query.OrderByDescending(e => e.ID).ToList();
            }
        }

        public void ApprovalCheckLog(CheckLog model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.ID == model.ID);
                if (entity == null) return;
                entity.UpdateTime = DateTime.Now;
                entity.Result = model.Result;
                entity.Note = model.Note;
                db.SaveChanges();
            }
        }

        public int AddCheckLog(int infoId, int userId, CheckType type, string extendData = null, bool? result = null)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.CheckType == type);
                if (entity != null && !entity.Result.HasValue)
                {
                    entity.Data = extendData;
                    entity.Result = result;
                    db.SaveChanges();
                    return entity.ID;
                }
                else
                {
                    entity = new CheckLog
                    {
                        InfoID = infoId,
                        UserID = userId,
                        CheckType = type,
                        Data = extendData,
                        Result = result
                    };
                    if (result.HasValue)
                    {
                        entity.UpdateTime = DateTime.Now;
                    }
                    db.CheckLogs.Add(entity);
                }
                db.SaveChanges();
                return entity.ID;
            }
        }
    }
}