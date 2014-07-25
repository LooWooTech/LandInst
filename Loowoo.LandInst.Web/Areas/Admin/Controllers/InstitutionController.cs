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
        public ActionResult Index(string name, CheckType? type, int page = 1)
        {
            if (type.HasValue)
            {
                var filter = new InstitutionFilter
                {
                    Keyword = name,
                    PageIndex = page,
                    ApprovalType = type.Value
                };
                ViewBag.List = Core.InstitutionManager.GetApprovalInsts(filter);
                ViewBag.Page = filter;
            }
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

        public ActionResult Profile(int id)
        {
            var inst = Core.InstitutionManager.GetInstitution(id);
            if (inst == null)
            {
                throw new ArgumentNullException("参数错误，没找到这个机构。");
            }

            ViewBag.Profile = Core.InstitutionManager.GetProfile(inst.ID);

            ViewBag.Approvals = Core.CheckLogManager.GetList(id);

            return View();
        }

        public ActionResult Approval(int id,bool result)
        {
            Core.CheckLogManager.UpdateCheckLog(id, result);
            return JsonSuccess();
        }
    }
}
