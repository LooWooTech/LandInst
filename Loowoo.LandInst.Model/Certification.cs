using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Certification
    {
        public string Name { get; set; }

        public string CertificationNo { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime? ObtainDate { get; set; }

        public string CertificationLevel { get; set; }

        public static List<Certification> GetList(NameValueCollection requestForm)
        {
            var list = new List<Certification>();
            try
            {
                var certNames = requestForm["Cert.Name"].Split(',');
                var certNos = requestForm["Cert.No"].Split(',');
                var certObtainDates = requestForm["Cert.ObtainDate"].Split(',');
                for (var i = 0; i < certNames.Length; i++)
                {
                    var obtainDate = DateTime.Now;
                    DateTime.TryParse(certObtainDates[i], out obtainDate);
                    list.Add(new Certification
                    {
                        Name = certNames[i],
                        CertificationNo = certNos[i],
                        ObtainDate = obtainDate == DateTime.MinValue ? default(Nullable<DateTime>) : obtainDate
                    });
                }
            }
            catch { }

            return list;
        }
    }
}