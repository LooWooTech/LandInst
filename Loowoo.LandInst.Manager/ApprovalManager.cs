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
                return db.Approvals.FirstOrDefault(e => e.InfoID == infoId && e.Type == type);
            }
        }

        public void UpdateApproval(int infoId, ApprovalType type, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Approvals.FirstOrDefault(e => e.InfoID == infoId && e.Type == type);
                if (entity == null) return;
                entity.ApprovalTime = DateTime.Now;
                entity.Result = result;
                db.SaveChanges();
            }
        }
    }
}
