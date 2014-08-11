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
                InInst = true,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        public new ActionResult Profile(int id)
        {
            var inst = GetCurrentInst();
            var member = Core.MemberManager.GetMember(id);
            var profile = Core.MemberManager.GetProfile(id);
            profile.SetMemberField(member);
            ViewBag.Profile = profile;

            //ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(id, CheckType.Practice);
            ViewBag.Practice = Core.PracticeManager.GetPracticeInfo(id, inst.ID);
            ViewBag.ChecLogs = Core.CheckLogManager.GetList(id);
            ViewBag.ExamResults = Core.ExamManager.GetMemberExamResult(id);
            ViewBag.Educations = Core.EducationManager.GetMemberEducations(id);
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
                InstID = GetCurrentInst().ID
            };

            switch (target.ToLower())
            {
                case "transferout":
                    filter.InInst = true;
                    break;
                case "transferin":
                    filter.InInst = false;
                    break;
                case "practice":
                    filter.InInst = true;
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
            ViewBag.MemberProfile = Core.MemberManager.GetProfile(memberId);
            return View();
        }

        [HttpGet]
        public ActionResult TransferOut(int memberId = 0)
        {
            if (memberId == 0)
            {
                return RedirectToAction("Search", new { target = "TransferOut" });
            }
            ViewBag.MemberProfile = Core.MemberManager.GetProfile(memberId);
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
                return RedirectToAction("Search", new { target = "Practice" });
            }

            var currentInst = GetCurrentInst();
            ViewBag.MemberProfile = Core.MemberManager.GetProfile(memberId);
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Practice);
            ViewBag.Institution = currentInst;
            ViewBag.CheckLog = checkLog;
            ViewBag.Practice = Core.PracticeManager.GetPracticeInfo(memberId, currentInst.ID);
            return View();
        }


        [HttpPost]
        public ActionResult Practice(int memberId, PracticeInfo data)
        {
            var currentInst = GetCurrentInst();
            var member = Core.MemberManager.GetMember(memberId);
            if (member.InstitutionID != currentInst.ID && member.InstitutionID != 0)
            {
                throw new HttpException(401, "你不能为此会员申请执业登记");
            }
            try
            {
                var certNames = Request.Form["Cert.Name"].Split(',');
                var certNos = Request.Form["Cert.No"].Split(',');
                var certObtainDates = Request.Form["Cert.ObtainDate"].Split(',');
                for (var i = 0; i < certNames.Length; i++)
                {
                    var obtainDate = DateTime.Now;
                    DateTime.TryParse(certObtainDates[i], out obtainDate);
                    data.Certifications.Add(new Certification
                    {
                        Name = certNames[i],
                        CertificationNo = certNos[i],
                        ObtainDate = obtainDate == DateTime.MinValue ? default(Nullable<DateTime>) : obtainDate
                    });
                }

                var startDates = Request.Form["job.StartDate"].Split(',');
                var endDates = Request.Form["job.StartDate"].Split(',');
                var insts = Request.Form["job.Institution"].Split(',');
                var offices = Request.Form["job.Office"].Split(',');
                var notes = Request.Form["job.Note"].Split(',');
                for (var i = 0; i < startDates.Length; i++)
                {
                    var startDate = DateTime.MinValue;
                    DateTime.TryParse(startDates[i], out startDate);
                    var endDate = DateTime.MinValue;
                    DateTime.TryParse(endDates[i], out endDate);

                    data.Jobs.Add(new Job
                    {
                        StartDate = startDate == DateTime.MinValue ? null : startDate.ToShortDateString(),
                        EndDate = endDate == DateTime.MinValue ? null : endDate.ToShortDateString(),
                        Institution = insts[i],
                        Office = offices[i],
                        Note = notes[i]
                    });
                }

            }
            catch { }

            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Practice);
            if (checkLog == null || checkLog.Result.HasValue)
            {

                var practiceId = Core.PracticeManager.AddPracticeInfo(memberId, currentInst.ID, data);
                Core.CheckLogManager.AddCheckLog(practiceId, memberId, CheckType.Practice);
            }
            else
            {
                Core.PracticeManager.UpdatePracticeInfo(checkLog.ID, data);
            }


            return JsonSuccess();
        }

    }
}
