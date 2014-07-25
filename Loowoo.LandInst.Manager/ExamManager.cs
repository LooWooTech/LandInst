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
                    query = query.Where(e => filter.SignTime.Value >= e.StartSignDate && filter.SignTime.Value <= e.EndSignDate);
                }

                return query.ToList();
            }
        }

        public List<Exam> GetMemberExams(int memberId)
        {
            using (var db = GetDataContext())
            {
                var exams = db.Exams.OrderByDescending(e => e.ID).ToList();
                foreach (var exam in exams)
                {
                    exam.Approval = Core.CheckLogManager.GetCheckLog(exam.ID, memberId, CheckType.Exam);
                }
                return exams;
            }
        }

        public List<VMemberExamResult> GetMemberExamResult(int memberId)
        {
            using (var db = GetDataContext())
            {
                return db.VMemberExamResults.Where(e => e.MemberID == memberId).OrderByDescending(e => e.ID).ToList();
            }
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

        public void SignupExam(int examId, int memberId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.CheckLogs.FirstOrDefault(e => e.InfoID == examId && e.UserID == memberId && e.CheckType == CheckType.Exam);
                if (entity != null)//已经报名 并通过审批
                {
                    return;
                }

                db.CheckLogs.Add(new CheckLog
                {
                    InfoID = examId,
                    UserID = memberId,
                    CheckType = CheckType.Exam
                });

                db.ExamResults.Add(new ExamResult
                {
                    ExamID = examId,
                    MemberID = memberId,
                    Result = false
                });
                db.SaveChanges();
            }
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

        public List<VCheckExam> GetApprovalExams(ApprovalFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalExams.Where(e => e.CheckType == CheckType.Education);
                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.ExamID == filter.InfoID.Value);
                }

                if (filter.Result.HasValue)
                {
                    query = query.Where(e => e.Result == filter.Result.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }
                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }

        public void UpdateExamResult(int examId, int memberId, bool result)
        {
            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.ExamID == examId && e.MemberID == memberId);
                if (entity != null)
                {
                    entity.Result = result;
                }
                else
                {
                    db.ExamResults.Add(new ExamResult
                    {
                        ExamID = examId,
                        MemberID = memberId,
                        Result = result
                    });
                }
                db.SaveChanges();
            }
        }
    }
}
