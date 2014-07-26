using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class EducationManager : ManagerBase
    {
        public void CheckEducation(int memberId)
        {

        }

        public List<Education> GetEducations()
        {
            using (var db = GetDataContext())
            {
                return db.Educations.OrderByDescending(e => e.ID).ToList();
            }
        }

        public Education GetEducatoin(int eduId)
        {
            if (eduId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Educations.FirstOrDefault(e => e.ID == eduId);
            }
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
            }
        }

        public List<Education> GetMemberEducations(int memberId)
        {
            using (var db = GetDataContext())
            {
                var list = db.Educations.OrderByDescending(e => e.ID).ToList();
                foreach (var edu in list)
                {
                    edu.Approval = Core.CheckLogManager.GetCheckLog(edu.ID, memberId, CheckType.Education);
                }
                return list;
            }

        }

        public void Approval(int approvalId, bool result)
        {
            using (var db = GetDataContext())
            {
                var approval = db.CheckLogs.FirstOrDefault(e => e.ID == approvalId);
                approval.Result = result;
                db.SaveChanges();
            }
        }

        public void SignupEducation(Member member, Education edu)
        {
            using (var db = GetDataContext())
            {
                db.CheckLogs.Add(new Model.CheckLog
                {
                    UserID = member.ID,
                    InfoID = edu.ID,
                    CheckType = CheckType.Education,

                });
                db.SaveChanges();
            }
        }

        public List<VCheckEducation> GetApprovalEducations(CheckLogFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalEducations.Where(e => e.CheckType == CheckType.Education);
                if (filter.Result.HasValue)
                {
                    query = query.Where(e => e.Result == filter.Result.Value);
                }
                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.EduID == filter.InfoID.Value);
                }
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword.Trim()));
                }
                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }
    }
}
