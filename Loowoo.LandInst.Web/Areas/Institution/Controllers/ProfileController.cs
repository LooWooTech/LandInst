using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class ProfileController : InstitutionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var profile = Core.InstitutionManager.GetProfile(GetCurrentInst());
            ViewBag.Profile = profile;
            //只要不是注册登记，那么就获取资料变更的审核状态
            ViewBag.Approval = Core.ApprovalManager.GetApproval(profile.ID, Identity.UserID, profile.Status == InstitutionStatus.Normal ? ApprovalType.Register : ApprovalType.Change);
            return View();
        }

        //[HttpPost]
        //public ActionResult Edit(InstitutionProfile profile, string mode)
        //{
        //    var currentInst = GetCurrentInst();
        //    Core.InstitutionManager.SaveProfile(profile);
        //    if (mode == "submit")
        //    {
        //        var approvalType = currentInst.Status == InstitutionStatus.Normal ? ApprovalType.Register : ApprovalType.Change;
        //        Core.ApprovalManager.AddApproval(Identity.UserID, Identity.UserID, approvalType);
        //    }

        //    return JsonSuccess();
        //}

        [HttpPost]
        public ActionResult Submit(InstitutionProfile profile, string type)
        {
            var currentInst = GetCurrentInst();

            if (type == "register")
            {
                if (currentInst.CanSubmitRegisterApproval)
                {
                    Core.InstitutionManager.SaveProfile(profile);
                    Core.ApprovalManager.AddApproval(currentInst.ID, Identity.UserID, ApprovalType.Register);
                }
            }
            else if (type == "change")
            {
                if (currentInst.CanSubmitChangeApproval)
                {
                    Core.InstitutionManager.SaveProfile(profile);
                    Core.ApprovalManager.AddApproval(currentInst.ID, Identity.UserID, ApprovalType.Change);
                }
            }
            else
            { 
            }
            return JsonSuccess();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout(string fullName)
        {
            var currentInst = GetCurrentInst();
            if (currentInst.FullName == fullName)
            {
                Core.InstitutionManager.LogoutInstitution(fullName);
                return JsonSuccess();
            }
            throw new ArgumentException("公司全称输入不正确");
        }

        public ActionResult History()
        {
            return View();
        }
    }
}
