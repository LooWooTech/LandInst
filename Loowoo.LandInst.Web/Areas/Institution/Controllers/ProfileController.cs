using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Common;
using System.IO;

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
                ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog) ?? Core.InstitutionManager.GetProfile(currentInst.ID, true);
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
            var profile = Core.InstitutionManager.GetProfile(inst.ID);
            if (type == CheckType.Annual)
            {
                profile.AnnualCheckProfile = Request.Form.GetAnnualProfile();
                profile.Members = Request.Form.GetMembers();
            }
            else
            {
                data.ShareHolders = Request.Form.GetShareholders();
                data.Equipments = Request.Form.GetEquipments();
                data.Softwares = Request.Form.GetSoftwares();
                data.Files = Request.Form.GetUploadFiles();
                data.Members = Request.Form.GetMembers();
                if (profile != null)
                {
                    data.AnnualCheckProfile = profile.AnnualCheckProfile;
                }
                profile = data;
            }
            if (isSubmit)
            {
                if (type == CheckType.Annual)
                {
                    Core.InstitutionManager.SubmitAnnaulCheck(inst, profile);
                }
                else
                {
                    Core.InstitutionManager.SubmitProfile(inst, profile);
                }
            }
            else
            {
                Core.InstitutionManager.SaveProfile(inst, profile);
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

        public void Export(int id, int checkLogId = 0)
        {
            var inst = GetCurrentInst();
            var filePath = Request.MapPath("/templates/规划机构导出模板.xls");
            var exportData = Core.InstitutionManager.GetExportData(id, checkLogId);
            var stream = NOPIHelper.WriteCell(filePath, exportData);
            Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(inst.Name) + ".xls"));
            Response.BinaryWrite(((MemoryStream)stream).GetBuffer());
            Response.End();
        }
    }
}
