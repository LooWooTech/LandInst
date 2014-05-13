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

        public List<VInstAnnualCheck> GetInstAnnualChecks(int instId)
        {
            using (var db = GetDataContext())
            {
                return db.VInstAnnualChecks.Where(e => (e.ApprovalType == ApprovalType.Annual && e.UserID == instId) || e.UserID == null).ToList();
            }
        }

        public List<VApprovalAnnualCheck> GetApprovalAnnualChecks(ApprovalFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalAnnualChecks.Where(e => e.ApprovalType == filter.Type);
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.Fullname.Contains(filter.Keyword) || e.InstName.Contains(filter.Keyword));
                }

                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.InfoID == filter.InfoID.Value);
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

        public AnnualCheck GetModel(int annualCheckId)
        {
            using (var db = GetDataContext())
            {
                return db.AnnualChecks.FirstOrDefault(e => e.ID == annualCheckId);
            }
        }
    }
}
