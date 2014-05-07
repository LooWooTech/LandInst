using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class ExamManager : ManagerBase
    {
        public List<Exam> GetExams(ExamFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Exams.AsQueryable();
                if (filter == null) return query.ToList();

                if (filter.SignTime.HasValue)
                {
                    query = query.Where(e => e.StartSignTime <= filter.SignTime.Value && e.EndSignTime >= filter.SignTime.Value);
                }

                return query.ToList();
            }
        }

        public List<MemberExam> GetMemberExams(int userId)
        {
            return Core.InfoDataManager.GetModel<List<MemberExam>>(userId, InfoType.Exam, InfoStatus.Normal);
        }

        public Exam GetExam(int examId)
        {
            if (examId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Exams.FirstOrDefault(e => e.ID == examId);
            }
        }

        public void SaveExam(Exam exam)
        {
            using (var db = GetDataContext())
            {
                
                if (exam.ID > 0)
                {
                    var entity = db.Exams.FirstOrDefault(e => e.ID == exam.ID);
                    if (entity != null)
                    {
                        db.Entry(entity).CurrentValues.SetValues(exam);
                    }
                }
                else
                {
                    db.Exams.Add(exam);
                }
                db.SaveChanges();
            }
        }

        public void SaveMemberExam(int userId, MemberExam memberExam)
        {
            Core.InfoDataManager.UpdateListItem(userId, InfoType.Exam, InfoStatus.Normal, memberExam);
        }

        public void Delete(int id)
        {
            if (id == 0) return;
            using (var db = GetDataContext())
            {
                var entity = db.Exams.FirstOrDefault(e => e.ID == id);
                if (entity == null) return;
                db.Exams.Remove(entity);
                db.SaveChanges();
            }
        }
    }
}
