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
            Status = MemberStatus.Registered;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int InstitutionID { get; set; }

        public string IDNo { get; set; }

        public string RealName { get; set; }

        public DateTime? Birthday { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        [Column(TypeName = "int")]
        public MemberStatus Status { get; set; }
    }

    public enum MemberStatus
    {
        [Description("新注册用户")]
        Normal = 0,

        [Description("已通过考试")]
        Registered = 1,

        [Description("执业人员")]
        Practice
    }


}
