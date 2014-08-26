using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;
using System.IO;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class MemberController : InstitutionControllerBase
    {
        public ActionResult Index(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                InstID = Identity.UserID,
                Keyword = name,
                InInst = true,
                Page = new Model.Filters.PageFilter { PageIndex = page },
            };
            ViewBag.List = Core.MemberManager.GetMembers(filter);
            ViewBag.Page = filter.Page;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Profile = Core.MemberManager.GetProfile(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, CheckType? type, Member member, MemberProfile profile)
        {
            var inst = GetCurrentInst();
            member.InstitutionID = inst.ID;

            profile.Certifications = Certification.GetList(Request.Form);
            profile.Jobs = Job.GetList(Request.Form);

            if (id == 0)
            {
                var memberId = Core.MemberManager.AddMember(member);
                member.ID = memberId;
                Core.MemberManager.SaveProfile(member, profile);
                return JsonSuccess();
            }

            if (type.Value == CheckType.Profile)
            {
                Core.MemberManager.SubmitProfile(member, profile);

            }
            else if (type.Value == CheckType.Practice)
            {
                Core.MemberManager.SubmitPractice(member, profile);
            }
            else
            {
                //没参加考试的用户可以随时变更资料 不需要审核
                if (member.Status == MemberStatus.Normal)
                {
                    Core.MemberManager.UpdateMember(member);
                }
                Core.MemberManager.SaveProfile(member, profile);
            }

            return JsonSuccess();
        }

        public new ActionResult Profile(int id, int checkLogId = 0)
        {
            var inst = GetCurrentInst();
            var member = Core.MemberManager.GetMember(id);
            if (checkLogId > 0)
            {
                var checkLog = Core.CheckLogManager.GetCheckLog(checkLogId);
                ViewBag.CheckLog = checkLog;

                var profile = Core.MemberManager.GetProfile(checkLog);
                profile.SetMemberField(member);
                ViewBag.Profile = profile;
            }
            else
            {
                var profile = Core.MemberManager.GetProfile(id);
                profile.SetMemberField(member);
                ViewBag.Profile = profile;
            }

            ViewBag.Institution = inst;
            ViewBag.CheckLogs = Core.CheckLogManager.GetList(id);
            ViewBag.ExamResults = Core.ExamManager.GetVExamResults(new MemberFilter { UserID = id });
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
                InstID = GetCurrentInst().ID,
                GetInstName = true,
            };

            switch (target.ToLower())
            {
                case "transferin":
                    filter.InInst = false;
                    filter.MinStatus = MemberStatus.Registered;
                    break;
                case "transferout":
                case "practice":
                    filter.InInst = true;
                    filter.MinStatus = MemberStatus.Registered;
                    break;
                default:
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

            ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Transfer);
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
            ViewBag.CheckLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Transfer);
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
                Core.MemberManager.SubmitTransfer(member, instId);
            }
            else
            {
                Core.MemberManager.SubmitTransfer(member, instId);
            }


            return JsonSuccess();
        }

        public ActionResult Transfers(string name, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new Model.Filters.PageFilter { PageIndex = page },
                CheckType = CheckType.Transfer

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
                CheckType = CheckType.Practice,
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
            ViewBag.Profile = Core.MemberManager.GetProfile(memberId);
            var checkLog = Core.CheckLogManager.GetLastLog(memberId, CheckType.Practice);
            ViewBag.Institution = currentInst;
            ViewBag.CheckLog = checkLog;
            //ViewBag.Practice = Core.PracticeManager.GetPracticeInfo(memberId, currentInst.ID);
            return View();
        }


        [HttpPost]
        public ActionResult Practice(int memberId, MemberProfile profile)
        {
            var currentInst = GetCurrentInst();
            var member = Core.MemberManager.GetMember(memberId);
            if (member.InstitutionID != currentInst.ID && member.InstitutionID != 0)
            {
                throw new HttpException(401, "你不能为此会员申请执业登记");
                return JsonSuccess();
            }

            if (member.Status == MemberStatus.Practice)
            {
                Core.MemberManager.SaveProfile(member, profile);
                throw new Exception("该会员已申请过执业登记，如需修改资料请选择会员资料变更");
            }

            Core.MemberManager.SubmitPractice(member, profile);

            return JsonSuccess();
        }

        public void Export(int id, int checkLogId = 0)
        {
            var member = Core.MemberManager.GetMember(id);
            if (member == null)
            {
                throw new ArgumentException("没有找到该会员的资料");
            }

            var filePath = Request.MapPath("/templates/规划人员导出模板.xls");
            var exportData = Core.MemberManager.GetExportData(id, checkLogId);
            var stream = NOPIHelper.WriteCell(filePath, exportData);
            Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(member.RealName) + "(" + member.IDNo + ")" + ".xls"));
            Response.BinaryWrite(((MemoryStream)stream).GetBuffer());
            Response.End();
        }

    }
}
