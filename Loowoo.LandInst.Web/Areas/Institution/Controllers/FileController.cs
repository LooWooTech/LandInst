using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class FileController : InstitutionControllerBase
    {
        public ActionResult Upload(string fileName, int index = 0)
        {
            var inst = GetCurrentInst();
            if (Request.Files.Count == 0)
            {
                return JsonFail("没有选择上传文件");
            }

            HttpPostedFileBase file = null;
            for (var i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];
                if (file.ContentLength > 0)
                {
                    break;
                }
            }

            if (file.ContentLength == 0)
            {
                return JsonFail("文件不正确");
            }

            var filePath = Core.FileManager.Upload(HttpContext, file, inst.ID.ToString(), fileName);

            return JsonSuccess(filePath);
        }

    }
}