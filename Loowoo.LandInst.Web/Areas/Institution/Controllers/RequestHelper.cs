using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public static class RequestHelper
    {
        public static AnnualCheckProfile GetAnnualProfile(this NameValueCollection requestForm)
        {
            var model = new AnnualCheckProfile
            {
                AwardsAndComplaints = requestForm["AwardsAndComplaints"],
                BusinessSituation = requestForm["BusinessSituation"],
                EmployeeConduct = requestForm["EmployeeConduct"],
                StandardsImplementation = requestForm["StandardsImplementation"],
                Summary = requestForm["Summary"]
            };
            try
            {
                var categories = requestForm["Result.Category"].Split(',');
                var incomes = requestForm["Result.Income"].Split(',');
                var numbers = requestForm["Result.Number"].Split(',');

                for (var i = 0; i < categories.Length; i++)
                {
                    model.Results.Add(new BusinessResult
                    {
                        Category = categories[i],
                        Income = incomes[i],
                        Number = numbers[i],
                    });
                }
            }
            catch { }
            return model;
        }

        public static List<MemberProfile> GetMembers(this NameValueCollection requestForm)
        {
            var list = new List<MemberProfile>();
            try
            {
                var realnames = requestForm["Members.RealName"].Split(',');
                var genders = requestForm["Members.Gender"].Split(',');
                var idnos = requestForm["Members.IDNo"].Split(',');
                var eduRecords = requestForm["Members.EduRecord"].Split(',');
                var majors = requestForm["Members.Major"].Split(',');
                var professionalLevels = requestForm["Members.ProfessionalLevel"].Split(',');
                var titles = requestForm["Members.Title"].Split(',');
                var certificationNOs = requestForm["Members.CertificationNO"].Split(',');
                var startWorkingDates = requestForm["Members.StartWorkingDate"].Split(',');
                var conducts = requestForm["Members.Conduct"].Split(',');

                for (var i = 0; i < realnames.Length; i++)
                {
                    var eduRecord = EduRecord.本科;
                    Enum.TryParse<EduRecord>(eduRecords[i], out eduRecord);

                    var major = Major.无;
                    Enum.TryParse<Major>(eduRecords[i], out major);

                    var professionalLevel = ProfessionalLevel.初级;
                    Enum.TryParse<ProfessionalLevel>(professionalLevels[i], out professionalLevel);

                    DateTime startWorkingDate;
                    DateTime.TryParse(startWorkingDates[i], out startWorkingDate);

                    list.Add(new MemberProfile
                    {
                        RealName = realnames[i],
                        Gender = genders[i],
                        IDNo = idnos[i],
                        EduRecord = eduRecord,
                        Major = major,
                        ProfessionalLevel = professionalLevel,
                        Title = titles[i],
                        CertificationNO = certificationNOs[i],
                        StartWorkingDate = startWorkingDate == DateTime.MinValue ? default(DateTime?) : startWorkingDate,
                        Conduct = conducts[i]
                    });
                }
            }
            catch
            {

            }
            return list;
        }

        public static List<UploadFile> GetUploadFiles(this NameValueCollection requestForm)
        {
            var list = new List<UploadFile>();
            try
            {
                var names = requestForm["file.FileName"].Split(',');
                var descs = requestForm["file.Description"].Split(',');
                var savePaths = requestForm["file.SavePath"].Split(',');

                for (var i = 0; i < names.Length; i++)
                {
                    list.Add(new UploadFile
                    {
                        FileName = names[i],
                        Description = descs[i],
                        SavePath = savePaths[i]
                    });
                }

            }
            catch { }

            return list;
        }

        public static List<Software> GetSoftwares(this NameValueCollection requestForm)
        {
            var list = new List<Software>();
            try
            {
                var softwareNames = requestForm["software.Name"].Split(',');
                var softwareNumbers = requestForm["software.Number"].Split(',');
                var softwareSources = requestForm["software.Source"].Split(',');
                var softwarePurposes = requestForm["software.Purpose"].Split(',');
                var softwareNotes = requestForm["software.Note"].Split(',');
                for (var i = 0; i < softwareNames.Length; i++)
                {
                    var number = 0;
                    int.TryParse(softwareNames[i], out number);
                    list.Add(new Software
                    {
                        Name = softwareNames[i],
                        Number = number,
                        Source = softwareSources[i],
                        Purpose = softwarePurposes[i],
                        Note = softwareNotes[i]
                    });
                }
            }
            catch { }
            return list;
        }

        public static List<Equipment> GetEquipments(this NameValueCollection requestForm)
        {
            var list = new List<Equipment>();
            try
            {
                var equipmentNames = requestForm["equipment.Name"].Split(',');
                var equipmentNumbers = requestForm["equipment.Number"].Split(',');
                var equipmentModels = requestForm["equipment.Model"].Split(',');
                var equipmentManufacturers = requestForm["equipment.Manufacturer"].Split(',');
                var equipmentPerformances = requestForm["equipment.Performance"].Split(',');
                var equipmentNotes = requestForm["equipment.Note"].Split(',');
                for (var i = 0; i < equipmentNames.Length; i++)
                {
                    var number = 0;
                    int.TryParse(equipmentNames[i], out number);
                    list.Add(new Equipment
                    {
                        Name = equipmentNames[i],
                        Number = number,
                        Model = equipmentModels[i],
                        Manufacturer = equipmentManufacturers[i],
                        Performance = equipmentPerformances[i],
                        Note = equipmentNotes[i]
                    });
                }
            }
            catch { }

            return list;
        }

        public static List<Shareholder> GetShareholders(this NameValueCollection requestForm)
        {
            var list = new List<Shareholder>();
            try
            {
                var shNames = requestForm["SH.Name"].Split(',');
                var shGenders = requestForm["SH.Gender"].Split(',');
                var shBirthdays = requestForm["SH.Birthday"].Split(',');
                var shShares = requestForm["SH.Shares"].Split(',');
                var shTitles = requestForm["SH.Title"].Split(',');
                var shProfessionals = requestForm["SH.Professionals"].Split(',');

                for (var i = 0; i < shNames.Length; i++)
                {
                    list.Add(new Shareholder
                    {
                        Name = shNames[i],
                        Gender = shGenders[i],
                        Birthday = shBirthdays[i],
                        Shares = shShares[i],
                        Title = shTitles[i],
                        Professionals = shProfessionals[i] == "是"
                    });
                }
            }
            catch { }
            return list;
        }
    }
}