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

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter.Page).ToList();
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
                ClearCache();
            }
        }

        public void UpdateInstitution(InstitutionProfile model)
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
                ClearCache();
            }
        }

        public InstitutionProfile GetLastProfile(int instId)
        {
            return Core.ProfileManager.GetLastProfile<InstitutionProfile>(instId);
        }

        public InstitutionProfile GetCheckProfile(CheckLog checkLog)
        {
            if (checkLog == null) return null;

            if (checkLog.CheckType == CheckType.Profile)
            {
                return Core.ProfileManager.GetProfile<InstitutionProfile>(checkLog.InfoID);
            }
            else
            {
                return GetLastProfile(checkLog.UserID);
            }
        }

        //public InstitutionProfile GetProfile(int instId, CheckLog checkLog)
        //{
        //    if (checkLog == null) return Core.ProfileManager.GetLastProfile<InstitutionProfile>(instId);
        //    return Core.ProfileManager.GetProfile<InstitutionProfile>(checkLog.InfoID);
        //}

        public int SaveProfile(int instId, InstitutionProfile profile)
        {
            return Core.ProfileManager.AddProfile(instId, profile);
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
                if (checkLog == null || checkLog.Result == false)
                {
                    Core.CheckLogManager.AddCheckLog(annualCheck.ID, instId, CheckType.Annual);
                }
                Core.ProfileManager.AddProfile(instId, profile);
            }
            else
            {
                if (checkLog == null || checkLog.Result.HasValue)
                {
                    var profileId = Core.ProfileManager.AddProfile(instId, profile);
                    Core.CheckLogManager.AddCheckLog(profileId, instId, CheckType.Profile);
                }
            }
        }

        //public int AddProfile(int instId, InstitutionProfile profile)
        //{
        //    var lastProfile = Core.ProfileManager.GetLastProfile(instId);
        //    if (lastProfile == null)
        //    {
        //        return Core.ProfileManager.AddProfile(instId, profile);
        //    }

        //    var checkLog = Core.CheckLogManager.GetCheckLog(lastProfile.ID, instId, CheckType.Profile);
        //    if (checkLog != null && !checkLog.Result.HasValue)
        //    {
        //        Core.ProfileManager.UpdateProfile(lastProfile.ID, profile);
        //        return lastProfile.ID;
        //    }
        //    else if (checkLog == null)
        //    {
        //        Core.ProfileManager.UpdateProfile(lastProfile.ID, profile);
        //        return lastProfile.ID;
        //    }
        //    else
        //    {
        //        return Core.ProfileManager.AddProfile(instId, profile);
        //    }
        //}

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

        public List<Profile> GetProfiles(int instId)
        {
            using (var db = GetDataContext())
            {
                return db.Profiles.Where(e => e.UserID == instId).OrderByDescending(e => e.ID).Select(e => new Profile
                {
                    ID = e.ID,
                    CreateTime = e.CreateTime,
                    UpdateTime = e.UpdateTime
                }).ToList();
            }
        }
    }
}
