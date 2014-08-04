using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class MemberController : InstitutionControllerBase
    {
        public ActionResult Index(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                InstID = string.IsNullOrEmpty(name) ? Identity.UserID : 0,
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public new ActionResult Profile(int id)
        {
            return View();
        }

        public ActionResult Search(bool? limit)
        {
            ViewBag.InstID = limit == true ? GetCurrentInst().ID : 0;
            return View();
        }

        public ActionResult SearchResult(string target, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return JsonSuccess(null);
            }



            var filter = new MemberFilter
            {
                Keyword = keyword,
            };

            switch(target.ToLower())
            {
                case "transferout":
                case "practice":
                    filter.InstID = GetCurrentInst().ID;
                    break;
            }

            return JsonSuccess(Core.MemberManager.GetMembers(filter));
        }


        [HttpGet]
        public ActionResult TransferIn(int memberId = 0)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Search", new { target = "TransferIn" });
            }
            ViewBag.Member = Core.MemberManager.GetMember(memberId);
            return View();
        }

        [HttpGet]
        public ActionResult TransferOut(int memberId = 0)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Search", new { target = "TransferOut" });
            }
            ViewBag.Member = Core.MemberManager.GetMember(memberId);
            return View();
        }


        [HttpPost]
        public ActionResult Transfer(int memberId, TransferMode mode, int instId = 0)
        {
            var member = Core.MemberManager.GetMember(memberId);
            if (member == null)
            {
                throw new ArgumentException("没有选择有效用户");
            }

            if (mode == TransferMode.Out)
            {
                if (member.InstitutionID != Identity.UserID)
                {
                    throw new ArgumentException("你没有权限转移此用户");
                }
                Core.TransferManager.Submit(member, instId, mode);
            }
            else
            {
                Core.TransferManager.Submit(member, Identity.UserID, mode);
            }


            return JsonSuccess();
        }

        public ActionResult Transfers(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                Type = CheckType.Transfer

            };

            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public ActionResult Practices(string name, bool? result, int page = 1)
        {
            var currentInst = GetCurrentInst();
            var filter = new MemberFilter
            {
                InstID = currentInst.ID,
                Keyword = name,
                Result = result,
                Type = CheckType.Practice,
                Page = new PageFilter { PageIndex = page }
            };
            ViewBag.List = Core.MemberManager.GetVCheckMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        [HttpGet]
        public ActionResult Practice(int memberId = 0)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Search", new { target = "TransferOut" });
            }

            var currentInst = GetCurrentInst();
            ViewBag.Member = Core.MemberManager.GetMember(memberId);
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Practice);
            if (checkLog != null && checkLog.Result != false)
            {
                ViewBag.CheckLog = checkLog;
                ViewBag.Practice = Core.PracticeManager.GetPracticeInfo(memberId);
            }
            return View();
        }


        [HttpPost]
        public ActionResult Practice(int memberId, PracticeInfo data)
        {
            var currentInst = GetCurrentInst();
            var member = Core.MemberManager.GetMember(memberId);
            if (member.InstitutionID != currentInst.ID)
            {
                throw new HttpException(401, "你不能为此会员申请执业登记");
            }
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Practice);
            if (checkLog == null || checkLog.Result == false)
            {
                try
                {
                    var certNames = Request.Form["Cert.Name"].Split(',');
                    var certNos = Request.Form["Cert.No"].Split(',');
                    var certLevel = Request.Form["Cert.Level"].Split(',');
                    for (var i = 0; i < certNames.Length; i++)
                    {
                        data.Certifications.Add(new Certification
                        {
                            Name = certNames[i],
                            CertificationNo = certNos[i],
                            CertificationLevel = certLevel[i]
                        });
                    }
                }
                catch { }

                Core.PracticeManager.SavePracticeInfo(memberId, data);
                Core.CheckLogManager.AddCheckLog(memberId, memberId, CheckType.Practice);
            }


            return JsonSuccess();
        }

    }
}
