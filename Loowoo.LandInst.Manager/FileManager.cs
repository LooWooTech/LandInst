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

        public string Upload(HttpContextBase context, HttpPostedFileBase postedFile)
        {
            var savePath = context.Request.MapPath(_uploadRootDir + postedFile.FileName);
            postedFile.SaveAs(savePath);
            return savePath;
        }
    }
}
