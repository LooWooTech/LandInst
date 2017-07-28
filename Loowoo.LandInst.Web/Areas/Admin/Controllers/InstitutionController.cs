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

        public new ActionResult Profile(int id, int checkLogId = 0)
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
                if (checkLog == null)
                {
                    checkLog = Core.CheckLogManager.GetLastLog(id, CheckType.Annual);
                }
                else if (checkLog.Result == true)
                {
                    var annualCheck = Core.CheckLogManager.GetLastLog(id, CheckType.Annual);
                    if (annualCheck != null && annualCheck.Result == true && annualCheck.CreateTime > checkLog.CreateTime)
                    {
                        checkLog = annualCheck;
                    }
                }
            }
            else
            {
                checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            }

            ViewBag.CheckLog = checkLog;

            ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog) ?? Core.InstitutionManager.GetProfile(inst.ID) ?? new InstitutionProfile(inst);

            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);

            ViewBag.Members = Core.MemberManager.GetMembers(new MemberFilter { InstID = id, InInst = true });

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
            Core.CheckLogManager.ApprovalCheckLog(checkLog);

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

            var rows = NOPIHelper.ReadExcelData(file.InputStream, 1);

            //Func<List<object>,int,

            var errors = new List<string>();
            //0机构名称	
            //1所在城市	2机构性质	 3法人代表  4技术负责人	 5工商登记号	 6税务登记号	 7工商登记机关	8注册资金（万元）	9成立日期	 10企业法人营业执照注册号
            //11电子信箱	 12公司网站	13联系电话	 14手机	15传真电话 	16公司地址 	17联系地址 	18邮编	  19在职职工总数	从事土地规划工作人员总数	
            //21从事土地规划工作办公用房面积（平方）
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
                    City = row[1].AsNullOrString(),
                    CompanyType = row[2].AsNullOrString(),
                    LegalPerson = row[3].AsNullOrString(),
                    TechLeader = row[4].AsNullOrString(),
                    RegistrationNo = row[5].AsNullOrString(),
                    TaxRegistryNo = row[6].AsNullOrString(),
                    RegistrationInstitution = row[7].AsNullOrString(),
                    RegisteredCapital = row[8].AsNullOrInt(),
                    EstablishedDate = row[9].AsNullOrDate(),
                    LegalpersonNo = row[10].AsNullOrString(),
                    Email = row[11].AsNullOrString(),
                    Website = row[12].AsNullOrString(),
                    Tel = row[13].AsNullOrString(),
                    MobilePhone = row[14].AsNullOrString(),
                    Fax = row[15].AsNullOrString(),
                    Address = row[16].AsNullOrString(),
                    Address1 = row[17].AsNullOrString(),
                    Postcode = row[18].AsNullOrString(),
                    Postcode1 = row[18].AsNullOrString(),
                    TotalMembers = row[19].AsNullOrInt(),
                    ExpertMembers = row[20].AsNullOrInt(),
                    OfficeArea = row[21].AsNullOrInt(),
                };

                var user = new User
                {
                    Username = profile.Name,
                    Password = password,
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
            var filePath = Request.MapPath("/templates/规划机构导出模板.xls");
            var exportData = Core.InstitutionManager.GetExportData(id, checkLogId);
            if (exportData == null)
            {
                throw new Exception("该机构还未通过审核");
            }
            var stream = NOPIHelper.WriteCell(filePath, exportData);
            Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(inst.Name) + ".xls"));
            Response.BinaryWrite(((MemoryStream)stream).GetBuffer());
            Response.End();
        }

        public ActionResult Delete(int id)
        {
            Core.InstitutionManager.Delete(id);
            return JsonSuccess();
        }
    }
}
