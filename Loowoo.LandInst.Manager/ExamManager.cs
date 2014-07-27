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
            var now =DateTime.Now;
            return GetExams().Where(e => e.StartSignDate <= now && e.EndSignDate >= now).ToList();
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

        public List<VCheckExam> GetVCheckExams(CheckLogFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckMembers.Where(e => e.CheckType == CheckType.Education);
                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.InfoID == filter.InfoID.Value);
                }

                if (filter.Result.HasValue)
                {
                    query = query.Where(e => e.Result == filter.Result.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }
                var vlist = query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
                return vlist.Select(e => new VCheckExam
                {

                }).ToList();
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
