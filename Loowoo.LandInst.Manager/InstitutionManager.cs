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


        public List<VCheckInst> GetApprovalInsts(InstitutionFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalInsts.Where(e => e.CheckType == filter.ApprovalType);
                if (!String.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.InstName.Contains(filter.Keyword));
                }
                if (filter.ApprovalResult.HasValue)
                {
                    query = query.Where(e => e.Result == filter.ApprovalResult.Value);
                }

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }

        public List<Institution> GetInstitutions(InstitutionFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Institutions.AsQueryable();
                if (filter.InstId.HasValue && filter.InstId.Value > 0)
                {
                    query = query.Where(e => e.ID == filter.InstId);
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.Name.Contains(filter.Keyword) || e.FullName.Contains(filter.Keyword));
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter).ToList();
            }
        }

        public void AddShareholder(int instId, Shareholder shareholder)
        {
            //Core.InfoDataManager.UpdateListItem(instId, InfoType.Shareholder, shareholder);
        }

        public void DeleteShareholder(int instId, string shareholderId)
        {
            //Core.InfoDataManager.DeleteListItem<Shareholder, string>(instId, InfoType.Shareholder,  shareholderId, e => e.ID);
        }

        public void AddCertification(int instId, Certification certification)
        {
            //Core.InfoDataManager.UpdateListItem(instId, InfoType.Certificatoin, certification);
        }

        public void DeleteCertification(int instId, string certificationId)
        {
            //Core.InfoDataManager.DeleteListItem<Certification, string>(instId, InfoType.Certificatoin, certificationId, e => e.ID);
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

        public InstitutionProfile GetProfile(int instId, bool? checkResult = null)
        {
            var approval = Core.CheckLogManager.GetLastLog(instId, CheckType.Profile, checkResult);
            if (approval == null) return null;
            return Core.ProfileManager.GetProfile<InstitutionProfile>(approval.InfoID);
        }


        public void SubmitProfile(int instId, InstitutionProfile profile)
        {
            var approval = Core.CheckLogManager.GetCheckLog(profile.ID, instId, CheckType.Profile);
            //如果没有提交过资料变更或者资料变更被审核过，则均可以重新提交
            if (approval == null || approval.Result.HasValue)
            {
                var profileId = Core.ProfileManager.AddProfile(instId, profile);
                Core.CheckLogManager.AddCheckLog(profileId, instId, CheckType.Profile);
            }
            else if (approval != null)//如果没被审核，则可以重复覆盖所提交的内容
            {
                Core.ProfileManager.UpdateProfile(approval.InfoID, profile);
            }
        }



        public void UpdateStatus(int instId, InstitutionStatus status)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.ID == instId);
                if (entity != null)
                {
                    entity.Status = status;
                    db.SaveChanges();
                }
            }
        }

        //public List<Shareholder> GetShareHolders(int id)
        //{
        //    return Core.InfoDataManager.GetModel<List<Shareholder>>(id, CheckType.Shareholder) ?? new List<Shareholder>();
        //}

        //public List<Certification> GetCertifications(int id)
        //{
        //    return Core.InfoDataManager.GetModel<List<Certification>>(id, CheckType.Certificatoin) ?? new List<Certification>();
        //}
    }
}
