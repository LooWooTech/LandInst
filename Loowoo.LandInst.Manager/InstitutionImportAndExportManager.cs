using Loowoo.LandInst.Common;
using Loowoo.LandInst.Model;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Manager
{
    public partial class InstitutionManager
    {

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

        public InstitutionProfile GetExportProfile(int instId, int checkLogId = 0)
        {
            if (checkLogId > 0)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
                return GetProfile(checkLog);
            }
            else
            {
                return GetProfile(instId);
            }
        }

        public List<ExcelCell> GetExportData(InstitutionProfile profile)
        {

            var list = new List<ExcelCell>();

            var rowIndex = 1;

            //名称
            list.Add(new ExcelCell(rowIndex, 1, profile.Name));

            //法人
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.LegalPerson));
            list.Add(new ExcelCell(rowIndex, 3, profile.LegalpersonNo));
            list.Add(new ExcelCell(rowIndex, 5, profile.CompanyType));

            //工商登记
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.RegistrationNo));
            list.Add(new ExcelCell(rowIndex, 3, profile.TaxRegistryNo));
            list.Add(new ExcelCell(rowIndex, 5, profile.RegisteredCapital.HasValue ? profile.RegisteredCapital.Value + "万元" : null));

            //成立日期
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.EstablishedDate.HasValue ? profile.EstablishedDate.Value.ToShortDateString() : null));
            list.Add(new ExcelCell(rowIndex, 3, profile.OperatingPeriod));
            list.Add(new ExcelCell(rowIndex, 5, profile.RegistrationInstitution));

            //机构人数
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.TotalMembers.HasValue ? profile.TotalMembers.Value + "人" : null));
            list.Add(new ExcelCell(rowIndex, 3, profile.ProMembers.HasValue ? profile.ProMembers.Value + "人" : null));
            list.Add(new ExcelCell(rowIndex, 5, profile.ExpertMembers.HasValue ? profile.ExpertMembers.Value + "人" : null));

            //执业注册号
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.PracticeRegistrationNo));
            list.Add(new ExcelCell(rowIndex, 3, profile.CorporateMemberNo));

            //联系电话
            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Tel));
            list.Add(new ExcelCell(rowIndex, 3, profile.Fax));
            list.Add(new ExcelCell(rowIndex, 5, profile.MobilePhone));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Email));
            list.Add(new ExcelCell(rowIndex, 4, profile.Website));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.BusinessScope));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Address));
            list.Add(new ExcelCell(rowIndex, 5, profile.Postcode));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 1, profile.Address1));
            list.Add(new ExcelCell(rowIndex, 5, profile.Postcode1));

            rowIndex++;
            list.Add(new ExcelCell(rowIndex, 2, profile.HasExequatur ? "是" : "否"));
            list.Add(new ExcelCell(rowIndex, 5, profile.ExequaturLevel));

            rowIndex = 15;
            foreach (var shareholder in profile.ShareHolders)
            {
                list.Add(new ExcelCell(rowIndex, 0, shareholder.Name));
                list.Add(new ExcelCell(rowIndex, 1, shareholder.Gender));
                var birthday = shareholder.Birthday.AsNullOrDate();
                list.Add(new ExcelCell(rowIndex, 2, birthday.HasValue ? (DateTime.Now.Year - birthday.Value.Year).ToString() : null));
                list.Add(new ExcelCell(rowIndex, 3, shareholder.Title));
                list.Add(new ExcelCell(rowIndex, 4, shareholder.Shares));
                list.Add(new ExcelCell(rowIndex, 5, shareholder.Professionals.HasValue ? (shareholder.Professionals.Value ? "是" : "否") : null));
            }

            rowIndex += (profile.ShareHolders.Count > 3 ? profile.ShareHolders.Count : 3) + 2;
            rowIndex += 2;
            foreach (var item in profile.Equipments)
            {
                list.Add(new ExcelCell(rowIndex, 0, item.Name));
                list.Add(new ExcelCell(rowIndex, 0, item.Number.HasValue ? item.Number.Value.ToString() : null));
                list.Add(new ExcelCell(rowIndex, 0, item.Model));
                list.Add(new ExcelCell(rowIndex, 0, item.Note));

            }

            return list;
        }

    }
}
