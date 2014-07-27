using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class CertificationController : InstitutionControllerBase
    {
        private void SetViewBag()
        {
            var checkLog = Core.CheckLogManager.GetLastLog(Identity.UserID, Model.CheckType.Profile);
            var profile = Core.ProfileManager.GetProfile<InstitutionProfile>(checkLog.InfoID);
            ViewBag.CheckLog = checkLog;
            ViewBag.Profile = profile;

        }

        private InstitutionProfile GetProfile()
        {
            SetViewBag();
            InstitutionProfile profile = ViewBag.Profile;
            return profile;
        }

        public ActionResult Index()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var profile = GetProfile();
            ViewBag.Model = profile.Certifications.FirstOrDefault(e => e.ID == id) ?? new Certification();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string id, Certification data)
        {
            var profile = GetProfile();
            CheckLog checkLog = ViewBag.CheckLog;
            var model = profile.Certifications.FirstOrDefault(e => e.ID == id);
            if (model == null)
            {
                profile.Certifications.Add(data);
            }
            else
            {
                model.Name = data.Name;
                model.CertificationLevel = data.CertificationLevel;
                model.CertificationNo = data.CertificationNo;
            }

            Core.InstitutionManager.SubmitProfile(Identity.UserID, checkLog, profile);
            return JsonSuccess();
        }

        public ActionResult Delete(string id)
        {
            var profile = GetProfile();
            CheckLog checkLog = ViewBag.CheckLog;
            var index = profile.Certifications.FindIndex(e => e.ID == id);
            if (index > -1)
            {
                profile.Certifications.RemoveAt(index);
            }
            Core.InstitutionManager.SubmitProfile(Identity.UserID, checkLog, profile);
            return JsonSuccess();
        }

    }
}
