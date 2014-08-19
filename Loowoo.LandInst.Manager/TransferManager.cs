using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public class TransferManager : ManagerBase
    {
        public void Submit(Member member, int targetInstId, TransferMode mode)
        {
            var checkLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Transfer);
            if (checkLog == null || checkLog.Checked)
            {
                var transferId = Core.TransferManager.AddTransfer(new Transfer
                {
                    MemberID = member.ID,
                    CurrentInstID = member.InstitutionID,
                    TargetInstID = targetInstId,
                    Mode = mode
                });
                Core.CheckLogManager.AddCheckLog(transferId, member.ID, CheckType.Transfer);
            }

        }

        public Transfer GetTransfer(int id)
        {
            using (var db = GetDataContext())
            {
                return db.Transfers.FirstOrDefault(e => e.ID == id);
            }
        }

        public int AddTransfer(Transfer model)
        {
            using (var db = GetDataContext())
            {
                if (model.ID > 0) return model.ID;
                db.Transfers.Add(model);
                db.SaveChanges();
            }
            return model.ID;
        }

        public void Approval(int checkLogId)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            //已经通过审核
            if (checkLog.Checked && checkLog.Result.Value)
            {
                var transfer = GetTransfer(checkLog.InfoID);
                var member = Core.MemberManager.GetMember(transfer.MemberID);
                member.InstitutionID = transfer.TargetInstID;
                Core.MemberManager.UpdateMember(member);
            }
        }

        public List<VCheckTransfer> GetVCheckTransfers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckTransfers.AsQueryable();
                if (filter.InfoID.HasValue && filter.InfoID.Value > 0)
                {
                    query = query.Where(e => e.InfoID == filter.InfoID.Value);
                }

                if (filter.UserID.HasValue && filter.UserID.Value > 0)
                {
                    query = query.Where(e => e.UserID == filter.UserID.Value);
                }

                if (!String.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                if (filter.InstID.HasValue)
                {
                    query = query.Where(e => e.CurrentInstID == filter.InstID.Value);
                }

                var list = query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();

                foreach (var item in list)
                {
                    item.CurrentInstName = Core.InstitutionManager.GetInstName(item.CurrentInstID);
                    item.TargetInstName = Core.InstitutionManager.GetInstName(item.TargetInstID);
                }

                return list;
            }
        }

        public void TransferMember(int transferId, Member member)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Transfers.FirstOrDefault(e => e.ID == transferId);
                if (entity != null)
                {
                    entity.UpdateTime = DateTime.Now;
                    db.SaveChanges();

                    member.InstitutionID = entity.TargetInstID;
                    Core.MemberManager.UpdateMember(member);
                }
            }
        }
    }
}
