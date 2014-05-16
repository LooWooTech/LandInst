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
            var profile = new MemberProfile(member);
            Core.InfoDataManager.Save(profile.ID, InfoType.MemberProfile, profile);
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
            if (memberId == 0) return null;

            return Core.InfoDataManager.GetModel<MemberProfile>(memberId, InfoType.MemberProfile);
        }

        public void SaveProfile(Member member, MemberProfile profile)
        {
            profile.SetMemberField(member);
            Core.InfoDataManager.Save(profile.ID, InfoType.MemberProfile, profile);
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

            var approvalId = Core.ApprovalManager.AddApproval(member.ID, currentInstId, Model.ApprovalType.Transfer);
            var transferData = new TransferData
            {
                ApprovalID = approvalId,
                MemberID = member.ID,
                InstitutionID = currentInstId,
                TargetInstitutionID = targetInstId
            };

            Core.InfoDataManager.Save(transferData.ApprovalID, InfoType.Transfer, transferData);
        }

        public void ApprovalTrasfer(int approvalId)
        { 
            var approval = Core.ApprovalManager.GetApproval(approvalId);
            var transferData = Core.InfoDataManager.GetModel<TransferData>(approval.ID, InfoType.Transfer);
            if (approval.Result.HasValue && approval.Result.Value)
            {
                var member = Core.MemberManager.GetMember(transferData.MemberID);
                member.InstitutionID = transferData.TargetInstitutionID;
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

                return query.OrderByDescending(e => e.ID).SetPage(filter).ToList();
            }
        }

        public List<VApprovalMember> GetApprovalMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalMembers.AsQueryable();

                if (filter.InstID.HasValue)
                {
                    query = query.Where(e => e.InstitutionID == filter.InstID.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter).ToList();
            }
        }
    }
}
