using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public partial class MemberManager
    {
        public void SubmitTransfer(Member member, int targetInstId)
        {
            var checkLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Transfer);
            if (checkLog == null || checkLog.Checked)
            {
                Core.CheckLogManager.AddCheckLog(member.InstitutionID, member.ID, CheckType.Transfer, targetInstId.ToString());
            }
            else
            {
                checkLog.Data = targetInstId.ToString();
                Core.CheckLogManager.ApprovalCheckLog(checkLog);
            }
        }

        public void ApprovalTransfer(CheckLog checkLog)
        {
            if (checkLog == null) return;
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == checkLog.UserID);
                if (entity == null)
                {
                    throw new ArgumentException("Member.ID");
                }
                var targetInstId = checkLog.DataAsInt();
                if (entity.InstitutionID == targetInstId) return;
                entity.InstitutionID = targetInstId;
                db.SaveChanges();
            }
        }

        public List<VCheckTransfer> GetVCheckTransfers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                filter.CheckType = CheckType.Transfer;

                return Core.MemberManager.GetVCheckMembers(filter).Select(e => new VCheckTransfer
                {
                    VCheck = e,
                    CurrentInstName = Core.InstitutionManager.GetInstName(e.InstitutionID),
                    TargetInstName = Core.InstitutionManager.GetInstName(int.Parse(e.Data))
                }).ToList();
            }
        }
    }
}
