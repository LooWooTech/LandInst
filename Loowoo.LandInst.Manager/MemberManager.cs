using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class MemberManager : ManagerBase
    {
        public void AddMember(User user, Member member)
        {
            Core.UserManager.AddUser(user);
            using (var db = GetDataContext())
            {
                member.ID = user.ID;
                db.Members.Add(member);
                db.SaveChanges();
            }
        }

        public void UpdateMember(Member member)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == member.ID);
                if (entity == null)
                {
                    throw new ArgumentException("Member.ID");
                }
                db.Entry(entity).CurrentValues.SetValues(member);
                db.SaveChanges();
            }
        }

        public Member GetMember(int userId)
        {
            if (userId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.ID == userId);
            }
        }


        public MemberProfile GetProfile(int userId, InfoStatus status = InfoStatus.Normal)
        {
            if (userId == 0) return null;

            return Core.InfoDataManager.GetModel<MemberProfile>(userId, InfoType.MemberProfile, status);
        }

        public void SaveProfile(MemberProfile profile)
        {
            Core.InfoDataManager.Update(new InfoData
            {
                InfoID = profile.ID,
                InfoType = InfoType.MemberProfile,
                Data = profile.ToBytes(),
                Status = InfoStatus.Draft
            });
        }

        public List<Member> GetMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.AsQueryable();
                if (filter.InstID.HasValue)
                {
                    query = query.Where(e => e.InstitutionID == filter.InstID.Value);
                }

                if (!string.IsNullOrEmpty(filter.LikeName))
                {
                    query = query.Where(e => e.RealName.Contains(filter.LikeName));
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                return query.SetPage(filter).ToList();
            }
        }
    }
}
