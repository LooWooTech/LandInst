using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public class MemberManager : ManagerBase
    {
        public bool Exist(string idNo, int instId = 0)
        {
            if (string.IsNullOrEmpty(idNo))
            {
                throw new ArgumentException("参数不正确");
            }

            using (var db = GetDataContext())
            {
                if (instId == 0)
                {
                    return db.Members.Any(e => e.IDNo == idNo);
                }
                else
                {
                    return db.Members.Any(e => e.InstID != instId && e.IDNo == idNo);
                }
            }
        }

        public void AddMembers(int instId, IEnumerable<Member> members)
        {
            using (var db = GetDataContext())
            {
                var entities = db.Members.Where(e => e.InstID == instId);
                db.Members.RemoveRange(entities);
                foreach (var member in members)
                {
                    member.InstID = instId;
                    db.Members.Add(member);
                }
                db.SaveChanges();
            }
        }
    }
}
