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
        public Approval GetApproval(int infoId, ApprovalType type)
        {
            using (var db = GetDataContext())
            {
                return db.Approvals.FirstOrDefault(e => e.InfoID == infoId && e.ApprovalType == type);
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
    }
}
