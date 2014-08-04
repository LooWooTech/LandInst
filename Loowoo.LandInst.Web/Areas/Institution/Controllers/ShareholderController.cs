using Loowoo.LandInst.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class ShareholderController : InstitutionControllerBase
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
            ViewBag.Model = profile.ShareHolders.FirstOrDefault(e => e.ID == id) ?? new Shareholder();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string id, Shareholder data)
        {
            var profile = GetProfile();
            CheckLog checkLog = ViewBag.CheckLog;
            var model = profile.ShareHolders.FirstOrDefault(e => e.Name == id);
            if (model == null)
            {
                profile.ShareHolders.Add(data);
            }
            else
            {
                model.Name = data.Name;
                model.Mobile = data.Mobile;
                model.Gender = data.Gender;
                model.Birthday = data.Birthday;
                model.Shares = data.Shares;
            }

            Core.InstitutionManager.SubmitProfile(Identity.UserID, profile);
            return JsonSuccess();
        }

        public ActionResult Delete(string id)
        {
            var profile = GetProfile();
            CheckLog checkLog = ViewBag.CheckLog;
            var index = profile.ShareHolders.FindIndex(e => e.ID == id);
            if (index > -1)
            {
                profile.ShareHolders.RemoveAt(index);
            }
            Core.InstitutionManager.SubmitProfile(Identity.UserID, profile);
            return JsonSuccess();
        }
    }
}
