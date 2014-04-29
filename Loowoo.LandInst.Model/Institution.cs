using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Institution
    {
        public int ID { get; set; }

        public InstitutionStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

        public InstitutionType Type { get; set; }

        /// <summary>
        /// 机构全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工商注册号
        /// </summary>
        public string RegistrationNo { get; set; }

        public string City { get; set; }

        public string MobilePhone { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalRepresentative { get; set; }
    }

    public enum InstitutionType
    {
        土地规划 = 1,
        土地勘测
    }

    public enum InstitutionStatus
    {

    }
}
