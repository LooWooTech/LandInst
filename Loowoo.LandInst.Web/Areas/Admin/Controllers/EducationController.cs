using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;
using Loowoo.LandInst.Common;
using System.IO;

namespace Loowoo.LandInst.Web.Areas.Admin.Controllers
{
    public class EducationController : AdminControllerBase
    {
        public ActionResult Index(int page = 1)
        {
            ViewBag.List = Core.EducationManager.GetEducations();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            ViewBag.Model = Core.EducationManager.GetEducatoin(id) ?? new Education();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Education education)
        {
            Core.EducationManager.SaveEducation(education);
            return JsonSuccess();
        }

        public ActionResult Delete(int id)
        {
            Core.EducationManager.Delete(id);
            return JsonSuccess();
        }

        public ActionResult Approvals(string name, bool? hasCheck, int? eduId = 0, int page = 1)
        {
            var filter = new MemberFilter
            {
                Keyword = name,
                Page = new PageFilter { PageIndex = page },
                InfoID = eduId,
                HasCheck = hasCheck,
            };
            ViewBag.List = Core.EducationManager.GetApprovalEducations(filter);
            ViewBag.Educations = Core.EducationManager.GetEducations();
            return View();
        }

        public void Export(bool? hasCheck, int? eduId = 0, int page = 1)
        {
            var filter = new MemberFilter
            {
                Page = new PageFilter { PageIndex = page },
                InfoID = eduId,
                HasCheck = hasCheck,
            };

            var list = Core.EducationManager.GetApprovalEducations(filter);

            var filePath = Request.MapPath("/templates/继续教育导出模板.xls");
            var exportData = Core.EducationManager.GetExportData(list);
            if (exportData == null)
            {
                throw new Exception("该机构还未通过审核");
            }

            var fileName = "继续教育申请记录.xls";
            if (eduId.HasValue && eduId.Value > 0)
            {
                var edu = Core.EducationManager.GetEducatoin(eduId.Value);
                fileName = edu.Name + "-" + fileName;
            }

            var stream = NOPIHelper.WriteCell(filePath, exportData);
            Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", HttpUtility.UrlEncode(fileName)));
            Response.BinaryWrite(((MemoryStream)stream).GetBuffer());
            Response.End();
        }

        public ActionResult DeleteCheckLog(int id)
        {
            Core.CheckLogManager.Delete(id);
            return JsonSuccess();
        }
    }
}
