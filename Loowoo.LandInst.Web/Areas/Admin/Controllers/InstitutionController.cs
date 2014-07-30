﻿using System;
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

        public ActionResult CheckAnnual(string name, int annualCheckId = 0, bool? result = null, int page = 1)
        {
            var filter = new CheckLogFilter
            {
                InfoID = annualCheckId,
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                Result = result
            };
            ViewBag.AnnualChecks = Core.AnnualCheckManager.GetAnnualChecks();
            ViewBag.List = Core.AnnualCheckManager.GetVCheckAnnual(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult CheckProfile(string name, bool? result = null, int page = 1)
        {
            var filter = new InstitutionFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                CheckType = CheckType.Profile,
                Result = result
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
            }
            else
            {
                checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
            }

            ViewBag.CheckLog = checkLog;

            ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog);

            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);

            return View();
        }

        public ActionResult Approval(int id, CheckLog data)
        {
            var checkLog = Core.CheckLogManager.GetCheckLog(id);
            checkLog.Note = data.Note;
            checkLog.Result = data.Result;
            Core.CheckLogManager.UpdateCheckLog(checkLog);

            if (checkLog.CheckType == CheckType.Profile)
            {
                var inst = Core.InstitutionManager.GetInstitution(checkLog.UserID);
                if (checkLog.Result == true && inst.Status == InstitutionStatus.Normal)
                {
                    Core.InstitutionManager.UpdateStatus(checkLog.UserID, InstitutionStatus.Registered);
                }
                var profie = Core.InstitutionManager.GetProfile(checkLog);
                Core.InstitutionManager.UpdateInstitution(profie);
            }
            return JsonSuccess();
        }
    }
}
