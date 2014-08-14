using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Common;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

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
            ViewBag.List = Core.InstitutionManager.GetApprovalInsts(filter);
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
        public ActionResult Edit(User user, Model.Institution inst)
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

            if (string.IsNullOrEmpty(inst.Name))
            {
                throw new ArgumentException("机构名称没有填写");
            }

            user.Role = UserRole.Institution;
            Core.UserManager.AddUser(user);
            inst.ID = user.ID;
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

            ViewBag.Members = Core.MemberManager.GetMembers(new MemberFilter { InstID = id, InInst = true, IncludeNoHaveInstMember = false });

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
                var inst = Core.InstitutionManager.GetInstitution(checkLog.UserID);
                Core.InstitutionManager.ApprovalInst(checkLog);
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
        public ActionResult ImportResult()
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentException("你没有选择上传文件");
            }
            var file = Request.Files[0];
            var filePath = Core.FileManager.Upload(HttpContext, file);
            var columns = NOPIHelper.ReadSimpleColumns(filePath);
            return JsonSuccess();
        }
    }
}
