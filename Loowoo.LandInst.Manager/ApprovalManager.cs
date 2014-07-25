using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class ApprovalManager : ManagerBase
    {
        public Approval GetApproval(int approvalId)
        {
            using (var db = GetDataContext())
            {
                return db.Approvals.FirstOrDefault(e => e.ID == approvalId);
            }
        }

        public Approval GetApproval(int infoId, int userId, ApprovalType type)
        {
            using (var db = GetDataContext())
            {
                return db.Approvals.OrderByDescending(e => e.CreateTime).FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.ApprovalType == type);
            }
        }

        public void UpdateApproval(int approvalId, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Approvals.FirstOrDefault(e => e.ID == approvalId);
                if (entity == null) return;
                entity.ApprovalTime = DateTime.Now;
                entity.Result = result;
                db.SaveChanges();
            }
        }

        public int AddApproval(int infoId, int userId, ApprovalType type)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Approvals.FirstOrDefault(e => e.InfoID == infoId && e.UserID == userId && e.ApprovalType == type);
                if (entity != null)
                {
                    if (entity.ApprovalTime.HasValue)
                    {
                        return entity.ID;
                    }
                }
                else
                {
                    entity = new Approval
                    {
                        InfoID = infoId,
                        UserID = userId,
                        ApprovalType = type,
                        CreateTime = DateTime.Now
                    };
                    db.Approvals.Add(entity);
                }
                db.SaveChanges();
                return entity.ID;
            }
        }
    }
}
