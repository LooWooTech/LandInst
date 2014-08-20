using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class PracticeInfo
    {
        public PracticeInfo()
        {
            Certifications = new List<Certification>();
            Jobs = new List<Job>();
        }

        /// <summary>
        /// 人事档案编号
        /// </summary>
        public string PersonalRecordsNO { get; set; }

        /// <summary>
        /// 个人社会保险编号
        /// </summary>
        public string SocialSecurityNO { get; set; }

        /// <summary>
        /// 人事档案存放机构
        /// </summary>
        public string PersonalRecordsInstitution { get; set; }

        /// <summary>
        /// 个人社会保险存放机构
        /// </summary>
        public string SocialSecurityInstitution { get; set; }

        /// <summary>
        /// 职业登记号
        /// </summary>
        public string PracticeRegistrationNO { get; set; }

        /// <summary>
        /// 资格证书号
        /// </summary>
        public string CertificationNO { get; set; }

        /// <summary>
        /// 职务
        /// </summary>
        public string Office { get; set; }

        /// <summary>
        /// 其他资格证书
        /// </summary>
        public List<Certification> Certifications { get; set; }

        /// <summary>
        /// 工作简历
        /// </summary>
        public List<Job> Jobs { get; set; }


        public string MobilePhone { get; set; }
    }

}
