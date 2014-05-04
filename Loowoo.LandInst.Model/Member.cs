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
        public int ID { get; set; }

        public int InstitutionID { get; set; }

        public string IDNo { get; set; }

        public string RealName { get; set; }

        public string Birthday { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public MemberStatus Status { get; set; }

        public string Username { get; set; }

        public string InstitutionName { get; set; }
    }

    public enum MemberStatus
    {
        [Description("新注册用户")]
        NewUser,
        [Description("通过考试")]
        PassExam,
        [Description("机构从业人员")]
        Staff
    }
}
