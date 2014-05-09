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

        public List<VMemberEducation> GetMemberEducations(int memberId)
        {
            using (var db = GetDataContext())
            {
                var query = db.VMemberEducations.Where(e => (e.ApprovalType == ApprovalType.Education && e.UserID == memberId) || e.ApprovalCount == 0);

                return query.ToList();
            }

        }

        public void Approval(int approvalId, bool result)
        {
            using (var db = GetDataContext())
            {
                var approval = db.Approvals.FirstOrDefault(e => e.ID == approvalId);
                approval.Result = result;
                db.SaveChanges();
            }
        }

        public void SignupEducation(Member member, Education edu)
        {
            using (var db = GetDataContext())
            {
                db.Approvals.Add(new Model.Approval
                {
                    UserID = member.ID,
                    InfoID = edu.ID,
                    ApprovalType = ApprovalType.Education,

                });
                db.SaveChanges();
            }
        }

        public List<VApprovalEducation> GetApprovalEducations(ApprovalFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalEducations.Where(e => e.ApprovalType == ApprovalType.Education);
                if (filter.Result.HasValue)
                {
                    query = query.Where(e => e.Result == filter.Result.Value);
                }
                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.EduID == filter.InfoID.Value);
                }
                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }
    }
}
