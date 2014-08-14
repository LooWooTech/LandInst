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
        public ActionResult Edit(int? checkLogId, string type)
        {
            //查看历史
            if (checkLogId.HasValue)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId.Value);
                if (checkLog != null)
                {
                    ViewBag.Disabled = true;
                    ViewBag.Profile = Core.InstitutionManager.GetProfile(checkLog);
                }
            }
            else
            {
                var currentInst = GetCurrentInst();
                //只要不是注册登记，那么就获取资料变更的审核状态
                var checkLog = Core.CheckLogManager.GetLastLog(currentInst.ID, CheckType.Profile);
                if (type == "annualcheck")
                {
                    var annualCheck = Core.AnnualCheckManager.GetIndateModel();
                    if (annualCheck != null)
                    {
                        ViewBag.AnnualCheck = annualCheck;
                        ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(currentInst.ID, CheckType.Annual);
                    }
                }
                else
                {
                    ViewBag.CheckLog = checkLog;
                }
                ViewBag.Profile = Core.InstitutionManager.GetProfile(currentInst.ID);
            }
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
        public ActionResult Submit(InstitutionProfile data, string type, bool isSubmit = false)
        {
            var inst = GetCurrentInst();
            try
            {
                var shNames = Request.Form["SH.Name"].Split(',');
                var shGenders = Request.Form["SH.Gender"].Split(',');
                var shBirthdays = Request.Form["SH.Birthday"].Split(',');
                var shShares = Request.Form["SH.Shares"].Split(',');
                var shTitles = Request.Form["SH.Title"].Split(',');
                var shProfessionals = Request.Form["SH.Professionals"].Split(',');

                for (var i = 0; i < shNames.Length; i++)
                {
                    data.ShareHolders.Add(new Shareholder
                    {
                        Name = shNames[i],
                        Gender = shGenders[i],
                        Birthday = shBirthdays[i],
                        Shares = shShares[i],
                        Title = shTitles[i],
                        Professionals = shProfessionals[i] == "是"
                    });
                }
            }
            catch { }
            try
            {
                var memberNames = Request.Form["member.Name"].Split(',');
                var memberGenders = Request.Form["member.Gender"].Split(',');
                var memberBirthdays = Request.Form["member.Birthday"].Split(',');
                var memberPracticeNos = Request.Form["member.PracticeNo"].Split(',');
                var memberMobiles = Request.Form["member.MobilePhone"].Split(',');

                for (var i = 0; i < memberNames.Length; i++)
                {
                    data.Members.Add(new InstMember
                    {
                        Name = memberNames[i],
                        Gender = memberGenders[i],
                        Birthday = memberBirthdays[i],
                        PracticeNo = memberPracticeNos[i],
                        MobilePhone = memberMobiles[i]
                    });
                }
            }
            catch { }
            try
            {
                var equipmentNames = Request.Form["equipment.Name"].Split(',');
                var equipmentNumbers = Request.Form["equipment.Number"].Split(',');
                var equipmentModels = Request.Form["equipment.Model"].Split(',');
                var equipmentManufacturers = Request.Form["equipment.Manufacturer"].Split(',');
                for (var i = 0; i < equipmentNames.Length; i++)
                {
                    var number = 0;
                    int.TryParse(equipmentNames[i], out number);
                    data.Equipments.Add(new Equipment
                    {
                        Name = equipmentNames[i],
                        Number = number,
                        Model = equipmentModels[i],
                        Manufacturer = equipmentManufacturers[i]
                    });
                }
            }
            catch { }

            if (isSubmit)
            {
                if (type == "annualcheck")
                {
                    Core.InstitutionManager.SubmitAnnaulCheck(inst, data);
                }
                else
                {
                    Core.InstitutionManager.SubmitProfile(inst, data);
                }
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
