using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [NotMapped]
    public class MemberProfile : Member
    {
        public MemberProfile()
        {
            IsFullTime = true;
            Certifications = new List<Certification>();
            Jobs = new List<Job>();
        }

        public MemberProfile(Member member):this()
        {
            foreach (var p in member.GetType().GetProperties())
            {
                var val = p.GetValue(member, null);

                var selfP = this.GetType().GetProperty(p.Name);
                selfP.SetValue(this, val, null);
            }
        }

        public void SetMemberField(Member member)
        {
            ID = member.ID;
            Status = member.Status;
            InstitutionID = member.InstitutionID;
        }

        /// <summary>
        /// 职称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 毕业学校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 学位
        /// </summary>
        public string EduLevel { get; set; }

        /// <summary>
        /// 毕业日期
        /// </summary>
        public DateTime? GraduationDate { get; set; }

        /// <summary>
        /// 任职时间
        /// </summary>
        public DateTime? StartWorkingDate { get; set; }

        /// <summary>
        /// 从事土地规划工作年限
        /// </summary>
        public int WorkingYears { get; set; }

        /// <summary>
        /// 现从事的工作
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        public string PoliticalState { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Nationality { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        public string Address { get; set; }

        public bool IsFullTime { get; set; }

        /// <summary>
        /// 其他资格证书
        /// </summary>
        public List<Certification> Certifications { get; set; }

        /// <summary>
        /// 工作简历
        /// </summary>
        public List<Job> Jobs { get; set; }


        #region 执业资料

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
        /// 照片1（证件正面）
        /// </summary>
        public string Photo1 { get; set; }

        /// <summary>
        /// 照片2（证件反面）
        /// </summary>
        public string Photo2 { get; set; }

        #endregion

    }
}
