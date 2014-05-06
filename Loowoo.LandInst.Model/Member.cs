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
            Status = MemberStatus.NewUser;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int InstitutionID { get; set; }

        public string IDNo { get; set; }

        public string RealName { get; set; }

        public DateTime? Birthday { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        [Column(TypeName = "int")]
        public MemberStatus Status { get; set; }

        [NotMapped]
        public string Username { get; set; }

        [NotMapped]
        public string InstitutionName { get; set; }
    }

    public enum MemberStatus
    {
        [Description("新注册用户")]
        NewUser = 1,
        [Description("通过考试")]
        PassExam,
        [Description("机构从业人员")]
        Staff
    }
}
