﻿using System;
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
                    query = query.Where(e => filter.SignTime.Value >= e.StartSignTime && filter.SignTime.Value <= e.EndSignTime);
                }

                return query.ToList();
            }
        }

        public List<VMemberExam> GetMemberExams(int memberId)
        {
            using (var db = GetDataContext())
            {
                return db.VMemberExams.Where(e => e.MemberID == memberId && e.ApprovalType == ApprovalType.Exam).ToList();
            }
        }

        public List<VMemberExamResult> GetMemberExamResult(int memberId)
        {
            using (var db = GetDataContext())
            {
                return db.VMemberExamResults.Where(e => e.MemberID == memberId).ToList();
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
                var entity = db.Approvals.FirstOrDefault(e => e.InfoID == examId && e.UserID == memberId && e.ApprovalType == ApprovalType.Education);
                if (entity.Result.Value)//已经报名 并通过审批
                {
                    return;
                }

                db.Approvals.Add(new Approval
                {
                    InfoID = examId,
                    UserID = memberId,
                });

                db.ExamResults.Add(new ExamResult
                {
                    ExamID = examId,
                    MemberID = memberId,
                    Result = false
                });
                db.SaveChanges();
            }

            Core.MemberManager.UpdateMemberStatus(memberId, MemberStatus.SingupExam);
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

        public List<VApprovalExam> GetApprovalExams(ApprovalFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VApprovalExams.Where(e => e.ApprovalType == ApprovalType.Education);
                if (filter.InfoID.HasValue)
                {
                    query = query.Where(e => e.ExamID == filter.InfoID.Value);
                }

                return query.OrderByDescending(e => e.CreateTime).SetPage(filter).ToList();
            }
        }
    }
}
