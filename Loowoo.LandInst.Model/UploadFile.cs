using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class UploadFile
    {
        public string FileName { get; set; }

        public string Description { get; set; }

        public string SavePath { get; set; }

        public static List<UploadFile> GetList(NameValueCollection requestForm)
        {
            var list = new List<UploadFile>();
            try
            {
                var names = requestForm["file.FileName"].Split(',');
                var descs = requestForm["file.Description"].Split(',');
                var savePaths = requestForm["file.SavePath"].Split(',');

                for (var i = 0; i < names.Length; i++)
                {
                    list.Add(new UploadFile
                    {
                        FileName = names[i],
                        Description = descs[i],
                        SavePath = savePaths[i]
                    });
                }

            }
            catch { }

            return list;
        }
    }
}
