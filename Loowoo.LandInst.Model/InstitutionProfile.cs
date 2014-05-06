using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [NotMapped]
    public class InstitutionProfile : Institution
    {

        /// <summary>
        /// 注册资金
        /// </summary>
        public string RegisteredCapital { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; }

        /// <summary>
        /// 注册证书号
        /// </summary>
        public string CertificateNo { get; set; }

        /// <summary>
        /// 利税总额
        /// </summary>
        public string TotalProfits { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        /// <summary>
        /// 经营期限
        /// </summary>
        public string OperatingPeriod { get; set; }

    }
}
