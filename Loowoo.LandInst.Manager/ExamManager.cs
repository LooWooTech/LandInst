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
    public class ExamManager : ManagerBase
    {
        private string _cacheKey = "exam_key";
        private void ClearCache()
        {
            CacheHelper.Remove(_cacheKey);
        }

        public List<Exam> GetExams()
        {
            return CacheHelper.GetOrSet(_cacheKey, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.Exams.ToList();
                }
            });
        }

        public List<Exam> GetIndateExams()
        {
            var now = DateTime.Now;
            return GetExams().Where(e => e.StartSignDate <= now && e.EndSignDate >= now).ToList();
        }

        public ExamResult GetExamResult(int examResultId)
        {
            using (var db = GetDataContext())
            {
                return db.ExamResults.FirstOrDefault(e => e.ID == examResultId);
            }
        }

        public ExamResult GetExamResult(CheckLog checkLog)
        {
            if (checkLog == null)
            {
                throw new ArgumentException("没有申请考试报名");
            }

            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.ExamID == checkLog.InfoID && e.MemberID == checkLog.UserID);
                if (entity == null)
                {
                    AddExamResult(checkLog.InfoID, checkLog.UserID);
                    return GetExamResult(checkLog);
                }
                else
                {
                    return entity;
                }
            }
        }

        public List<ExamResult> GetMemberExamResult(int memberId)
        {
            var exams = GetExams();
            using (var db = GetDataContext())
            {
                var list = db.ExamResults.Where(e => e.MemberID == memberId).OrderByDescending(e => e.ID).ToList();
                foreach (var result in list)
                {
                    result.Exam = exams.FirstOrDefault(e => e.ID == result.ExamID);
                }
                return list;
            }
        }

        public Exam GetExam(int examId)
        {
            return GetExams().FirstOrDefault(e => e.ID == examId);
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
                ClearCache();
            }
        }

        private void AddExamResult(int examId, int memberId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.ExamID == examId && e.MemberID == memberId);
                if (entity == null)
                {
                    db.ExamResults.Add(new ExamResult
                    {
                        ExamID = examId,
                        MemberID = memberId,
                        Result = false
                    });
                    db.SaveChanges();
                }
            }
        }

        public void SignupExam(int examId, int memberId)
        {
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Exam);
            if (checkLog == null || checkLog.Result == false)
            {
                Core.CheckLogManager.AddCheckLog(examId, memberId, CheckType.Exam);
                AddExamResult(examId, memberId);
            }
            else
            {
                AddExamResult(examId, memberId);
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
                ClearCache();
            }
        }

        private string GetExamName(int examId)
        {
            var exam = GetExam(examId);
            return exam == null ? null : exam.Name;
        }

        public List<VCheckExam> GetVCheckExams(CheckLogFilter filter)
        {
            using (var db = GetDataContext())
            {
                filter.Type = CheckType.Exam;
                var query = Core.MemberManager.GetVCheckMembers(db.VCheckMembers, filter);
                var vlist = query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
                return vlist.Select(e => new VCheckExam
                {
                    ExamID = e.InfoID,
                    ExamName = GetExamName(e.InfoID),
                    Member = e
                }).ToList();
            }
        }

        public void UpdateExamResult(CheckLog checkLog, ExamResult model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.ExamID == checkLog.InfoID && e.MemberID == checkLog.UserID);
                entity.Result = model.Result;
                entity.Note = model.Note;

                db.SaveChanges();

                if (checkLog.Result == null)
                {
                    checkLog.Result = entity.Result;
                    Core.CheckLogManager.UpdateCheckLog(checkLog);
                }
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

        public void Approval(int approvalId, bool result)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(approvalId);

            if (checkLog == null) return;

            if (checkLog.Result.HasValue) return;

            checkLog.Result = result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);

            if (result)
            {
                Core.MemberManager.UpdateMemberStatus(checkLog.UserID, MemberStatus.Registered);
            }
        }
    }
}
