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
            Status = MemberStatus.Register;
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
        public bool CanSingup
        {
            get { return Status == MemberStatus.Register; }
        }
    }

    public enum MemberStatus
    {
        /// <summary>
        /// 新注册的用户状态
        /// </summary>
        [Description("新注册用户")]
        Register = 1,

        /// <summary>
        /// 已批准报名用户状态
        /// </summary>
        [Description("报名考试")]
        SingupExam,

        /// <summary>
        /// 已考试通过的用户状态
        /// </summary>
        [Description("通过考试")]
        ExamSuccess,

        /// <summary>
        /// 已批准从业的用户状态
        /// </summary>
        [Description("从业人员")]
        Staff
    }


}
