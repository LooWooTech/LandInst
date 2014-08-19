﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class EducationController : AdminControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            ViewBag.List = Core.EducationManager.GetEducations();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.EducationManager.GetEducatoin(id) ?? new Education();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Education education)
        {
            Core.EducationManager.SaveEducation(education);
            return JsonSuccess();
        }

        public ActionResult Approvals(string name, bool? hasCheck, int? eduId = 0, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                InfoID = eduId,
                HasCheck= hasCheck,
            };
            ViewBag.List = Core.EducationManager.GetApprovalEducations(filter);
            return View();
        }

        public ActionResult Delete(int id)
        {
            Core.EducationManager.Delete(id);
            return JsonSuccess();
        }

        //public ActionResult Approval(string id, CheckType type, bool result = true)
        //{
        //    var ids = id.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(_id => int.Parse(_id)).ToArray();
        //    foreach (var _id in ids)
        //    {
        //        var approval = Core.CheckLogManager.GetCheckLog(_id);
                
        //        Core.EducationManager.Approval(_id, result);
        //    }

        //    return JsonSuccess();
        //}

        private void UpdateMember()
        { 
        
        }

        private void UpdateMemberTransfer()
        { 
            
        }
    }
}
