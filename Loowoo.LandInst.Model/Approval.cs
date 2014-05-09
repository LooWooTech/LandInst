using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("Approval")]
    public class Approval
    {
        public Approval()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int InfoID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool? Result { get; set; }

        public string Note { get; set; }

        [Column(TypeName = "int")]
        public ApprovalType ApprovalType { get; set; }

        [NotMapped]
        public bool IsLocked
        {
            get
            {
                return !ApprovalTime.HasValue || Result.HasValue;
            }
        }
    }

    public enum ApprovalType
    {
        Register,//注册、登记
        Change,//信息变更
        Transfer,//会员转移
        Working,//会员执业
        Annual,//年审
        Education,//继续教育
        Exam//报名考试
    }
}
