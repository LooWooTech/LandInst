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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int InstitutionID { get; set; }

        [NotMapped]
        public string InstitutionName { get; set; }

        public string RealName { get; set; }

        public DateTime? Birthday { get; set; }

        public string Gender { get; set; }

        [Column(TypeName = "int")]
        public Major Major { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Column(TypeName = "int")]
        public EduRecord EduRecord { get; set; }

        public string IDNo { get; set; }

        [Column(TypeName = "int")]
        public ProfessionalLevel ProfessionalLevel { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        [Column(TypeName = "int")]
        public MemberStatus Status { get; set; }
    }

    public enum MemberStatus
    {
        [Description("新注册用户")]
        Normal = 0,

        [Description("培训合格")]
        Registered = 1,

        [Description("执业人员")]
        Practice
    }


}
