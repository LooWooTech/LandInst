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
        public MemberProfile() { }

        public MemberProfile(Member member)
        {
            SetMemberField(member);
        }

        public void SetMemberField(Member member)
        {
            ID = member.ID;
            RealName = member.RealName;
            InstitutionID = member.InstitutionID;
            MobilePhone = member.MobilePhone;
            Status = member.Status;
            IDNo = member.IDNo;
            Email = member.Email;
            Birthday = member.Birthday;
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
        /// 专业
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string EduRecord { get; set; }

        /// <summary>
        /// 学位
        /// </summary>
        public string EduLevel { get; set; }

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
    }
}
