using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Manager
{
    public class ApprovalManager
    {
        public Approval GetApproval(int infoId, ApprovalType type)
        {
            return new Approval();
        }

        public void UpdateApproval(int infoId, ApprovalType type, bool result)
        {
            var entity = GetApproval(infoId, type);
            entity.CheckTime = DateTime.Now;
            entity.Result = result;
            //update db
        }
    }
}
