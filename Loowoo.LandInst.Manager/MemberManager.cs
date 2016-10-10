using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
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

        public List<VCheckMember> GetVCheckMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckMembers.AsQueryable().GetCheckBaseQuery(filter);

                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    query = query.Where(e => e.InstID == filter.InstID.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        public Member GetMember(string memberName, int instId)
        {
            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.RealName == memberName && e.InstID == instId);
            }
        }

        public int AddMember(Member member)
        {
            using (var db = GetDataContext())
            {
                db.Members.Add(member);
                db.SaveChanges();
                return member.ID;
            }
        }

        internal IQueryable<VCheckMember> GetVCheckMembers(IQueryable<VCheckMember> query, CheckLogFilter filter)
        {
            query = query.GetCheckBaseQuery(filter);

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(e => e.RealName.Contains(filter.Keyword.Trim()));
            }


            return query;
        }
    }
}
