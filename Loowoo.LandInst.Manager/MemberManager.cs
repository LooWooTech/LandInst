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

            //AddProfile(member);
        }

        private void AddProfile(Member member)
        {
            var profile = new MemberProfile(member);
            Core.ProfileManager.AddProfile(member.ID, profile);
        }

        public void UpdateMemberStatus(int memberId, MemberStatus status)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == memberId);
                entity.Status = status;
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

        public Member GetMember(int memberId)
        {
            if (memberId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.ID == memberId);
            }
        }


        public MemberProfile GetProfile(int memberId)
        {
            return Core.ProfileManager.GetLastProfile<MemberProfile>(memberId);
            //if (memberId == 0) return null;
            //var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Profile, checkResult);
            //if (checkLog == null)
            //    return null;
            //return Core.ProfileManager.GetProfile<MemberProfile>(checkLog.InfoID);
        }

        public void SaveProfile(Member member, MemberProfile profile)
        {
            profile.SetMemberField(member);

            var entity = Core.ProfileManager.GetLastProfile<MemberProfile>(member.ID);
            if (entity == null)
            {
                Core.ProfileManager.AddProfile(member.ID, profile);
            }
            else
            {
                Core.ProfileManager.UpdateProfile(entity.ID, profile);
            }
        }


        public void Transfer(Member member, int targetInstId, TransferMode mode)
        {
            var currentInstId = mode == TransferMode.In ? targetInstId : member.InstitutionID;
            if (mode == TransferMode.In)
            {
                if (member.InstitutionID == currentInstId)
                {
                    return;
                }
            }

            if (mode == TransferMode.Out)
            {
                if (targetInstId == currentInstId)
                {
                    return;
                }
            }

            var checkLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Transfer);
            if (checkLog == null || checkLog.Checked)
            {
                var transferId = AddTransfer(member.ID, currentInstId, targetInstId);
                Core.CheckLogManager.AddCheckLog(transferId, member.ID, CheckType.Transfer);
            }

        }

        private Transfer GetTransfer(int id)
        {
            using (var db = GetDataContext())
            {
                return db.Transfers.FirstOrDefault(e => e.ID == id);
            }
        }

        private int AddTransfer(int memberId, int currentInstId, int targetInstId)
        {
            var entity = new Transfer
            {
                MemberID = memberId,
                CurrentInstID = currentInstId,
                TargetInstID = targetInstId
            };
            using (var db = GetDataContext())
            {
                db.Transfers.Add(entity);
                db.SaveChanges();
            }
            return entity.ID;
        }

        public void ApprovalTrasfer(int checkLogId)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            //已经通过审批
            if (checkLog.Checked && checkLog.Result.Value)
            {
                var transfer = GetTransfer(checkLog.InfoID);
                var member = GetMember(transfer.MemberID);
                member.InstitutionID = transfer.TargetInstID;
                Core.MemberManager.UpdateMember(member);
            }
        }

        public List<Member> GetInstMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.Where(e => e.InstitutionID == filter.InstID || e.InstitutionID == 0);
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        public List<VCheckMember> GetVCheckMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = GetVCheckMembers(db.VCheckMembers, filter);
                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }
                return query.OrderByDescending(e => e.CreateTime).SetPage(filter.Page).ToList();
            }
        }

        internal IQueryable<VCheckMember> GetVCheckMembers(IQueryable<VCheckMember> query, CheckLogFilter filter)
        {
            query = query.Where(e => e.CheckType == filter.Type);
            if (filter.Result.HasValue)
            {
                query = query.Where(e => e.Result == filter.Result.Value);
            }
            if (filter.InfoID.HasValue && filter.InfoID.Value > 0)
            {
                query = query.Where(e => e.InfoID == filter.InfoID.Value);
            }
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(e => e.RealName.Contains(filter.Keyword.Trim()));
            }
            if (filter.UserID.HasValue && filter.UserID.Value > 0)
            {
                query = query.Where(e => e.UserID == filter.UserID.Value);
            }
            return query;
        }
    }
}
