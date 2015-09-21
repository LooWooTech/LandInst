﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class EducationController : InstitutionControllerBase
    {
        public ActionResult Index(string name, bool? result, int page = 1)
        {
            var filter = new Model.Filters.MemberFilter
            {
                CheckType = Model.CheckType.Education,
                Keyword = name,
                Result = result,
                InstID = Identity.UserID,
                Page = new Model.Filters.PageFilter { PageIndex = page }
            };
            ViewBag.List = Core.EducationManager.GetApprovalEducations(filter);
            ViewBag.PageFilter = filter;
            return View();
        }

        public ActionResult Import()
        {
            ViewBag.Educations = Core.EducationManager.GetEducations().Where(e => e.EndDate > DateTime.Now).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Submit(string realNames, int eduId = 0)
        {
            if (eduId == 0)
            {
                throw new ArgumentException("没有选择具体的继续教育");
            }

            if (string.IsNullOrEmpty(realNames))
            {
                throw new ArgumentException("用户参数错误");
            }

            var memberIds = new List<int>();

            if (!string.IsNullOrEmpty(realNames))
            {
                var names = realNames.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                memberIds = Core.MemberManager.GetMemberIds(names, Identity.UserID);
            }

            if (memberIds.Count == 0)
            {
                throw new ArgumentException("请输入正确的用户姓名");
            }

            foreach (var id in memberIds)
            {
                Core.EducationManager.SignupEducation(eduId, id);
                //var checkLog = Core.CheckLogManager.GetLastLog(id, Model.CheckType.Education);
                //if (checkLog == null || checkLog.Result == false)
                //{
                //    Core.CheckLogManager.AddCheckLog(eduId, id, Model.CheckType.Education);
                //}
            }
            return JsonSuccess();
        }
    }
}
