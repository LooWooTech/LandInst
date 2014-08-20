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

        public Exam GetIndateExam()
        {
            var now = DateTime.Now;
            return GetExams().Where(e => e.StartSignDate <= now && e.EndSignDate >= now).OrderByDescending(e => e.ID).FirstOrDefault();//.ToList();
        }

        //public ExamResult GetExamResult(int examResultId)
        //{
        //    using (var db = GetDataContext())
        //    {
        //        return db.ExamResults.FirstOrDefault(e => e.ID == examResultId);
        //    }
        //}

        public ExamResult GetExamResult(int examId, int memberId)
        {
            using (var db = GetDataContext())
            {
                return db.ExamResults.FirstOrDefault(e => e.ExamID == examId && e.MemberID == memberId);
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

        private void SaveExamResult(int examId, int memberId, string subjectNames)
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
                        Subjects = subjectNames
                    });
                }
                else
                {
                    var subjects = entity.Subjects + "," + subjectNames;
                    var names = subjects.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).GroupBy(name => name).Select(g => g.Key);
                    entity.Subjects = string.Join(",", names);
                }
                db.SaveChanges();
            }
        }



        public void SubmitExam(int examId, int memberId, string subjectNames)
        {
            if (string.IsNullOrEmpty(subjectNames))
            {
                throw new ArgumentNullException("没有选择报考科目");
            }

            Core.CheckLogManager.AddCheckLog(examId, memberId, CheckType.Exam, subjectNames);
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

        public List<VExamResult> GetVExamResults(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VExamResults.AsQueryable();
                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }
                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    query = query.Where(e => e.InstitutionID == filter.InstID.Value);
                }

                if (filter.InfoID.HasValue && filter.InfoID.Value > 0)
                {
                    query = query.Where(e => e.ExamID == filter.InfoID.Value);
                }

                if (filter.UserID.HasValue && filter.UserID.Value > 0)
                {
                    query = query.Where(e => e.MemberID == filter.UserID.Value);
                }

                var list = query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
                foreach (var item in list)
                {
                    item.Exam = GetExam(item.ExamID);
                }

                return list;
            }
        }

        public List<VCheckExam> GetVCheckExams(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                filter.Type = CheckType.Exam;
                return Core.MemberManager.GetVCheckMembers(filter).Select(e => new VCheckExam
                {
                    ExamName = GetExamName(e.InfoID),
                    VCheck = e
                }).ToList();
            }
        }

        private string _subjectsCache = "subjects";
        public List<ExamSubject> GetSubjects()
        {
            return CacheHelper.GetOrSet(_subjectsCache, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.Subjects.ToList();
                }
            });
        }

        public ExamSubject GetSubject(int subjectId)
        {
            return GetSubjects().FirstOrDefault(e => e.ID == subjectId);
        }

        public ExamSubject GetSubject(string name)
        {
            return GetSubjects().FirstOrDefault(e => e.Name == name);
        }

        public string GetSubjectName(int subjectId)
        {
            var subject = GetSubject(subjectId);
            return subject == null ? null : subject.Name;
        }

        public int SaveSubject(string name, int totalScore)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Subjects.FirstOrDefault(e => e.Name == name);
                if (entity != null)
                {
                    entity.TotalScore = totalScore;
                }
                else
                {
                    entity = new ExamSubject { Name = name, TotalScore = totalScore };
                    db.Subjects.Add(entity);
                }
                db.SaveChanges();
                CacheHelper.Remove(_subjectsCache);
                return entity.ID;
            }
        }

        public void DeleteSubject(int subjectId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Subjects.FirstOrDefault(e => e.ID == subjectId);
                db.Subjects.Remove(entity);
                db.SaveChanges();
                CacheHelper.Remove(_subjectsCache);
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
                //Add ExamResult
                Core.ExamManager.SaveExamResult(checkLog.InfoID, checkLog.UserID, checkLog.Data);
            }
        }

        public void ImportExamResult(ExamResult model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.MemberID == model.MemberID && e.ExamID == model.ExamID);
                if (entity == null)
                {
                    entity = new ExamResult
                    {
                        ExamID = model.ExamID,
                        MemberID = model.MemberID,
                        Subjects = model.Subjects,
                        CreateTime = model.CreateTime == DateTime.MinValue ? DateTime.Now : model.CreateTime,
                        UpdateTime = DateTime.Now,
                        Scores = model.Scores
                    };
                    db.ExamResults.Add(entity);
                }
                else
                {
                    entity.Subjects = model.Subjects;
                    entity.Scores = model.Scores;
                    entity.UpdateTime = DateTime.Now;
                }
                db.SaveChanges();
            }
        }

        public void UpdateExamScores(int memberId, int examId, string scores)
        {
            using (var db = GetDataContext())
            {
                var entity = db.ExamResults.FirstOrDefault(e => e.MemberID == memberId && e.ExamID == examId);
                if (entity != null)
                {
                    entity.UpdateTime = DateTime.Now;
                    entity.Scores = scores;
                    db.SaveChanges();
                }
            }
        }

    }
}
