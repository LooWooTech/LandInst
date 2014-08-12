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
    public class InstitutionManager : ManagerBase
    {
        private string _cacheKey = "InstNames";
        private Dictionary<int, string> GetInstNames()
        {
            return CacheHelper.GetOrSet(_cacheKey, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.Institutions.Select(e => new { e.ID, e.Name }).ToDictionary(e => e.ID, e => e.Name);
                }
            });
        }

        internal string GetInstName(int instId)
        {
            var names = GetInstNames();
            return names.ContainsKey(instId) ? names[instId] : null;
        }

        private void ClearCache()
        {
            CacheHelper.Remove(_cacheKey);
        }

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
                var query = db.VCheckInsts.Where(e => e.CheckType == filter.CheckType);
                if (!String.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.InstName.Contains(filter.Keyword));
                }
                if (filter.Result.HasValue)
                {
                    query = query.Where(e => e.Result == filter.Result.Value);
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
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
                    query = query.Where(e => e.Name.Contains(filter.Keyword));
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    query = query.Where(e => e.City == filter.City);
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        public void LogoutInstitution(string name)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.Name.ToLower() == name.ToLower());
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
                ClearCache();
            }
        }

        public void ApprovalInst(CheckLog checkLog)
        {
            var profile = Core.ProfileManager.GetLastProfile(checkLog.UserID, null);
            if (checkLog.Result == true)
            {
                UpdateInst(checkLog.UserID, profile.Data.Convert<InstitutionProfile>());
            }
            Core.ProfileManager.UpdateProfileCheckResult(profile.ID, checkLog.Result);
        }

        public void UpdateInst(int instId, Institution model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.ID == instId);
                if (entity != null)
                {
                    model.ID = entity.ID;
                    db.Entry(entity).CurrentValues.SetValues(model);
                }
                else
                {
                    throw new ArgumentException("没找到该机构");
                }
                db.SaveChanges();
                ClearCache();
            }
        }

        public InstitutionProfile GetProfile(int instId, bool? checkResult = null)
        {
            return Core.ProfileManager.GetLastProfile<InstitutionProfile>(instId, checkResult);
        }

        public InstitutionProfile GetProfile(CheckLog checkLog)
        {
            if (checkLog == null) return null;

            var profileId = Core.ProfileManager.GetProfileId(checkLog.ID);

            return Core.ProfileManager.GetProfile<InstitutionProfile>(profileId);
        }

        public void SubmitProfile(int instId, InstitutionProfile profile)
        {
            //如果当前已经提交了资料变更申请，则只更新资料
            var checkLog = Core.CheckLogManager.GetLastLog(instId, CheckType.Profile);
            if (checkLog != null && !checkLog.Result.HasValue)
            {
                Core.ProfileManager.UpdateProfile(checkLog.InfoID, profile);
                return;
            }

            var inst = GetInstitution(instId);
            var annualCheck = Core.AnnualCheckManager.GetIndateModel();
            //不是注册登记 并且 当前处于年检时或当前没有资料变更的提交
            if (inst.Status != InstitutionStatus.Normal && annualCheck != null)
            {
                checkLog = Core.CheckLogManager.GetCheckLog(annualCheck.ID, instId, CheckType.Annual);
                //如果没有申请年检或年检没有通过，则重新提交年检
                if (checkLog == null || checkLog.Result == false)
                {
                    var checkLogId = Core.CheckLogManager.AddCheckLog(annualCheck.ID, instId, CheckType.Annual);
                    var profileId = SaveProfile(instId, profile);
                    Core.ProfileManager.SaveCheckProfile(checkLogId, profileId);
                }
                else
                {
                    SaveProfile(instId, profile);
                }
            }
            else
            {
                if (checkLog == null || checkLog.Result.HasValue)
                {
                    var profileId = SaveProfile(instId, profile);
                    var checkLogId = Core.CheckLogManager.AddCheckLog(profileId, instId, CheckType.Profile);
                    Core.ProfileManager.SaveCheckProfile(checkLogId, profileId);
                }
            }
        }

        public int SaveProfile(int instId, InstitutionProfile profile)
        {
            var draftProfile = Core.ProfileManager.GetLastProfile(instId);
            if (draftProfile == null || draftProfile.CheckResult.HasValue)
            {
                return Core.ProfileManager.AddProfile(instId, profile);
            }

            Core.ProfileManager.UpdateProfile(draftProfile.ID, profile);
            return draftProfile.ID;
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

        public IEnumerable<CheckLog> GetProfileHistory(int instId)
        {
            return Core.CheckLogManager.GetList(instId)
                .Where(e => e.CheckType == CheckType.Profile || e.CheckType == CheckType.Annual)
                .ToList();
        }
    }
}
