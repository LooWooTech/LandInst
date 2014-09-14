using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class MemberController : AdminControllerBase
    {

        public ActionResult Index(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                Page = new Model.Filters.PageFilter { PageIndex = page },
                GetInstName = true,
            };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Search(string name)
        {
            var list = Core.MemberManager.GetMembers(new MemberFilter
            {
                Keyword = name
            });
            return JsonSuccess(list);
        }


        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportResult()
        {
            //0机构全称 1真实姓名	2性别	3出生年月	4证件号码	5专业	6学历	7学位	8毕业学校	9民族	
            //10籍贯	    11职称	12政治面貌	13手机	14邮箱	15通信地址	16邮编
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                throw new ArgumentException("没有选择上传文件");
            }
            var file = Request.Files[0];

            var filePath = Core.FileManager.Upload(HttpContext, file);
            var errors = new List<string>();
            var rows = NOPIHelper.ReadExcelData(Request.MapPath(filePath), 1);
            for (var i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                var instName = row[0].AsNullOrString();
                var instId = Core.InstitutionManager.GetInstId(instName);
                if (instId == 0)
                {
                    errors.Add(instName + "机构名称不存在");
                    continue;
                }

                var member = new Member
                {
                    InstitutionID = instId,
                    RealName = row[1].AsNullOrString(),
                    Gender = row[2].AsNullOrString() ?? "男",
                    Birthday = row[3].AsNullOrDate(),
                    IDNo = row[4].AsNullOrString(),
                    Major = row[5].AsEnum<Major>(),
                    EduRecord = row[6].AsEnum<EduRecord>(),
                    Email = row[14].AsNullOrString(),
                    MobilePhone = row[13].AsNullOrString(),
                    Status = MemberStatus.Registered,
                };

                var profile = new MemberProfile(member)
                {
                    EduLevel = row[7].AsNullOrString(),
                    School = row[8].AsNullOrString(),
                    Nationality = row[9].AsNullOrString(),
                    NativePlace = row[10].AsNullOrString(),
                    ProfessionalLevel = row[11].AsEnum<ProfessionalLevel>(),
                    PoliticalState = row[12].AsNullOrString(),
                    Address = row[15].AsNullOrString(),
                    Postcode = row[16].AsNullOrString()
                };


                if (string.IsNullOrEmpty(member.RealName))
                {
                    errors.Add("第" + (i + 1) + "行用户姓名没有填写");
                    continue;
                }

                if (string.IsNullOrEmpty(member.IDNo))
                {
                    errors.Add("第" + (i + 1) + "行证件号码没有填写");
                    continue;
                }

                if (Core.MemberManager.Exist(member.IDNo))
                {
                    errors.Add("第" + (i + 1) + "行" + member.RealName + "已存在");
                    continue;
                }

                Core.MemberManager.Import(member, profile);

            }

            return JsonSuccess();
        }


        public ActionResult Approvals(string name, CheckType? type, bool? hasCheck, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = type,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
            switch (filter.CheckType)
            {
                default:
                    //case CheckType.Profile:
                    //case CheckType.Practice:
                    return View();
                case CheckType.Transfer:
                    return RedirectToAction("Transfers");
            }
        }

        public ActionResult Transfers(string name, bool? hasCheck, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = CheckType.Transfer,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.MemberManager.GetVCheckTransfers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Approval(int id, CheckLog data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(id);
            checkLog.Note = data.Note;
            checkLog.Result = data.Result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);
            switch (checkLog.CheckType)
            {
                case CheckType.Exam:
                    Core.ExamManager.Approval(checkLog);
                    break;
                case CheckType.Education:
                    break;
                case CheckType.Transfer:
                    Core.MemberManager.ApprovalTransfer(checkLog);
                    break;
                case CheckType.Practice:
                case CheckType.Profile:
                    Core.MemberManager.ApprovalMember(checkLog);
                    break;
            }
            return JsonSuccess();
        }

        public new ActionResult Profile(int id, int checkLogId = 0)
        {
            if (id == 0)
            {
                return View();
            }

            var member = Core.MemberManager.GetMember(id);
            if (member == null)
            {
                throw new ArgumentException("参数不正确");
            }

            CheckLog checkLog = null;
            if (checkLogId > 0)
            {
                checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            }
            else
            {
                checkLog = Core.CheckLogManager.GetLastLog(id);
            }

            ViewBag.CheckLog = checkLog;


            var profile = Core.MemberManager.GetProfile(checkLog) ?? Core.MemberManager.GetProfile(id) ?? new MemberProfile(member);

            profile.SetMemberField(member);
            ViewBag.Member = member;
            ViewBag.Profile = profile;
            ViewBag.ExamResults = Core.ExamManager.GetVExamResults(new MemberFilter { UserID = id });
            ViewBag.Educations = Core.EducationManager.GetMemberEducations(id);
            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);


            return View();
        }


        [HttpGet]
        public ActionResult ResetPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPwd(int memberId, string newPwd)
        {
            Core.UserManager.ResetPwd(memberId, newPwd);

            return JsonSuccess();
        }
    }
}
