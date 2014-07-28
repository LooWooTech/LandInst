using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Manager
{
    public class AnnualCheckManager : ManagerBase
    {
        private string _cacheKey = "annualchecks";
        public List<AnnualCheck> GetAnnualChecks()
        {
            return CacheHelper.GetOrSet(_cacheKey, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.AnnualChecks.OrderByDescending(e => e.ID).ToList();
                }
            });
        }

        public AnnualCheck GetModel(int id)
        {
            return GetAnnualChecks().FirstOrDefault(e => e.ID == id);
        }

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

        private string GetAnnualCheckName(int annualCheckId)
        {
            var model = GetModel(annualCheckId);
            return model == null ? null : model.Name;
        }

        public List<VCheckAnnual> GetVCheckAnnual(CheckLogFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckInsts.Where(e => e.CheckType == CheckType.Annual);

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.InstName.Contains(filter.Keyword) || e.FullName.Contains(filter.Keyword));
                }

                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.ID == filter.InfoID.Value);
                }

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter.Page).ToList().Select(e => new VCheckAnnual
                {
                    AnnualCheckID = e.InfoID,
                    AnnualCheckName = GetAnnualCheckName(e.InfoID),
                    VCheckInst = e
                }).ToList();
            }
        }

        public AnnualCheck GetIndateModel()
        {
            var now =DateTime.Now;
            return GetAnnualChecks().FirstOrDefault(e => e.StartDate <= now && e.EndDate >= now);
        }

        public List<AnnualCheck> GetInstAnnualChecks(int instId)
        {
            var list = GetAnnualChecks();
            var now = DateTime.Now;
            foreach (var item in list)
            {
                if (item.StartDate <= now && item.EndDate >= now)
                {
                    item.Approval = Core.CheckLogManager.GetCheckLog(item.ID, instId, CheckType.Annual);
                }
            }
            return list;
        }
    }
}
