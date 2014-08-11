using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Certification
    {
        public Certification()
        {
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string CertificationNo { get; set; }

        /// <summary>
        /// 获取时间
        /// </summary>
        public DateTime? ObtainDate { get; set; }

        public string CertificationLevel { get; set; }
    }
}
