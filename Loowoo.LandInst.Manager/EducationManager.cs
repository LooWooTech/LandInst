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

        public List<Education> GetEducations(EducationFilter filter = null)
        {
            using (var db = GetDataContext())
            {
                var query = db.Educations.AsQueryable();
                if (filter == null) return query.ToList();


                return query.SetPage(filter).ToList();
            }
        }

        public Education GetEducatoin(int eduId)
        {
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

        public List<MemberEducation> GetMemberEducations(EducationFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.MemberEducations.AsQueryable();
                if (filter.EducationID.HasValue)
                {
                    query = query.Where(e => e.EducationID == filter.EducationID.Value);
                }
                if (filter.Approval.HasValue)
                {
                    query = query.Where(e => e.Approval == filter.Approval.Value);
                }

                return query.Join(db.Members, e => e.MemberID, e => e.ID, (e, m) => new MemberEducation
                {
                    MemberID = e.MemberID,
                    ID = e.ID,
                    SignupTime = e.SignupTime,
                    ApprovalTime = e.ApprovalTime,
                    Approval = e.Approval,
                    MemberName = m.Username,
                    EducationID = e.EducationID
                }).ToList();
            }

        }

        public void Approval(int memberId, int eduId, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.MemberEducations.FirstOrDefault(e => e.MemberID == memberId && e.EducationID == eduId);
                if (entity != null)
                {
                    entity.Approval = result;
                }
                db.SaveChanges();
            }
        }
    }
}
