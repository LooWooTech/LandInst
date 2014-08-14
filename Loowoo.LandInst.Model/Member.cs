using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("Member")]
    public class Member
    {
        public Member()
        {
            Status = MemberStatus.Normal;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int InstitutionID { get; set; }

        [NotMapped]
        public string InstitutionName { get; set; }

        public string RealName { get; set; }

        public DateTime? Birthday { get; set; }

        public string Gender { get; set; }

        [Column(TypeName = "int")]
        public Major Major { get; set; }

        [Column(TypeName = "int")]
        public EduRecord EduRecord { get; set; }

        public string IDNo { get; set; }
        ///// <summary>
        ///// 土地规划从业人员职业培训合格证号
        ///// </summary>
        //public string CertificationNo { get; set; }

        [Column(TypeName = "int")]
        public MemberStatus Status { get; set; }
    }

    public enum MemberStatus
    {
        [Description("新注册用户")]
        Normal = 0,

        [Description("已报名考试")]
        Registered = 1,

        [Description("正式会员")]
        Member = 2,

        [Description("执业人员")]
        Practice
    }


}
