using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Manager
{
    public partial class MemberManager : ManagerBase
    {

        public int SaveProfile(Member member, MemberProfile profile)
        {
            profile.SetMemberField(member);
            var draftProfile = Core.ProfileManager.GetLastProfile(member.ID);
            if (draftProfile == null || draftProfile.CheckResult.HasValue)
            {
                return Core.ProfileManager.AddProfile(member.ID, profile);
            }
            Core.ProfileManager.UpdateProfile(draftProfile.ID, profile);
            return draftProfile.ID;
        }

        public void SubmitProfile(Member member, MemberProfile profile)
        {
            //如果当前已经提交了资料变更申请，则只更新资料
            var checkLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Profile);

            if (checkLog == null || checkLog.Result.HasValue)
            {
                var profileId = SaveProfile(member, profile);
                Core.CheckLogManager.AddCheckLog(profileId, member.ID, CheckType.Profile, profileId.ToString());
            }
            else
            {
                Core.ProfileManager.UpdateProfile(checkLog.InfoID, profile);
            }
        }

        public void SubmitPractice(Member member, MemberProfile profile)
        {
            if (member.Status == MemberStatus.Practice) return;
            var checkLog = Core.CheckLogManager.GetLastLog(member.ID, CheckType.Practice);
            if (checkLog == null || checkLog.Result == false)
            {
                var profileId = SaveProfile(member, profile);
                Core.CheckLogManager.AddCheckLog(profileId, member.ID, CheckType.Practice, profileId.ToString());
            }
            else
            {
                SaveProfile(member, profile);
            }
        }

        public void ApprovalMember(CheckLog checkLog)
        {
            if (checkLog == null) return;
            if (checkLog.Result == true)
            {
                var profile = Core.MemberManager.GetProfile(checkLog);
                if (profile == null) return;

                profile.ID = checkLog.UserID;
                if (checkLog.CheckType == CheckType.Practice)
                {
                    profile.Status = MemberStatus.Practice;
                }
                Core.MemberManager.UpdateMember(profile);
            }
            Core.ProfileManager.UpdateProfileCheckResult(checkLog.DataAsInt(), checkLog.Result);
        }

        public int AddMember(Member member)
        {
            using (var db = GetDataContext())
            {
                db.Members.Add(member);
                db.SaveChanges();
                return member.ID;
            }
        }

        public void UpdateMemberStatus(int memberId, MemberStatus status)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == memberId);
                entity.Status = status;
                db.SaveChanges();
            }
        }

        public void UpdateMember(Member member)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == member.ID);
                if (entity == null)
                {
                    throw new ArgumentException("Member.ID");
                }
                db.Entry(entity).CurrentValues.SetValues(member);
                db.SaveChanges();
            }
        }

        public void Import(Member member, MemberProfile profile)
        {
            AddMember(member);
            profile.ID = member.ID;
            Core.ProfileManager.AddProfile(member.ID, profile);
        }

        public Member GetMember(int memberId)
        {
            if (memberId == 0) return null;
            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.ID == memberId);
            }
        }

        public Member GetMember(string idNo)
        {
            if (string.IsNullOrEmpty(idNo)) return null;

            using (var db = GetDataContext())
            {
                return db.Members.FirstOrDefault(e => e.IDNo == idNo);
            }
        }

        public MemberProfile GetProfile(CheckLog checkLog)
        {
            if (checkLog == null || !(checkLog.CheckType == CheckType.Practice || checkLog.CheckType == CheckType.Profile)) return null;
            return Core.ProfileManager.GetProfile<MemberProfile>(checkLog.DataAsInt());
        }

        public MemberProfile GetProfile(int memberId)
        {
            if (memberId == 0) return null;
            var member = Core.MemberManager.GetMember(memberId);
            if (member == null) return null;
            var profile = Core.ProfileManager.GetLastProfile<MemberProfile>(memberId);
            if (profile != null)
            {
                profile.SetMemberField(member);
            }
            else
            {
                profile = new MemberProfile(member);
            }
            return profile;
            //if (memberId == 0) return null;
            //var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Profile, checkResult);
            //if (checkLog == null)
            //    return null;
            //return Core.ProfileManager.GetProfile<MemberProfile>(checkLog.InfoID);
        }

        //public void SaveProfile(Member member, MemberProfile profile)
        //{
        //    profile.SetMemberField(member);
        //    Core.MemberManager.UpdateMember(profile);
        //    var entity = Core.ProfileManager.GetLastProfile(member.ID);
        //    if (entity == null)
        //    {
        //        Core.ProfileManager.AddProfile(member.ID, profile);
        //    }
        //    else
        //    {
        //        Core.ProfileManager.UpdateProfile(entity.ID, profile);
        //    }

        //}

        public List<Member> GetMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.AsQueryable();

                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    if (filter.InInst)
                    {
                        query = query.Where(e => e.InstitutionID == filter.InstID);
                    }
                    else
                    {
                        query = query.Where(e => e.InstitutionID != filter.InstID);
                    }
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                if (filter.MinStatus.HasValue)
                {
                    query = query.Where(e => e.Status >= filter.MinStatus.Value);
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                var list = query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
                if (filter.GetInstName)
                {
                    foreach (var item in list)
                    {
                        if (item.InstitutionID > 0)
                            item.InstitutionName = Core.InstitutionManager.GetInstName(item.InstitutionID);
                    }
                }
                return list;
            }
        }

        public List<VCheckMember> GetVCheckMembers(MemberFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckMembers.AsQueryable().GetCheckBaseQuery(filter);

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                if (filter.InstID.HasValue && filter.InstID.Value > 0)
                {
                    query = query.Where(e => e.InstitutionID == filter.InstID.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.RealName.Contains(filter.Keyword));
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        internal IQueryable<VCheckMember> GetVCheckMembers(IQueryable<VCheckMember> query, CheckLogFilter filter)
        {
            query = query.GetCheckBaseQuery(filter);

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(e => e.RealName.Contains(filter.Keyword.Trim()));
            }


            return query;
        }

        public List<int> GetMemberIds(string[] realNames, int instId)
        {
            using (var db = GetDataContext())
            {
                var query = db.Members.Where(e => e.InstitutionID == instId && realNames.Contains(e.RealName));
                return query.Select(e => e.ID).ToList();
            }
        }

        public bool Exist(string idNo)
        {
            using (var db = GetDataContext())
            {
                return db.Members.Any(e => e.IDNo == idNo);
            }
        }

        public List<ExcelCell> GetExportData(int memberId, int checkLogId)
        {
            MemberProfile profile = null;
            if (checkLogId > 0)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
                profile = GetProfile(checkLog);
            }
            else
            {
                profile = GetProfile(memberId);
            }

            if (profile == null)
            {
                throw new ArgumentException("没有找到该会员资料");
            }

            if (profile.InstitutionID == 0)
            {
                throw new ArgumentException("没有权限导出该会员资料");
            }

            var inst = Core.InstitutionManager.GetInstitution(profile.InstitutionID);

            if (inst == null)
            {
                throw new ArgumentException("没有权限导出该会员资料");
            }


            var list = new List<ExcelCell>();

            var rowIndex = 1;

            list.Add(new ExcelCell(rowIndex, 1, profile.RealName));
            list.Add(new ExcelCell(rowIndex, 3, profile.Gender));
            list.Add(new ExcelCell(rowIndex, 5, profile.Birthday.HasValue ? profile.Birthday.Value.ToShortDateString() : null));

            rowIndex++;

            list.Add(new ExcelCell(rowIndex, 1, profile.NativePlace));
            list.Add(new ExcelCell(rowIndex, 3, profile.Nationality));
            list.Add(new ExcelCell(rowIndex, 5, profile.Email));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.MobilePhone));
            list.Add(new ExcelCell(rowIndex, 3, profile.IDNo));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Address));
            list.Add(new ExcelCell(rowIndex, 6, profile.Postcode));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Major == 0 ? "" : profile.Major.ToString()));
            list.Add(new ExcelCell(rowIndex, 6, profile.ProfessionalLevel == 0 ? "" : profile.ProfessionalLevel.ToString()));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.School));
            list.Add(new ExcelCell(rowIndex, 6, profile.EduRecord == 0 ? "" : profile.EduRecord.ToString()));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 5, profile.PracticeRegistrationNO));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 3, profile.PersonalRecordsInstitution));
            list.Add(new ExcelCell(rowIndex, 6, profile.PersonalRecordsNO));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 3, profile.SocialSecurityInstitution));
            list.Add(new ExcelCell(rowIndex, 6, profile.SocialSecurityNO));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 3, inst.Name));
            list.Add(new ExcelCell(rowIndex, 6, profile.Office));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 3, inst.LegalPerson));
            list.Add(new ExcelCell(rowIndex, 6, inst.MobilePhone));


            rowIndex = 14;
            foreach (var cert in profile.Certifications)
            {
                list.Add(new ExcelCell(rowIndex, 0, cert.Name));
                list.Add(new ExcelCell(rowIndex, 2, cert.CertificationNo));
                list.Add(new ExcelCell(rowIndex, 5, cert.ObtainDate.HasValue ? cert.ObtainDate.Value.ToShortDateString() : null));
                rowIndex++;
            }

            rowIndex = 20;
            foreach (var job in profile.Jobs)
            {
                list.Add(new ExcelCell(rowIndex, 0, job.StartDate + "~" + job.EndDate));
                list.Add(new ExcelCell(rowIndex, 2, job.Institution));
                list.Add(new ExcelCell(rowIndex, 5, job.Office));
                list.Add(new ExcelCell(rowIndex, 6, job.Note));
            }

            rowIndex = 25;
            using (var db = GetDataContext())
            {
                var examResult = db.ExamResults.OrderByDescending(e => e.ID).FirstOrDefault(e => e.MemberID == memberId);
                if (examResult != null && !string.IsNullOrEmpty(examResult.Scores))
                {
                    var cellIndex = 0;
                    foreach (var score in examResult.Scores.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var values = score.Split(':');
                        list.Add(new ExcelCell(rowIndex, cellIndex, values[0]));
                        if (values.Length > 1)
                        {
                            list.Add(new ExcelCell(rowIndex + 1, cellIndex, values[1]));
                        }
                    }
                }
            }

            return list;
        }

        public void Delete(int memberId)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Members.FirstOrDefault(e => e.ID == memberId);
                if (entity != null)
                {
                    db.Members.Remove(entity);
                    db.SaveChanges();
                }
            }
        }
    }
}
