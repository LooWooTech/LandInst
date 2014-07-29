using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public class PracticeManager : ManagerBase
    {
        public PracticeInfo GetPracticeInfo(int memberId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Practices.FirstOrDefault(e => e.MemberID == memberId);
                return entity == null ? null : entity.Data.Convert<PracticeInfo>();
            }
        }

        public void SavePracticeInfo(int memberId, PracticeInfo data)
        {
            if (memberId == 0) return;
            using (var db = GetDataContext())
            {
                var entity = db.Practices.FirstOrDefault(e => e.MemberID == memberId);
                if (entity == null)
                {
                    entity = new Practice
                    {
                        Data = data.ToBytes(),
                        MemberID = memberId
                    };
                    db.Practices.Add(entity);
                }
                else
                {
                    entity.Data = data.ToBytes();
                }
                db.SaveChanges();
            }
        }
    }
}
