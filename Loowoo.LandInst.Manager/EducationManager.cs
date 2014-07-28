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
    public class EducationManager : ManagerBase
    {

        private string _cacheKey = "educations_cache";

        public List<Education> GetEducations()
        {
            return CacheHelper.GetOrSet(_cacheKey, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.Educations.OrderByDescending(e => e.ID).ToList();
                }
            });
        }

        private void ClearCache()
        {
            CacheHelper.Remove(_cacheKey);
        }

        public Education GetEducatoin(int eduId)
        {
            return GetEducations().FirstOrDefault(e => e.ID == eduId);
        }

        public void SaveEducation(Education edu)
        {
            using (var db = GetDataContext())
            {
                if (edu.ID > 0)
                {
                    var entity = db.Educations.FirstOrDefault(e => e.ID == edu.ID);
                    if (entity == null)
                    {
                        throw new ArgumentException("Education.Id错误");
                    }
                    db.Entry(entity).CurrentValues.SetValues(edu);
                }
                else
                {
                    db.Educations.Add(edu);
                }
                db.SaveChanges();
                ClearCache();
            }
        }

        //public void Approval(int approvalId, bool result)
        //{
        //    using (var db = GetDataContext())
        //    {
        //        var approval = db.CheckLogs.FirstOrDefault(e => e.ID == approvalId);
        //        approval.Result = result;
        //        db.SaveChanges();
        //    }
        //}

        public void SignupEducation(int eduId, int memberId)
        {
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Education);
            if (checkLog == null || checkLog.Result.HasValue)
            {
                Core.CheckLogManager.AddCheckLog(eduId, memberId, CheckType.Education);
            }
        }

        private string GetEduName(int eduId)
        {
            var edu = GetEducatoin(eduId);
            return edu == null ? null : edu.Name;
        }

        public List<Education> GetMemberEducations(int memberId)
        {
            var list = GetEducations();
            var now = DateTime.Now;
            foreach (var edu in list)
            {
                if (edu.StartDate <= now && edu.EndDate >= now)
                {
                    edu.Approval = Core.CheckLogManager.GetCheckLog(edu.ID, memberId, CheckType.Education);
                }
            }
            return list;
        }

        public List<VCheckEducation> GetApprovalEducations(CheckLogFilter filter)
        {
            using (var db = GetDataContext())
            {
                filter.Type = CheckType.Education;

                var query = Core.MemberManager.GetVCheckMembers(db.VCheckMembers, filter);

                var vlist = query.OrderByDescending(e => e.CreateTime).SetPage(filter.Page).ToList();

                return vlist.Select(e => new VCheckEducation
                {
                    EduID = e.InfoID,
                    EduName = GetEduName(e.InfoID),
                    Member = e
                }).ToList();
            }
        }

    }
}
