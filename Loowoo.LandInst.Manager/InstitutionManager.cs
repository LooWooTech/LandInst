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
    public class InstitutionManager : ManagerBase
    {
        private string _cacheKey = "InstNames";
        private Dictionary<int, string> GetInstNames()
        {
            return CacheHelper.GetOrSet(_cacheKey, () =>
            {
                using (var db = GetDataContext())
                {
                    return db.Institutions.Select(e => new { e.ID, e.Name }).ToDictionary(e => e.ID, e => e.Name);
                }
            });
        }

        internal string GetInstName(int instId)
        {
            var names = GetInstNames();
            return names.ContainsKey(instId) ? names[instId] : null;
        }

        private void ClearCache()
        {
            CacheHelper.Remove(_cacheKey);
        }

        public int GetInstId(string instName)
        {
            if (string.IsNullOrEmpty(instName))
            {
                return 0;
            }

            using (var db = GetDataContext())
            {
                return db.Institutions.Where(e => e.Name == instName).Select(e => e.ID).FirstOrDefault();
            }
        }

        public Institution GetInstitution(int id)
        {
            using (var db = GetDataContext())
            {
                return db.Institutions.FirstOrDefault(e => e.ID == id);
            }
        }


        public List<VCheckInst> GetVCheckInsts(InstitutionFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.VCheckInsts.AsQueryable().GetCheckBaseQuery(filter);

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        public List<Institution> GetInstitutions(InstitutionFilter filter)
        {
            using (var db = GetDataContext())
            {
                var query = db.Institutions.AsQueryable();
                if (filter.InstId.HasValue && filter.InstId.Value > 0)
                {
                    query = query.Where(e => e.ID == filter.InstId);
                }

                if (filter.Status.HasValue)
                {
                    query = query.Where(e => e.Status == filter.Status.Value);
                }

                if (!string.IsNullOrEmpty(filter.Keyword))
                {
                    query = query.Where(e => e.Name.Contains(filter.Keyword));
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    query = query.Where(e => e.City == filter.City);
                }

                return query.OrderByDescending(e => e.ID).SetPage(filter.Page).ToList();
            }
        }

        public void LogoutInstitution(string name)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.Name.ToLower() == name.ToLower());
                if (entity == null)
                {
                    throw new ArgumentException("没找到该全称的公司");
                }
                entity.Status = InstitutionStatus.Logout;
                db.SaveChanges();
            }
        }

        public void AddInstitution(Institution model)
        {
            using (var db = GetDataContext())
            {
                db.Institutions.Add(model);
                db.SaveChanges();
                ClearCache();
            }
        }

        public void ApprovalInst(CheckLog checkLog)
        {
            var profile = Core.ProfileManager.GetLastProfile(checkLog.UserID, null);
            if (checkLog.Result == true)
            {
                UpdateInst(checkLog.UserID, profile.Convert<InstitutionProfile>());
            }
            Core.ProfileManager.UpdateProfileCheckResult(profile.ID, checkLog.Result);
        }

        public void UpdateInst(int instId, Institution model)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.ID == instId);
                if (entity != null)
                {
                    if (model.Name != entity.Name)
                    {
                        Core.UserManager.UpdateUsername(entity.ID, model.Name);
                    }

                    entity.Name = model.Name;
                    entity.RegistrationNo = model.RegistrationNo;
                    entity.LegalPerson = model.LegalPerson;
                    //entity.MobilePhone = model.MobilePhone;
                    entity.City = model.City;
                    db.SaveChanges();
                    ClearCache();
                }
                else
                {
                    throw new ArgumentException("没找到该机构");
                }
            }
        }

        public InstitutionProfile GetProfile(int instId, bool? checkResult = null)
        {
            return Core.ProfileManager.GetLastProfile<InstitutionProfile>(instId, checkResult);
        }

        public InstitutionProfile GetProfile(CheckLog checkLog)
        {
            if (checkLog == null) return null;
            var profileId = checkLog.DataAsInt();
            if (profileId == 0 && checkLog.CheckType == CheckType.Profile)
                profileId = checkLog.InfoID;
            return Core.ProfileManager.GetProfile<InstitutionProfile>(profileId);
        }


        public void SubmitAnnaulCheck(Institution inst, InstitutionProfile profile)
        {
            var annualCheck = Core.AnnualCheckManager.GetIndateModel();
            var checkLog = Core.CheckLogManager.GetCheckLog(annualCheck.ID, inst.ID, CheckType.Annual);
            if (checkLog == null || checkLog.Result == false)
            {
                var profileId = SaveProfile(inst, profile);
                Core.CheckLogManager.AddCheckLog(annualCheck.ID, inst.ID, CheckType.Annual, profileId.ToString());
            }
            else
            {
                SaveProfile(inst, profile);
            }
        }

        public void SubmitProfile(Institution inst, InstitutionProfile profile)
        {
            //如果当前已经提交了资料变更申请，则只更新资料
            var checkLog = Core.CheckLogManager.GetLastLog(inst.ID, CheckType.Profile);

            if (checkLog == null || checkLog.Result.HasValue)
            {
                var profileId = SaveProfile(inst, profile);
                Core.CheckLogManager.AddCheckLog(profileId, inst.ID, CheckType.Profile, profileId.ToString());
            }
            else
            {
                Core.ProfileManager.UpdateProfile(checkLog.InfoID, profile);
            }
        }

        public int SaveProfile(Institution inst, InstitutionProfile profile)
        {
            profile.SetInstField(inst);
            var draftProfile = Core.ProfileManager.GetLastProfile(inst.ID);
            if (draftProfile == null || draftProfile.CheckResult.HasValue)
            {
                return Core.ProfileManager.AddProfile(inst.ID, profile);
            }

            Core.ProfileManager.UpdateProfile(draftProfile.ID, profile);
            return draftProfile.ID;
        }

        public void UpdateStatus(int instId, InstitutionStatus status)
        {
            using (var db = GetDataContext())
            {
                var entity = db.Institutions.FirstOrDefault(e => e.ID == instId);
                if (entity != null)
                {
                    entity.Status = status;
                    db.SaveChanges();
                }
            }
        }


        public IEnumerable<CheckLog> GetProfileHistory(int instId)
        {
            return Core.CheckLogManager.GetList(instId)
                .Where(e => e.CheckType == CheckType.Profile || e.CheckType == CheckType.Annual)
                .ToList();
        }

        public void Import(User user, InstitutionProfile profile)
        {
            var inst = new Institution
            {
                ID = user.ID,
                City = profile.City,
                CreateTime = DateTime.Now,
                LegalPerson = profile.LegalPerson,
                Name = user.Username,
                RegistrationNo = profile.RegistrationNo,
                Status = InstitutionStatus.Registered,

            };

            Core.InstitutionManager.AddInstitution(inst);
            Core.ProfileManager.AddProfile(user.ID, profile);
        }

        public Dictionary<int, List<ExcelCell>> GetExportData(int instId, int checkLogId = 0)
        {
            InstitutionProfile profile = null;

            if (checkLogId > 0)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
                profile = GetProfile(checkLog);
            }
            else
            {
                profile = GetProfile(instId);
            }
            if (profile == null) return null;

            var sheetValues = new Dictionary<int, List<ExcelCell>>();
            sheetValues.Add(0, GetBasicInfoSheetValues(profile));
            sheetValues.Add(1, GetMemberStatisticSheetValues(instId));
            sheetValues.Add(2, GetMemberListSheetValues(instId));
            sheetValues.Add(3, GetEquipmentAndSoftwareSheetValues(profile));

            return sheetValues;
        }

        private List<ExcelCell> GetBasicInfoSheetValues(InstitutionProfile profile)
        {
            var list = new List<ExcelCell>();

            list.Add(new ExcelCell(1, 1, profile.Name));

            list.Add(new ExcelCell(2, 1, profile.CompanyType));
            list.Add(new ExcelCell(2, 4, profile.RegisteredCapital + "（万元）"));

            list.Add(new ExcelCell(3, 1, profile.CreateTime.ToShortDateString()));
            list.Add(new ExcelCell(3, 5, profile.LegalpersonNo));

            list.Add(new ExcelCell(4, 1, profile.Address));
            list.Add(new ExcelCell(4, 5, profile.Postcode));

            list.Add(new ExcelCell(5, 1, profile.Address1));
            list.Add(new ExcelCell(5, 5, profile.Postcode1));

            list.Add(new ExcelCell(6, 1, profile.LegalPerson));
            list.Add(new ExcelCell(6, 5, profile.TechLeader));

            list.Add(new ExcelCell(8, 1, profile.ContactPerson));
            list.Add(new ExcelCell(8, 3, profile.Tel));
            list.Add(new ExcelCell(8, 5, profile.Fax));

            list.Add(new ExcelCell(9, 1, profile.MobilePhone));
            list.Add(new ExcelCell(9, 3, profile.Email));

            if (profile.TotalMembers.HasValue)
            {
                list.Add(new ExcelCell(10, 2, profile.TotalMembers + "（人）"));
            }
            else
            {
                list.Add(new ExcelCell(10, 2, "            （人）"));
            }

            if (profile.ExpertMembers.HasValue)
            {
                list.Add(new ExcelCell(10, 5, profile.ExpertMembers + "（人）"));
            }
            else
            {
                list.Add(new ExcelCell(10, 5, "       （人）"));
            }

            if (profile.OfficeArea.HasValue)
            {
                list.Add(new ExcelCell(11, 2, profile.OfficeArea + "（平方米）"));
            }
            else
            {
                list.Add(new ExcelCell(11, 2, "                                     （平方米）"));
            }

            switch (profile.CommendLevel)
            {
                case "甲级":
                    list.Add(new ExcelCell(12, 2, "甲级√            乙级            准乙级"));
                    break;
                case "乙级":
                    list.Add(new ExcelCell(12, 2, "甲级            乙级√            准乙级"));
                    break;
                case "准乙级":
                    list.Add(new ExcelCell(12, 2, "甲级            乙级            准乙级√"));
                    break;
                default:
                    list.Add(new ExcelCell(12, 2, "甲级            乙级            准乙级"));
                    break;
            }

            return list;
        }

        public void Delete(int id)
        {
            using (var db = GetDataContext())
            {
                var user = db.Users.FirstOrDefault(e => e.ID == id);
                db.Users.Remove(user);

                var inst = db.Institutions.FirstOrDefault(e => e.ID == user.ID);
                db.Institutions.Remove(inst);

                var profiles = db.Profiles.Where(e => e.UserID == inst.ID);
                db.Profiles.RemoveRange(profiles);

                db.SaveChanges();
            }
        }

        private List<ExcelCell> GetMemberStatisticSheetValues(int instId)
        {
            var list = new List<ExcelCell>();

            using (var db = GetDataContext())
            {
                var datas = db.Members.Where(e => e.InstitutionID == instId && e.Status != MemberStatus.Normal).Select(e => new
                {
                    e.Major,
                    e.ProfessionalLevel,
                    e.EduRecord
                }).ToList();

                db.Dispose();

                var rowIndex = 3;

                Action<Dictionary<Major, int>> writeRow = (rowData) =>
                {
                    foreach (var kv in rowData)
                    {
                        var cellIndex = (int)kv.Key + 1;
                        list.Add(new ExcelCell(rowIndex, cellIndex, kv.Value.ToString()));
                    }
                    //合计
                    list.Add(new ExcelCell(rowIndex, rowData.Count + 2, rowData.Sum(e => e.Value).ToString()));
                    rowIndex++;
                };


                var columns = Enum.GetNames(typeof(Major)).Select(name => (Major)Enum.Parse(typeof(Major), name)).ToArray();
                //第一行总计
                var row1 = columns.ToDictionary(name => name, name => datas.Count(e => e.Major == name));
                writeRow(row1);

                foreach (ProfessionalLevel val in Enum.GetValues(typeof(ProfessionalLevel)))
                {
                    var row = columns.ToDictionary(name => name, name => datas.Count(e => e.Major == name && e.ProfessionalLevel == val));
                    writeRow(row);
                }

                foreach (EduRecord val in Enum.GetValues(typeof(EduRecord)))
                {
                    var row = columns.ToDictionary(name => name, name => datas.Count(e => e.Major == name && e.EduRecord == val));
                    writeRow(row);
                }
            }

            return list;
        }

        private List<ExcelCell> GetMemberListSheetValues(int instId)
        {
            var list = new List<ExcelCell>();
            using (var db = GetDataContext())
            {
                var ids = db.Members.Where(e => e.InstitutionID == instId && e.Status != MemberStatus.Normal).Select(e => e.ID).ToArray();
                db.Dispose();
                var rowIndex = 4;
                foreach (var memberId in ids)
                {
                    var profile = Core.ProfileManager.GetLastProfile<MemberProfile>(memberId);
                    list.Add(new ExcelCell(rowIndex, 0, profile.ID.ToString()));
                    list.Add(new ExcelCell(rowIndex, 1, profile.RealName));
                    list.Add(new ExcelCell(rowIndex, 2, profile.Gender));
                    list.Add(new ExcelCell(rowIndex, 3, profile.Birthday.HasValue ? (DateTime.Now.Year - profile.Birthday.Value.Year).ToString() : null));
                    list.Add(new ExcelCell(rowIndex, 4, profile.EduRecord.ToString()));
                    list.Add(new ExcelCell(rowIndex, 5, profile.EduLevel));
                    list.Add(new ExcelCell(rowIndex, 6, profile.School));
                    list.Add(new ExcelCell(rowIndex, 7, profile.GraduationDate.HasValue ? profile.GraduationDate.Value.ToShortDateString() : null));
                    list.Add(new ExcelCell(rowIndex, 8, profile.Major.ToString()));
                    list.Add(new ExcelCell(rowIndex, 9, profile.ProfessionalLevel.ToString()));
                    list.Add(new ExcelCell(rowIndex, 10, profile.StartWorkingDate.HasValue ? profile.StartWorkingDate.Value.ToShortDateString() : null));
                    list.Add(new ExcelCell(rowIndex, 11, profile.Office));
                    list.Add(new ExcelCell(rowIndex, 12, profile.WorkingYears == 0 ? null : profile.WorkingYears.ToString()));
                    list.Add(new ExcelCell(rowIndex, 13, profile.Job));
                    list.Add(new ExcelCell(rowIndex, 14, profile.IDNo));
                    list.Add(new ExcelCell(rowIndex, 15, profile.IsFullTime ? "√" : null));
                    list.Add(new ExcelCell(rowIndex, 16, !profile.IsFullTime ? "√" : null));


                    rowIndex++;
                }
            }
            return list;
        }

        private List<ExcelCell> GetEquipmentAndSoftwareSheetValues(InstitutionProfile profile)
        {
            var list = new List<ExcelCell>();
            var rowIndex = 3;
            foreach (var equipment in profile.Equipments)
            {
                list.Add(new ExcelCell(rowIndex, 0, equipment.Name));
                list.Add(new ExcelCell(rowIndex, 1, equipment.Model));
                list.Add(new ExcelCell(rowIndex, 2, equipment.Number.ToString()));
                list.Add(new ExcelCell(rowIndex, 3, equipment.Performance));
                list.Add(new ExcelCell(rowIndex, 4, equipment.Note));

                rowIndex++;
                if (rowIndex > 11) break;
            }

            rowIndex = 14;
            foreach (var software in profile.Softwares)
            {
                list.Add(new ExcelCell(rowIndex, 0, software.Name));
                list.Add(new ExcelCell(rowIndex, 1, software.Source));
                list.Add(new ExcelCell(rowIndex, 2, software.Number.ToString()));
                list.Add(new ExcelCell(rowIndex, 3, software.Purpose));
                list.Add(new ExcelCell(rowIndex, 4, software.Note));
                if (rowIndex > 22) break;
            }

            return list;
        }
    }
}
