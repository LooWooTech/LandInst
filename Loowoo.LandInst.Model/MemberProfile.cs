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
            PracticeInfo = new PracticeInfo();
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


        public string MobilePhone { get; set; }

        public string Email { get; set; }

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

        /// <summary>
        /// 执业登记信息
        /// </summary>
        public PracticeInfo PracticeInfo { get; set; }

    }
}
