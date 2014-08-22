using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Loowoo.LandInst.Manager
{
    public class FileManager : ManagerBase
    {
        private string _uploadRootDir = "/uploadfiles/";

        public string Upload(HttpContextBase context, HttpPostedFileBase postedFile, string dirName = null, string newFileName = null)
        {
            var virtualPath = _uploadRootDir + (string.IsNullOrEmpty(dirName) ? null : (dirName + "/"));

            var dirPath = context.Request.MapPath(virtualPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var fileName = postedFile.FileName;

            var tmpIndex = fileName.LastIndexOf('.');

            var fileExt = fileName.Substring(tmpIndex, fileName.Length - tmpIndex);

            if (string.IsNullOrEmpty(newFileName))
            {
                newFileName = DateTime.Now.Ticks.ToString() + fileExt;
            }
            else
            {
                newFileName = newFileName.Replace("/", "-").Replace(".", "-") + fileExt;
            }

            var savePath = Path.Combine(dirPath, newFileName);

            postedFile.SaveAs(savePath);

            return virtualPath + newFileName;
        }
    }
}