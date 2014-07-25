using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class AnnualCheckManager : ManagerBase
    {
        public void Save(AnnualCheck model)
        {
            using (var db = GetDataContext())
            {
                if (model.ID > 0)
                {
                    var entity = db.AnnualChecks.FirstOrDefault(e => e.ID == model.ID);
                    if (entity != null)
                    {
                        db.Entry(entity).CurrentValues.SetValues(model);
                    }
                }
                else
                {
                    db.AnnualChecks.Add(model);
                }
                db.SaveChanges();
            }
        }

        public List<AnnualCheck> GetInstAnnualChecks(int instId)
        {
            var list = GetAnnualChecks();
            foreach (var annual in list)
            {
                annual.Approval = Core.CheckLogManager.GetCheckLog(annual.ID, instId, CheckType.Annual);
            }
            return list;
        }

        public List<VCheckAnnual> GetApprovalAnnualChecks(ApprovalFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalAnnualChecks.Where(e => e.CheckType == filter.Type);
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.InstName.Contains(filter.Keyword));
                }

                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.ID == filter.InfoID.Value);
                }

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }

        public List<AnnualCheck> GetAnnualChecks()
        {
            using (var db = GetDataContext())
            {
                return db.AnnualChecks.OrderByDescending(e => e.ID).ToList();
            }
        }

        public AnnualCheck GetModel(int id)
        {
            if (id == 0)
            {
                return null;
            }
            using (var db = GetDataContext())
            {
                return db.AnnualChecks.FirstOrDefault(e => e.ID == id);
            }
        }
    }
}
