using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public class PracticeManager : ManagerBase
    {
        public int GetInstId(int practiceId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.OrderByDescending(e => e.ID).FirstOrDefault(e => e.ID == practiceId);
                return entity == null ? 0 : entity.InstID;
            }
        }

        public PracticeInfo GetPracticeInfo(int practiceId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.OrderByDescending(e => e.ID).FirstOrDefault(e => e.ID == practiceId);
                return entity == null ? null : entity.Data.Convert<PracticeInfo>();
            }
        }

        public PracticeInfo GetPracticeInfo(int memberId, int instId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.OrderByDescending(e => e.ID).FirstOrDefault(e => e.MemberID == memberId && e.InstID == instId);
                return entity == null ? null : entity.Data.Convert<PracticeInfo>();
            }
        }

        public void UpdatePracticeInfo(int id, PracticeInfo data)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    entity.Data = data.ToBytes();
                    db.SaveChanges();
                }
            }
        }

        public int AddPracticeInfo(int memberId, int instId, PracticeInfo data)
        {
            if (memberId == 0) return 0;
            using (var db = GetDataContext())
            {
                var entity = new Practice
                {
                    Data = data.ToBytes(),
                    MemberID = memberId,
                    InstID = instId
                };

                db.Practices.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }
    }
}
