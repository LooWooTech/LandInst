using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    /*个人社会保险存放机构、编号
人事档案存放机构、编号
起始从业时间
资格证书号
执业登记号
继续教育证书号
其他经济鉴证资格（一对多，包括经济鉴证资格名称、资格证书号、资格取得时间）
资料补充后点击提交将申请提交后台管理员审核。一旦提交则当前页面信息暂时不可修改。当前页面还负责显示是否审核通过、审核不通过的原因等。*/

    [Table("practice")]
    public class Practice
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int InstID { get; set; }

        public byte[] Data { get; set; }
    }

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
