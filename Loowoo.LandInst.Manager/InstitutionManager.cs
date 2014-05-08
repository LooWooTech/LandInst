using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class InstitutionManager : ManagerBase
    {

        public Institution GetInstitution(int id)
        {
            using (var db = GetDataContext())
            {
                return db.Institutions.FirstOrDefault(e => e.ID == id);
            }
        }


        public List<VApprovalInst> GetApprovalInsts(InstitutionFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalInsts.Where(e => e.ApprovalType == filter.ApprovalType);
                if (!String.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.FullName.Contains(filter.Keyword) || e.Name.Contains(filter.Keyword));
                }
                if (filter.ApprovalResult.HasValue)
                {
                    query = query.Where(e => e.ApprovalResult == filter.ApprovalResult.Value);
                }

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }

        //public List<Institution> GetInstitutions(InstitutionFilter filter)
        //{
        //    using (var db = GetDataContext())
        //    {
        //        var query = db.Institutions.AsQueryable();
        //        if (filter.InstId.HasValue && filter.InstId.Value > 0)
        //        {
        //            query = query.Where(e => e.ID == filter.InstId);
        //        }

        //        if (filter.Status.HasValue)
        //        {
        //            query = query.Where(e => e.Status == filter.Status.Value);
        //        }

        //        if (!string.IsNullOrEmpty(filter.Keyword))
        //        {
        //            query = query.Where(e => e.Name.Contains(filter.Keyword) || e.FullName.Contains(filter.Keyword));
        //        }

        //        return query.OrderByDescending(e => e.ID).SetPage(filter).ToList();
        //    }
        //}

        public void AddShareholder(int instId, Shareholder shareholder)
        {
            Core.InfoDataManager.UpdateListItem(instId, InfoType.Shareholder, InfoStatus.Normal, shareholder);
        }

        public void DeleteShareholder(int instId, string shareholderId)
        {
            Core.InfoDataManager.DeleteListItem<Shareholder, string>(instId, InfoType.Shareholder, InfoStatus.Normal, shareholderId, e => e.ID);
        }

        public void AddCertification(int instId, Certification certification)
        {
            Core.InfoDataManager.UpdateListItem(instId, InfoType.Certificatoin, InfoStatus.Normal, certification);
        }

        public void DeleteCertification(int instId, string certificationId)
        {
            Core.InfoDataManager.DeleteListItem<Certification, string>(instId, InfoType.Certificatoin, InfoStatus.Normal, certificationId, e => e.ID);
        }

        public void LogoutInstitution(string name)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.FullName.ToLower() == name.ToLower());
                if (entity == null)
                {
                    throw new ArgumentException("没找到该全称的公司");
                }
                entity.Status = InstitutionStatus.Logout;
                db.SaveChanges();
            }
        }

        public InstitutionProfile GetProfile(int instId, InfoStatus status = InfoStatus.Normal)
        {
            return Core.InfoDataManager.GetModel<InstitutionProfile>(instId, InfoType.InstitutionProfile, status);
        }

        public void AddInstitution(Institution model)
        {
            using (var db = GetDataContext())
            {
                db.Institutions.Add(model);
                db.SaveChanges();
            }
        }

        public void UpdateInstitution(Institution model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.ID == model.ID);
                if (entity != null)
                {
                    db.Entry(entity).CurrentValues.SetValues(model);
                }
                else
                {
                    throw new ArgumentException("没找到该机构");
                }
                db.SaveChanges();
            }
        }

        public void SaveProfile(InstitutionProfile profile, InfoStatus status = InfoStatus.Draft)
        {
            Core.InfoDataManager.Update(new InfoData
            {
                InfoID = profile.ID,
                InfoType = InfoType.InstitutionProfile,
                Status = status,
                Data = profile.ToBytes(),
            });
        }
    }
}
