using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class MemberManager : ManagerBase
    {
        public void AddMember(Member member)
        {
            using (var db = GetDataContext())
            {
                db.Members.Add(member);
                db.SaveChanges();
            }

            AddProfile(member);
        }

        private void AddProfile(Member member)
        {
            var profile = new MemberProfile
            {
                MemberID = member.ID,
            };
            Core.InfoDataManager.Add(new InfoData
            {
                InfoID = member.ID,
                InfoType = InfoType.MemberProfile,
                Data = profile.ToBytes(),
                Status = InfoStatus.Draft,

            });
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


        public MemberProfile GetProfile(int userId, InfoStatus status = InfoStatus.Normal | InfoStatus.Draft)
        {
            if (userId == 0) return null;

            return Core.InfoDataManager.GetModel<MemberProfile>(userId, InfoType.MemberProfile, status);
        }

        public void SaveProfile(int memberId, MemberProfile profile)
        {
            profile.MemberID = memberId;
            Core.InfoDataManager.Update(new InfoData
            {
                InfoID = profile.MemberID,
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
