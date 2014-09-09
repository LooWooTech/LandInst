using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Common;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using System.IO;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class InstitutionController : AdminControllerBase
    {
        public ActionResult Index(string name, string city, int page = 1)
        {
            var filter = new InstitutionFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = CheckType.Profile,
                City = city
            };

            ViewBag.List = Core.InstitutionManager.GetInstitutions(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult CheckAnnual(string name, bool? hasCheck, int annualCheckId = 0, int page = 1)
        {
            var filter = new CheckLogFilter
            {
                InfoID = annualCheckId,
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                HasCheck = hasCheck,
            };
            ViewBag.AnnualChecks = Core.AnnualCheckManager.GetAnnualChecks();
            ViewBag.List = Core.AnnualCheckManager.GetVCheckAnnual(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult CheckProfile(string name, bool? hasCheck, int page = 1)
        {
            var filter = new InstitutionFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = CheckType.Profile,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.InstitutionManager.GetVCheckInsts(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.InstitutionManager.GetInstitution(id) ?? new Model.Institution();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (Core.UserManager.Exists(user.Username))
            {
                throw new ArgumentException("用户名已被使用！");
            }

            var randomPwd = StringHelper.GenerateRandomString(8);
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = randomPwd;
            }
            else
            {
                randomPwd = user.Password;
            }


            user.Role = UserRole.Institution;
            Core.UserManager.AddUser(user);
            var inst = new Model.Institution
            {
                ID = user.ID,
                Name = user.Username,
            };
            Core.InstitutionManager.AddInstitution(inst);
            return JsonSuccess(new { password = randomPwd });
        }

        //public ActionResult Approval(string id, bool result = true)
        //{
        //    var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_id => int.Parse(_id)).ToArray();
        //    foreach (var _id in ids)
        //    {
        //        Core.ApprovalManager.UpdateApproval(_id, result);
        //    }
        //    return JsonSuccess();
        //}

        public ActionResult Profile(int id, int checkLogId = 0)
        {
            var inst = Core.InstitutionManager.GetInstitution(id);
            if (inst == null)
            {
                throw new ArgumentNullException("参数错误，没找到这个机构。");
            }
            CheckLog checkLog = null;
            if (checkLogId == 0)
            {
                checkLog = Core.CheckLogManager.GetLastLog(id, CheckType.Profile);
                if (checkLog == null || checkLog.Result.HasValue)
                {
                    checkLog = Core.CheckLogManager.GetLastLog(id, CheckType.Annual);
                }
            }
            else
            {
                checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            }

            ViewBag.CheckLog = checkLog;

            ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog) ?? Core.InstitutionManager.GetProfile(inst.ID) ?? new InstitutionProfile(inst);

            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);

            return View();
        }

        public ActionResult Approval(int id, CheckLog data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(id);
            if (checkLog == null)
            {
                throw new ArgumentException("参数错误");
            }
            checkLog.Note = data.Note;
            checkLog.Result = data.Result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);

            if (checkLog.CheckType == CheckType.Profile || checkLog.CheckType == CheckType.Annual)
            {
                Core.InstitutionManager.ApprovalInst(checkLog);
                var inst = Core.InstitutionManager.GetInstitution(checkLog.UserID);
                if (checkLog.Result == true && inst.Status == InstitutionStatus.Normal)
                {
                    Core.InstitutionManager.UpdateStatus(checkLog.UserID, InstitutionStatus.Registered);
                }
            }
            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(string password, string rePassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("初始密码没有填写");
            }

            if (string.IsNullOrEmpty(rePassword))
            {
                throw new ArgumentException("确认初始密码没有填写");
            }

            if (password != rePassword)
            {
                throw new ArgumentException("两次输入密码不一致");
            }


            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("你没有选择上传文件");
            }
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                throw new ArgumentException("没有选择上传文件");
            }

            var filePath = Core.FileManager.Upload(HttpContext, file);
            var rows = ExcelHelper.ReadExcelData(Request.MapPath(filePath), 1);

            //Func<List<object>,int,

            var errors = new List<string>();

            //0机构名称	1城市	2机构性质	3"注册资金"	4工商登记号	5税务登记号
            //6工商登记机关	7成立日期	8"法人代表" 9法人代码	10执业注册号
            //11团体会员证号	12"证书级别"	13电子信箱	14公司网站	15联系电话	
            //16手机	17传真电话 18公司地址	19联系地址	20邮编	
            //21机构总人数	22专业人员数	23中级及以上专业人员数

            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                if (row[0] == null)
                {
                    errors.Add("第" + (i + 2) + "行 机构名称为空");
                    continue;
                }
                var name = row[0].ToString();
                if (Core.UserManager.Exists(name))
                {
                    errors.Add(name + "机构名称已存在");
                    continue;
                }

                var profile = new InstitutionProfile
                {
                    Name = name,
                    City = row[1].AsString(),
                    CompanyType = row[2].AsString(),
                    RegisteredCapital = row[3].AsNullOrInt(),
                    RegistrationNo = row[4].AsString(),
                    TaxRegistryNo = row[5].AsString(),
                    RegistrationInstitution = row[6].AsString(),
                    EstablishedDate = row[7].AsNullOrDate(),
                    LegalPerson = row[8].AsString(),
                    LegalpersonNo = row[9].AsString(),
                    PracticeRegistrationNo = row[10].AsString(),
                    CorporateMemberNo = row[11].AsString(),
                    ExequaturLevel = row[12].AsString(),
                    HasExequatur = !string.IsNullOrEmpty(row[12].AsString()),
                    Email = row[13].AsString(),
                    Website = row[14].AsString(),
                    Tel = row[15].AsString(),
                    MobilePhone = row[16].AsString(),
                    Fax = row[17].AsString(),
                    Address = row[18].AsString(),
                    Address1 = row[19].AsString(),
                    Postcode = row[20].AsString(),
                    Postcode1 = row[20].AsString(),
                    TotalMembers = row[21].AsNullOrInt(),
                    ProMembers = row[22].AsNullOrInt(),
                    ExpertMembers = row[23].AsNullOrInt(),
                };

                var user = new User
                {
                    Username = profile.Name,
                    Password = password.MD5(),
                    Role = UserRole.Institution,
                };

                Core.UserManager.AddUser(user);
                profile.ID = user.ID;
                Core.InstitutionManager.Import(user, profile);

            }


            return JsonSuccess(errors);
        }

        public void Export(int id, int checkLogId = 0)
        {
            var inst = Core.InstitutionManager.GetInstitution(id);
            if (inst == null)
            {
                throw new ArgumentException("没有找到这个机构");
            }
            var filePath = Request.MapPath("/templates/勘测机构导出模板.xls");
            var profile = Core.InstitutionManager.GetExportProfile(id, checkLogId);

            var excel = ExcelHelper.GetExcel(filePath);
            Core.InstitutionManager.UpdateExcel(excel, profile);
            var exportDatas = Core.InstitutionManager.GetExportData(profile);
            var stream = excel.ToStream(exportDatas);

            Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(inst.Name) + ".xls"));
            Response.BinaryWrite(((MemoryStream)stream).GetBuffer());
            Response.End();
        }
    }
}