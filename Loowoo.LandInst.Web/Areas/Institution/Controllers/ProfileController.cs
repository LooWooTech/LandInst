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
        public ActionResult Index(int? checkLogId)
        {
            if (checkLogId.HasValue)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId.Value);
                if (checkLog != null)
                {
                    ViewBag.CheckLog = checkLog;
                    ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog);
                }
            }
            else
            {
                ViewBag.Profile = Core.InstitutionManager.GetProfile(Identity.UserID);
            }
            return View();
        }

        public ActionResult AnnualCheck()
        {
            var currentInst = GetCurrentInst();
            var annualCheck = Core.AnnualCheckManager.GetIndateModel();
            if (annualCheck != null)
            {
                ViewBag.AnnualCheck = annualCheck;
                var checkLog = Core.CheckLogManager.GetLastLog(currentInst.ID, CheckType.Annual);
                ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog) ?? Core.InstitutionManager.GetProfile(currentInst.ID);
                ViewBag.CheckLog = checkLog;
            }
            else
            {
                ViewBag.Profile = Core.InstitutionManager.GetProfile(currentInst.ID);
            }
            return View();
        }


        [HttpGet]
        public ActionResult Edit()
        {
            var currentInst = GetCurrentInst();
            ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(currentInst.ID, CheckType.Profile);
            ViewBag.Profile = Core.InstitutionManager.GetProfile(currentInst.ID);
            return View();
        }



        [HttpPost]
        public ActionResult Submit(InstitutionProfile data, CheckType? type, bool isSubmit = false)
        {
            var inst = GetCurrentInst();

            data.ID = inst.ID;
            data.ShareHolders = Shareholder.GetList(Request.Form);
            data.Equipments = Equipment.GetList(Request.Form);
            data.Members = InstMember.GetList(Request.Form);
            data.Files = UploadFile.GetList(Request.Form);

            if (type == CheckType.Annual)
            {
                Core.InstitutionManager.SubmitAnnaulCheck(inst, data);
            }
            else if (type == CheckType.Profile)
            {
                Core.InstitutionManager.SubmitProfile(inst, data);
            }
            else
            {
                Core.InstitutionManager.SaveProfile(inst, data);
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
            if (currentInst.Name == fullName)
            {
                Core.InstitutionManager.LogoutInstitution(fullName);
                return JsonSuccess();
            }
            throw new ArgumentException("公司全称输入不正确");
        }

        public ActionResult History()
        {
            ViewBag.List = Core.InstitutionManager.GetProfileHistory(Identity.UserID);
            return View();
        }
    }
}